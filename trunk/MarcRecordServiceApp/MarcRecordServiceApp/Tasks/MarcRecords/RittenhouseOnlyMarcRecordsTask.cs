using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MARCEngine5;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class RittenhouseOnlyMarcRecordsTask : TaskBase2
    {
        private readonly Random _random = new Random();
        private readonly DateTime _currentDateTime = DateTime.Now;
        private readonly StringBuilder _results = new StringBuilder();

        private int BatchSize { get; set; }

        private int _recordsProcessed;

        private int _recordCountBeingProcessed;


        public RittenhouseOnlyMarcRecordsTask()
            : base("Generate Missing Rittenhouse Only MArC Records", "CreateMarcRecords")
        {
            BatchSize = Settings.Default.RbdMarcRecordMaxProducts;
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "MarcRecords.Generator",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {
                step.CompletedSuccessfully = ProcessMarcRecordBatchs();
            }
            catch (Exception ex)
            {
                step.Results.Insert(0, string.Format("EXCEPTION: {0}", ex.Message));
                step.CompletedSuccessfully = false;
                throw;
            }
            finally
            {
                step.EndTime = DateTime.Now;
                TaskResultFactory.InsertTaskResultStep(step);
            }
        }


        private bool ProcessMarcRecordBatchs()
        {
            try
            {
                //Set workingDirectory
                string workingDirectory = (Settings.Default.MarcFilesWorkingDirectory.EndsWith(@"\"))
                                              ? Settings.Default.MarcFilesWorkingDirectory
                                              : string.Format(@"{0}\",
                                                              Settings.Default.MarcFilesWorkingDirectory);
                Log.InfoFormat("workingDirectory:\n{0}", workingDirectory);
                Log.InfoFormat("MarcRecordGeneratorMaxProducts: {0}", BatchSize);
                Log.InfoFormat("MarcRecordBatchSize: {0}", Settings.Default.MarcRecordBatchSize);

                //Set Marc Records to get
                _recordCountBeingProcessed = MarcRecordsProductFactory.GetProductsWithoutMarcRecordsCount();
                Log.InfoFormat("productsWithoutMarcRecordsCount: {0}", _recordCountBeingProcessed);


                ClearWorkingDirectory(workingDirectory);


                int newProductsProcessed = 0;
                int batchProductProcessed;
                while ((batchProductProcessed = ProcessNextBatchOfNewProducts(Settings.Default.MarcRecordBatchSize, workingDirectory)) > 0)
                {
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    newProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("newProductsProcessed: {0}", newProductsProcessed);
                    //Want to process them ALL!!!
                    if (newProductsProcessed >= _recordCountBeingProcessed)
                    {
                        break;
                    }
                    ClearWorkingDirectory(workingDirectory);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                _results.Insert(0, string.Format("EXCEPTION: {0}\n\r\t", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int ProcessNextBatchOfNewProducts(int batchSize, string workingDirectory)
        {

            int productProcessedCount = 0;
            int processedAttempts = 0;
            //Loop three times if a failure
            while (processedAttempts < 3)
            {
                try
                {
                    // get next batch to process
                    List<IMarcFile> marcFiles = MarcRecordsProductFactory.GetProductsWithoutMarcRecords(batchSize);

                    Log.InfoFormat("batch contains {0} products", marcFiles.Count);
                    if (marcFiles.Count == 0)
                    {
                        return 0;
                    }

                    marcFiles = SetMarcDataForProducts(marcFiles, workingDirectory);

                    foreach (int rowsSaved in marcFiles.Select(MarcRecordsProductFactory.InsertUpdateMarcRecordFile))
                    {
                        Log.DebugFormat("rowsSaved: {0}", rowsSaved);
                        productProcessedCount++;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    processedAttempts++;
                }
            }

            return productProcessedCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <param name="workingDirectory"></param>
        private List<IMarcFile> SetMarcDataForProducts(List<IMarcFile> marcFiles, string workingDirectory)
        {
            List<string> mrcStrings = new List<string>(GetMarcRecords(marcFiles, workingDirectory));

            try
            {
                foreach (string mrcString in mrcStrings)
                {
                    string batchFileNameBase = string.Format("batch_{0:yyyyMMdd_HHmmssfff}", DateTime.Now);

                    string mrkFilePath = string.Format(@"{0}{1}.mrk", workingDirectory, batchFileNameBase);
                    Log.DebugFormat("mrkFilePath: {0}", mrkFilePath);

                    string mrcFilePath = string.Format(@"{0}{1}.mrc", workingDirectory, batchFileNameBase);
                    Log.DebugFormat("mrcFilePath: {0}", mrcFilePath);

                    string xmlFilePath = string.Format(@"{0}{1}.xml", workingDirectory, batchFileNameBase);
                    Log.DebugFormat("xmlFilePath: {0}", xmlFilePath);

                    File.WriteAllText(mrcFilePath, mrcString);

                    MARC21 marc21 = new MARC21();
                    marc21.MarcFile(mrcFilePath, mrkFilePath);

                    if (new FileInfo(mrkFilePath).Length == 0)
                    {
                        //Indicates an issue with the record or the process
                        continue;
                    }
                    string mrkFileText = ReadMarcFile(mrkFilePath);
                    marc21.MARC2MARC21XML(mrcFilePath, xmlFilePath, false);

                    if (new FileInfo(xmlFilePath).Length == 0)
                    {
                        //Indicates an issue with the record or the process
                        continue;
                    }
                    string xmlFileText = ReadMarcFile(xmlFilePath);

                    IEnumerable<string> isbns = ExtractIsbnsFromXml(xmlFileText);

                    foreach (string isbn in isbns)
                    {
                        string isbn1 = isbn;
                        foreach (IMarcFile marcFile in marcFiles.Where(marcFile => marcFile.Product.Isbn10 == isbn1 || marcFile.Product.Isbn13 == isbn1))
                        {
                            marcFile.MrcFileText = mrcString;
                            marcFile.XmlFileText = xmlFileText;
                            marcFile.MrkFileText = mrkFileText;
                            break;
                        }
                    }
                }
                return marcFiles;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private IEnumerable<string> GetMarcRecords(List<IMarcFile> marcFiles, string workingDirectory)
        {
            List<string> mrcStrings = new List<string>();
            int processAttempts = 0;
            //Loop five times if a failure
            while (processAttempts < 5)
            {
                try
                {
                    foreach (IMarcFile marcFile in marcFiles)
                    {
                        _recordsProcessed++;
                        Log.InfoFormat("Processing {0} of {1}", _recordsProcessed, _recordCountBeingProcessed);

                        Product product = marcFile.Product;
                        string mrkFileText = GetRbdMrkFileText(product);
                        string mrkFilePath = string.Format(@"{0}{1}.mrk", workingDirectory, product.Isbn13);
                        Log.DebugFormat("mrkFilePath: {0}", mrkFilePath);

                        string mrcFilePath = string.Format(@"{0}{1}.mrc", workingDirectory, product.Isbn13);
                        Log.DebugFormat("mrcFilePath: {0}", mrcFilePath);

                        File.WriteAllText(mrkFilePath, mrkFileText);
                        MARC21 marc21 = new MARC21();

                        marc21.MMaker(mrkFilePath, mrcFilePath);

                        mrcStrings.Add(ReadMarcFile(mrcFilePath));
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    processAttempts++;
                }
            }
            Log.InfoFormat("Created {0} MARC files", mrcStrings.Count);
            return mrcStrings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDirectory"></param>
        private void ClearWorkingDirectory(string workingDirectory)
        {
            string[] filesToDelete = Directory.GetFiles(workingDirectory);
            foreach (string file in filesToDelete.Where(file => file.EndsWith(".mrk") || file.EndsWith(".mrc") || file.EndsWith(".xml")))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Log.InfoFormat("Error Deleteing file: {0} || Exception: {1}", file, ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private string ReadMarcFile(string filepath)
        {

            //while (IsFileLocked(new FileInfo(filepath)))
            //{

            //}
            StreamReader streamReaderToParse = File.OpenText(filepath);
            string parsedText = streamReaderToParse.ReadToEnd();
            streamReaderToParse.Close();
            return parsedText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private string GetRbdMrkFileText(Product product)
        {
            try
            {
                string publicationYearText = product.PublicationYearText;

                if (publicationYearText == "")
                {
                    Log.DebugFormat("Id: {0}, Sku: {1}, Isbn13: {2}", product.Id, product.Sku, product.Isbn13);
                }

                StringBuilder mrkFileText = new StringBuilder();
                string sitepath = Settings.Default.SiteSubDirectory;

                mrkFileText.AppendFormat("=LDR  {0}nam  22{1}2a 4500", GetNext5DigitRandomNumber(),
                                         GetNext5DigitRandomNumber()).AppendLine();

                Log.DebugFormat("PublicationYearText: {0}, PublicationYear: {1}, sku: {2}", product.PublicationYearText,
                                product.PublicationYear, product.Sku);

                string line008 =
                    string.Format("=008  {0:yyMMdd}s{1:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ",
                                  _currentDateTime, product.PublicationYear);

                Log.DebugFormat("line008: {0}", line008);
                Log.DebugFormat("line008.Length: {0}", line008.Length);

                mrkFileText.AppendLine(line008);

                //The MRC file requires an additional \\. For some reason it loses when when converted
                //string extraSlashes = (mrkOnly) ? "" : "\\";
                const string extraSlashes = "";
                mrkFileText.AppendFormat("=020  \\\\{1}$a{0}", product.Isbn10, extraSlashes).AppendLine()
                    .AppendFormat("=020  \\\\{1}$a{0}", product.Isbn13, extraSlashes).AppendLine();

                mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc")
                    .AppendFormat("=100  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
                    .AppendFormat("=245  10$a{0}", StripOffCarriageReturnAndLineFeed(product.Title)).AppendLine();

                mrkFileText.AppendFormat("=260  \\\\$b{0},$c{1}", product.PublisherName, publicationYearText).AppendLine
                    ();
                mrkFileText.AppendFormat(
                    "=533  \\\\$a{0}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{1}", product.Format,
                    publicationYearText)
                    .AppendLine();

                mrkFileText.AppendFormat("=650  \\0$a{0}.", product.CategoryName).AppendLine()
                    .AppendFormat("=700  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
                    .AppendFormat("=856  4\\$zConnect to this resource online$u{0}Products/Book.aspx?sku={1}", sitepath,
                                  product.Isbn10).AppendLine()
                    .AppendLine();

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
                    Log.InfoFormat("PublicationDate: {0}", product.PublicationDate);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string StripOffCarriageReturnAndLineFeed(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetNext5DigitRandomNumber()
        {
            int next = _random.Next(10000, 99999);
            return next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:             
                //still being written to             
                //or being processed by another thread             
                //or does not exist (has already been processed)             
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked         
            return false;
        }
    }
}