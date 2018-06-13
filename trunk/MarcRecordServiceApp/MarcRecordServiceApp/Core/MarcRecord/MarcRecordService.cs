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
        private readonly MarcRecordProviderType _marcRecordType;

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
                try
                {
                    //int queryLoopCount = 0;
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
                                //queryLoopCount++;
                                PrefixQuery prefixQuery = new PrefixQuery(query);
                                IResultSet resultSet;
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
                                            var marcFile = marcFiles.FirstOrDefault(x => x.Product.Isbn10 == isbn || x.Product.Isbn13 == isbn);
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
                        foreach (IMarcFile marcFile in marcFiles)
                        {
                            string mrkFileText = GetRbdMrkFileText2(marcFile);
                            string mrcFileText = marc21.Mnemonic2Stream(mrkFileText);
                            string xmlFileText = marc21.MARC2MARC21XML_Stream(mrcFileText, false);

                            marcFile.MrcFileText = mrcFileText;
                            marcFile.XmlFileText = xmlFileText;
                            marcFile.MrkFileText = mrkFileText;
                            foundRecords++;
                        }
                    }

                    Log.Info($"GetMarcRecords --- It took {timer.ElapsedMilliseconds}ms to set {foundRecords} out of {marcFiles.Count} Marc Records");
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

                foreach (XmlNode node in xmlNodeList.Cast<XmlNode>().Where(node =>
                    node.Attributes != null && Convert.ToInt32(node.Attributes["tag"].InnerText) == 20))
                {
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode xmlNode in node.ChildNodes)
                        {
                            string nodeInnerText = xmlNode.InnerText.Replace(".", "");
                            if (!string.IsNullOrWhiteSpace(nodeInnerText))
                            {
                                int space = nodeInnerText.IndexOf(" ", StringComparison.Ordinal);
                                int seperator = nodeInnerText.IndexOf("(", StringComparison.Ordinal);
                                if (space > 0 || seperator > 0)
                                {
                                    isbns.Add(space > 0
                                        ? nodeInnerText.Substring(0, space)
                                        : nodeInnerText.Substring(0, seperator));
                                }
                                else
                                {
                                    isbns.Add(nodeInnerText);
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        string nodeInnerText = node.InnerText.Replace(".", "");
                        if (!string.IsNullOrWhiteSpace(nodeInnerText))
                        {
                            if (nodeInnerText.Length > 13)
                            {
                                int space = nodeInnerText.IndexOf(" ", StringComparison.Ordinal);
                                isbns.Add(space > 0
                                    ? nodeInnerText.Substring(0, space)
                                    : nodeInnerText.Substring(0, node.InnerText.IndexOf("(", StringComparison.Ordinal)));
                            }
                            else
                            {
                                isbns.Add(nodeInnerText);
                            }
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

        //private string GetRbdMrkFileText(Product product)
        //{
        //    try
        //    {
        //        string publicationYearText = product.PublicationYearText;

        //        if (publicationYearText == "")
        //        {
        //            Log.Debug($"Id: {product.Id}, Sku: {product.Sku}, Isbn13: {product.Isbn13}");
        //        }

        //        StringBuilder mrkFileText = new StringBuilder();
        //        string sitepath = Settings.Default.SiteSubDirectory;

        //        mrkFileText.AppendLine(
        //            $"=LDR  {GetNext5DigitRandomNumber()}nam  22{GetNext5DigitRandomNumber()}2a 4500");

        //        Log.Debug(
        //            $"PublicationYearText: {product.PublicationYearText}, PublicationYear: {product.PublicationYear}, sku: {product.Sku}");

        //        string line008 =
        //            $"=008  {_currentDateTime:yyMMdd}s{product.PublicationYear:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ";

        //        Log.DebugFormat("line008: {0}", line008);
        //        Log.DebugFormat("line008.Length: {0}", line008.Length);

        //        mrkFileText.AppendLine($"=001  {product.Sku}");
        //        mrkFileText.AppendLine($"=005  {DateTime.Now:yyyyMMddhhmmss}.0");
        //        mrkFileText.AppendLine(line008);



        //        if (!string.IsNullOrWhiteSpace(product.Isbn10))
        //        {
        //            mrkFileText.AppendLine($"=020  \\\\$a{product.Isbn10}");
        //        }

        //        if (!string.IsNullOrWhiteSpace(product.Isbn13))
        //        {
        //            mrkFileText.AppendLine($"=020  \\\\$a{product.Isbn13}");
        //        }

        //        mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc");


        //        mrkFileText.AppendLine($"=100  1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}");
        //        mrkFileText.AppendLine($"=245  10$a{StripOffCarriageReturnAndLineFeed(product.Title)}");
        //        mrkFileText.AppendLine($"=260  \\\\$b{product.PublisherName},$c{publicationYearText}");
        //        mrkFileText.AppendLine(
        //            $"=533  \\\\$a{product.Format}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{publicationYearText}");
        //        mrkFileText.AppendLine($"=650  \\4$a{product.CategoryName}.");


        //        mrkFileText.AppendLine($"=700  1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}");
        //        mrkFileText.AppendLine(
        //                $"=856  4\\$zConnect to this resource online$u{sitepath}Products/Book.aspx?sku={product.Isbn10}")
        //            .AppendLine();


        //        return mrkFileText.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (product != null)
        //        {
        //            Log.Info(product.ToString());
        //            Log.InfoFormat("Id: {0}", product.Id);
        //            Log.InfoFormat("Sku: {0}", product.Sku);
        //            Log.InfoFormat("Isbn10: {0}", product.Isbn10);
        //            Log.InfoFormat("Isbn13: {0}", product.Isbn13);
        //            Log.InfoFormat("Title: {0}", product.Title);
        //            Log.InfoFormat("Authors: {0}", product.Authors);
        //            Log.InfoFormat("PublicationYearText: {0}", product.PublicationYearText);
        //            Log.InfoFormat("PublicationDate: {0}", product.PublicationDate);
        //            Log.InfoFormat("Copyright: {0}", product.Copyright);
        //            Log.InfoFormat("Format: {0}", product.Format);
        //            Log.InfoFormat("CategoryName: {0}", product.CategoryName);
        //        }
        //        else
        //        {
        //            Log.Info("Product is null!");
        //        }

        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }

        //}

        private string GetRbdMrkFileText2(IMarcFile marcFile)
        {
            Product product = marcFile.Product;
            List<AdditionalField> additionalFields = marcFile.AdditionalFields;
            try
            {
                StringBuilder mrkFileText = new StringBuilder();
                string sitepath = Settings.Default.SiteSubDirectory;

                mrkFileText.AppendMarcValue($"{GetNext5DigitRandomNumber()}nam  22{GetNext5DigitRandomNumber()}2a 4500",additionalFields);
                mrkFileText.AppendMarcValue(product.Sku, additionalFields, 1);
                mrkFileText.AppendMarcValue($"{DateTime.Now:yyyyMMddhhmmss}.0", additionalFields, 5);
                mrkFileText.AppendMarcValue($"{_currentDateTime:yyMMdd}s{product.PublicationYear:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ",additionalFields, 8);


                if (!string.IsNullOrWhiteSpace(product.Isbn10))
                {
                    mrkFileText.AppendMarcValue($"\\\\$a{product.Isbn10}", additionalFields, 20);
                }

                if (!string.IsNullOrWhiteSpace(product.Isbn13))
                {
                    mrkFileText.AppendMarcValue($"\\\\$a{product.Isbn13}", additionalFields, 20);
                }

                mrkFileText.AppendMarcValue("\\\\$bRittenhouse Book Distributors, Inc", additionalFields, 37);
                mrkFileText.AppendMarcValue($"1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}",additionalFields, 100);
                mrkFileText.AppendMarcValue($"10$a{StripOffCarriageReturnAndLineFeed(product.Title)}", additionalFields,245);
                mrkFileText.AppendMarcValue($"\\\\$b{product.PublisherName}{(string.IsNullOrWhiteSpace(product.PublicationYearText) ? "" : $",$c{product.PublicationYearText}")}", additionalFields,260);
                mrkFileText.AppendMarcValue($"\\\\$a{product.Format}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc{(string.IsNullOrWhiteSpace(product.PublicationYearText) ? "" : $",$d{product.PublicationYearText}")}",additionalFields, 533);
                mrkFileText.AppendMarcValue($"\\4$a{product.CategoryName}.", additionalFields, 650);

                mrkFileText.AppendMarcValue($"1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}",additionalFields, 700);
                mrkFileText.AppendMarcValue($"4\\$zConnect to this resource online$u{sitepath}Products/Book.aspx?sku={product.Isbn10}",additionalFields, 856);
                mrkFileText.AppendMarcValue(additionalFields);
                mrkFileText.AppendLine();

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


        public R2LibraryMarcFile GetR2LibraryMarcFile(R2Resource resource)
        {
            List<AdditionalField> additionalFields = resource.AdditionalFields;

            StringBuilder mrkFileText = new StringBuilder();

            mrkFileText.AppendMarcValue($"{GetNext5DigitRandomNumber()}nam  22{GetNext5DigitRandomNumber()}2a 4500", additionalFields);
            mrkFileText.AppendMarcValue(resource.Isbn, additionalFields, 1);
            mrkFileText.AppendMarcValue($"{DateTime.Now:yyyyMMddhhmmss}.0", additionalFields, 5);
            mrkFileText.AppendMarcValue($"{_currentDateTime:yyMMdd}s{resource.PublicationYear:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ", additionalFields, 8);



            var firstAuthorString = resource.AuthorList.Any()
                ? resource.AuthorList.First(x => x.Order == 1).ToDisplayName()
                : resource.FirstAuthor;

            if (!string.IsNullOrWhiteSpace(resource.Isbn10))
            {
                mrkFileText.AppendMarcValue($"\\\\$a{resource.Isbn10}", additionalFields, 20);
            }

            if (!string.IsNullOrWhiteSpace(resource.Isbn13))
            {
                mrkFileText.AppendMarcValue($"\\\\$a{resource.Isbn13}", additionalFields, 20);
            }

            if (!string.IsNullOrWhiteSpace(resource.EIsbn))
            {
                mrkFileText.AppendMarcValue($"\\\\$a{resource.EIsbn}", additionalFields, 20);

            }

            mrkFileText.AppendMarcValue("\\\\$bRittenhouse Book Distributors, Inc", additionalFields, 37);
            mrkFileText.AppendMarcValue($"1\\$a{StripOffCarriageReturnAndLineFeed(firstAuthorString)}", additionalFields, 100);
            mrkFileText.AppendMarcValue($"10$a{StripOffCarriageReturnAndLineFeed(resource.Title)}", additionalFields, 245);
            mrkFileText.AppendMarcValue($"\\\\$b{resource.PublisherName},$c{resource.PublicationYear}", additionalFields, 260);


            mrkFileText.AppendMarcValue("\\\\$a online resource", additionalFields, 300);
            mrkFileText.AppendMarcValue($"\\\\$aeBook.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{resource.PublicationYear}", additionalFields, 533);
            mrkFileText.AppendMarcValue("\\\\$a Mode of Access: World Wide Web", additionalFields, 538);


            WriteCategories(mrkFileText, resource, additionalFields); //650

            mrkFileText.AppendMarcValue("\\4$a Electronic books", additionalFields, 655);

            if (resource.AuthorList != null)
            {
                foreach (var r2Author in resource.AuthorList)
                {
                    mrkFileText.AppendMarcValue($"1\\$a{r2Author.ToDisplayName()}", additionalFields, 700);
                }
            }
            else
            {
                mrkFileText.AppendMarcValue($"1\\$a{resource.FirstAuthor}", additionalFields, 700);
            }

            mrkFileText.AppendMarcValue($"4\\$zConnect to this resource online$u{Settings.Default.R2WebSite}{resource.Isbn}", additionalFields, 856);

            mrkFileText.AppendMarcValue(additionalFields);

            return new R2LibraryMarcFile
            {
                MrkText = mrkFileText.ToString(),
                ProviderSourceId = 4,
                Isbn = resource.Isbn,
                Isbn10 = resource.Isbn10,
                Isbn13 = resource.Isbn13,
                EIsbn = resource.EIsbn
            };
        }

        public static void WriteCategories(StringBuilder mrkFileText, R2Resource resource, List<AdditionalField> additionalFields)
        {
            if (resource.Categories != null || resource.SubCategories != null)
            {
                var categories = resource.Categories?.ToList();
                var subCategories = resource.SubCategories?.ToList();
                if (categories != null && subCategories != null)
                {
                    if (categories.Count == subCategories.Count)
                    {
                        for (int i = 0; i < categories.Count; i++)
                        {
                            mrkFileText.AppendMarcValue($"\\4$a{categories[i].Category}$x{subCategories[i].SubCategory}", additionalFields,650);
                        }
                    }
                    else if (categories.Count > subCategories.Count && subCategories.Count == 1)
                    {
                        if (subCategories.Count == 1)
                        {
                            foreach (var r2Category in categories)
                            {
                                mrkFileText.AppendMarcValue($"\\4$a{r2Category.Category}$x{subCategories[0].SubCategory}", additionalFields,650);
                            }
                        }
                    }
                    else
                    {
                        foreach (var r2Category in categories)
                        {
                            foreach (var r2SubCategory in subCategories)
                            {
                                mrkFileText.AppendMarcValue($"\\4$a{r2Category.Category}$x{r2SubCategory.SubCategory}",additionalFields, 650);
                            }
                        }
                    }
                }
                else if (categories != null) //subCategories == null
                {
                    foreach (var r2Category in categories)
                    {
                        mrkFileText.AppendMarcValue($"\\4$a{r2Category.Category}", additionalFields, 650);
                    }
                }
                else
                {
                    foreach (var r2SubCategories in subCategories)
                    {
                        mrkFileText.AppendMarcValue($"\\4$a{r2SubCategories.SubCategory}", additionalFields, 650);
                    }
                }
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

    public static class StringBuilderExtension
    {
        public static void AppendMarcValue(this StringBuilder sb, string fieldValue,
            List<AdditionalField> additionalFields, int fieldNumber = 0)
        {
            if (fieldNumber == 0)
            {
                sb.AppendLine($"=LDR  {fieldValue}");
            }
            else
            {
                if (additionalFields != null && additionalFields.Any())
                {
                    var additionalFieldsFound = additionalFields.Where(x => x.FieldNumber < fieldNumber).ToList();
                    if (additionalFieldsFound.Any())
                    {
                        additionalFieldsFound = additionalFieldsFound.OrderBy(x => x.FieldNumber).ToList();
                        foreach (var additionalField in additionalFieldsFound)
                        {
                            sb.AppendLine(additionalField.Value);
                            additionalFields.Remove(additionalField);
                        }
                    }
                }


                sb.AppendLine($"={fieldNumber:000}  {fieldValue}");
            }
        }

        public static void AppendMarcValue(this StringBuilder sb, List<AdditionalField> additionalFields)
        {

            if (additionalFields != null && additionalFields.Any())
            {
                additionalFields = additionalFields.OrderBy(x => x.FieldNumber).ToList();
                foreach (var additionalField in additionalFields)
                {
                    sb.AppendLine(additionalField.Value);
                }
            }


        }

    }
}