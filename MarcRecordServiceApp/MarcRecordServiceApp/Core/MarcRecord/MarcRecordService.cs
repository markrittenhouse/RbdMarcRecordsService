using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MARCEngine5;
using Zoom.Net;
using Zoom.Net.YazSharp;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class MarcRecordService
    {
        private MarcRecordProviderType _marcRecordType;
        public MarcRecordService(MarcRecordProviderType type)
        {
            _marcRecordType = type;
        }
        private readonly Random _random = new Random();
        private readonly DateTime _currentDateTime = DateTime.Now;

        protected static readonly ILog Log = LogManager.GetLogger(typeof(MarcRecordService));

        public int SetMarcRecords(List<IMarcFile> marcFiles)
        {
            int foundRecords = 0;
            int processAttempts = 0;
            //Loop five times if a failure
            while (processAttempts < 5)
            {
                IResultSet resultSet;
                try
                {
                    int queryLoopCount = 0;
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    MARC21 marc21 = new MARC21();

                    if (_marcRecordType != MarcRecordProviderType.Rbd)
                    {
                        List<string> queries = new List<string>(GetMarcQuery(marcFiles));

                        MarcRecordProviderValue serverValue = new MarcRecordProviderValue(_marcRecordType);


                        processAttempts++;
                        using (Connection lcConnection = new Connection(serverValue.ToString(), 7090)
                        {
                            DatabaseName = "Voyager",
                            Syntax = RecordSyntax.USMARC
                        })
                        {
                            lcConnection.Connect();

                            Log.Info("Zoom-Connected");
                            foreach (string query in queries)
                            {
                                queryLoopCount++;
                                PrefixQuery prefixQuery = new PrefixQuery(query);
                                using (resultSet = lcConnection.Search(prefixQuery))
                                {
                                    foreach (IRecord record in resultSet)
                                    {
                                        string mrcFileText = Encoding.UTF8.GetString(record.Content);
                                        if (mrcFileText.Length <= 0)
                                        {
                                            continue;
                                        }

                                        string mrkFileText = marc21.MARC2Stream(mrcFileText);
                                        string xmlFileText = marc21.MARC2MARC21XML_Stream(mrcFileText, false);

                                        var isbns = ExtractIsbnsFromXml(xmlFileText);

                                        foreach (string isbn in isbns)
                                        {
                                            var marcFile = marcFiles.FirstOrDefault(x =>
                                                x.Product.Isbn10 == isbn || x.Product.Isbn13 == isbn);
                                            if (marcFile != null)
                                            {
                                                marcFile.MrcFileText = mrcFileText;
                                                marcFile.XmlFileText = xmlFileText;
                                                marcFile.MrkFileText = mrkFileText;
                                                foundRecords++;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        Log.Info("Zoom-Disconnected");
                    }
                    else
                    {
                        //MarcFieldParsingFactory marcFieldParsingFactory = new MarcFieldParsingFactory();
                        //var skuList = marcFiles.Select(x => x.Product.Sku).ToList();
                        //List<ParsedMarcField> allParsedMarcFields = marcFieldParsingFactory.GetParsedMarcFields(skuList);
                        //MARC21 marc21 = new MARC21();

                        foreach (IMarcFile marcFile in marcFiles)
                        {
                            //List<ParsedMarcField> parsedMarcFields = allParsedMarcFields?.Where(x => x.Sku == marcFile.Product.Sku).ToList();

                            Product product = marcFile.Product;
                            string mrkFileText = GetRbdMrkFileText(product);
                            string mrcFileText = marc21.Mnemonic2Stream(mrkFileText);
                            string xmlFileText = marc21.MARC2MARC21XML_Stream(mrcFileText, false);

                            marcFile.MrcFileText = mrcFileText;
                            marcFile.XmlFileText = xmlFileText;
                            marcFile.MrkFileText = mrkFileText;
                            foundRecords++;
                        }
                    }

                    Log.Info(
                        $"GetMarcRecords --- It took {timer.ElapsedMilliseconds}ms to set {foundRecords} out of {marcFiles.Count} Marc Records");
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    processAttempts++;
                }
            }

            return foundRecords;
        }

        private static IEnumerable<string> GetMarcQuery(List<IMarcFile> marcFiles)
        {
            List<string> queries = new List<string>();

            int batchCount = 10;
            int skipCounter = 0;
            var marcRecords = marcFiles.Skip(skipCounter * batchCount).Take(batchCount).ToList();
            while (marcRecords.Any())
            {
                var queryBuilder = new StringBuilder();
                var orBuilder = new StringBuilder();

                foreach (var marcRecord in marcRecords)
                {
                    if (queryBuilder.Length > 0)
                    {
                        orBuilder.Append("@or ");
                    }
                    queryBuilder.Append($" @attr 1=7 {marcRecord.Product.Isbn13 ?? marcRecord.Product.Isbn10} ");
                }
                queries.Add($"{orBuilder}{queryBuilder}");

                skipCounter++;
                marcRecords = marcFiles.Skip(skipCounter * batchCount).Take(batchCount).ToList();
            }

            return queries;
        }

        private IEnumerable<string> ExtractIsbnsFromXml(string xmlString)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNodeList xmlNodeList = doc.GetElementsByTagName("datafield");

                List<string> isbns = new List<string>();

                foreach (XmlNode node in xmlNodeList.Cast<XmlNode>().Where(node => node.Attributes != null && Convert.ToInt32(node.Attributes["tag"].InnerText) == 20))
                {
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode xmlNode in node.ChildNodes)
                        {
                            string nodeInnterText = xmlNode.InnerText.Replace(".", "");
                            int space = nodeInnterText.IndexOf(" ");
                            int seperator = nodeInnterText.IndexOf("(");
                            if (space > 0 || seperator > 0)
                            {
                                isbns.Add(space > 0
                                          ? nodeInnterText.Substring(0, space)
                                          : nodeInnterText.Substring(0, nodeInnterText.IndexOf("(")));
                            }
                            else
                            {
                                isbns.Add(nodeInnterText);
                            }
                        }
                    }
                    else
                    {
                        string nodeInnterText = node.InnerText.Replace(".", "");
                        if (nodeInnterText.Length > 13)
                        {
                            int space = nodeInnterText.IndexOf(" ");
                            isbns.Add(space > 0
                                          ? nodeInnterText.Substring(0, space)
                                          : nodeInnterText.Substring(0, node.InnerText.IndexOf("(")));
                        }
                        else
                        {
                            isbns.Add(nodeInnterText);
                        }
                    }
                }

                return isbns;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        private string GetRbdMrkFileText(Product product)
        {
            try
            {
                string publicationYearText = product.PublicationYearText;

                if (publicationYearText == "")
                {
                    Log.Debug($"Id: {product.Id}, Sku: {product.Sku}, Isbn13: {product.Isbn13}");
                }

                StringBuilder mrkFileText = new StringBuilder();
                string sitepath = Settings.Default.SiteSubDirectory;

                mrkFileText.AppendLine($"=LDR  {GetNext5DigitRandomNumber()}nam  22{GetNext5DigitRandomNumber()}2a 4500");

                Log.Debug($"PublicationYearText: {product.PublicationYearText}, PublicationYear: {product.PublicationYear}, sku: {product.Sku}");


                string line008 = $"=008  {_currentDateTime:yyMMdd}s{product.PublicationYear:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ";

                Log.DebugFormat("line008: {0}", line008);
                Log.DebugFormat("line008.Length: {0}", line008.Length);

                mrkFileText.AppendLine($"=001  {product.Sku}");
                mrkFileText.AppendLine($"=005  {DateTime.Now:yyyyMMddhhmmss}.0");
                mrkFileText.AppendLine(line008);

                if (!string.IsNullOrWhiteSpace(product.Isbn10))
                {
                    mrkFileText.AppendLine($"=020  \\\\$a{product.Isbn10}");
                }

                if (!string.IsNullOrWhiteSpace(product.Isbn13))
                {
                    mrkFileText.AppendLine($"=020  \\\\$a{product.Isbn13}");
                }

                //var parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.OclcNumber).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}

                mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc");

                //parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.NlmNumber).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}

                //parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.LcNumber).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}
                mrkFileText.AppendLine($"=100  1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}");
                mrkFileText.AppendLine($"=245  10$a{StripOffCarriageReturnAndLineFeed(product.Title)}");
                mrkFileText.AppendLine($"=260  \\\\$b{product.PublisherName},$c{publicationYearText}");
                mrkFileText.AppendLine($"=533  \\\\$a{product.Format}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{publicationYearText}");
                mrkFileText.AppendLine($"=650  \\4$a{product.CategoryName}.");


                //parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.NlmSubject).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}
                mrkFileText.AppendLine($"=700  1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}");
                mrkFileText.AppendLine($"=856  4\\$zConnect to this resource online$u{sitepath}Products/Book.aspx?sku={product.Isbn10}").AppendLine();

                return mrkFileText.ToString();
            }
            catch (Exception ex)
            {
                if (product != null)
                {
                    Log.Info(product.ToString());
                    Log.InfoFormat("Id: {0}", product.Id);
                    Log.InfoFormat("Sku: {0}", product.Sku);
                    Log.InfoFormat("Isbn10: {0}", product.Isbn10);
                    Log.InfoFormat("Isbn13: {0}", product.Isbn13);
                    Log.InfoFormat("Title: {0}", product.Title);
                    Log.InfoFormat("Authors: {0}", product.Authors);
                    Log.InfoFormat("PublicationYearText: {0}", product.PublicationYearText);
                    Log.InfoFormat("PublicationDate: {0}", product.PublicationDate);
                    Log.InfoFormat("Copyright: {0}", product.Copyright);
                    Log.InfoFormat("Format: {0}", product.Format);
                    Log.InfoFormat("CategoryName: {0}", product.CategoryName);
                }
                else
                {
                    Log.Info("Product is null!");
                }
                Log.Error(ex.Message, ex);
                throw;
            }

        }

        public string StripOffCarriageReturnAndLineFeed(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        public int GetNext5DigitRandomNumber()
        {
            int next = _random.Next(10000, 99999);
            return next;
        }
    }
}
