using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
//using MARCEngine5;
using MARCEngine5;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;
using Zoom.Net;
using Zoom.Net.YazSharp;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class MarcRecordsTask : TaskBase2
    {
        //private readonly int _batchSize = Properties.Settings.Default.MarcRecordBatchSize;
        //private readonly int _batchSize = 100;

        private readonly Random _random = new Random();
        private readonly DateTime _currentDateTime = DateTime.Now;
        private readonly StringBuilder _results = new StringBuilder();

        private MarcRecordProvider ProviderType { get; set; }
        private int BatchSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MarcRecordsTask(MarcRecordProvider recordProvider)
            : base("Generate MArC Records", "CreateMarcRecords")
        {
            ProviderType = recordProvider;
            switch (recordProvider)
            {
                case MarcRecordProvider.Lc:
                    BatchSize = Settings.Default.LcMarcRecordMaxProducts;
                    break;
                case MarcRecordProvider.Nlm:
                    BatchSize = Settings.Default.NlmMarcRecordMaxProducts;
                    break;
                case MarcRecordProvider.Rbd:
                    BatchSize = Settings.Default.RbdMarcRecordMaxProducts;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                int productsWithoutMarcRecordsCount = MarcRecordsProductFactory.GetProductsWithoutMarcRecordsCount(ProviderType);
                Log.InfoFormat("productsWithoutMarcRecordsCount: {0}", productsWithoutMarcRecordsCount);


                ClearWorkingDirectory(workingDirectory);


                int newProductsProcessed = 0;
                int batchProductProcessed;
                while ((batchProductProcessed = ProcessNextBatchOfNewProducts(Settings.Default.MarcRecordBatchSize, workingDirectory)) > 0)
                {
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    newProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("newProductsProcessed: {0}", newProductsProcessed);
                    if (newProductsProcessed >= BatchSize)
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
                    List<IMarcFile> marcFiles = MarcRecordsProductFactory.GetProductsWithoutMarcRecords(batchSize, ProviderType);

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
			List<string> mrcStrings = new List<string>(GetMarcRecords(marcFiles, ProviderType, workingDirectory));

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
						foreach (IMarcFile marcFile in marcFiles.Where(marcFile => marcFile.Product.Isbn10 == isbn || marcFile.Product.Isbn13 == isbn))
						{
							marcFile.MrcFileText = mrcString;
							marcFile.XmlFileText = xmlFileText;
							marcFile.MrkFileText = mrkFileText;
							break;
						}
					}

                    File.Delete(mrkFilePath);
                    File.Delete(mrcFilePath);
                    File.Delete(xmlFilePath);

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
        /// <param name="marcRecordType"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private IEnumerable<string> GetMarcRecords(List<IMarcFile> marcFiles, MarcRecordProvider marcRecordType, string workingDirectory)
        {
            List<string> queries = new List<string>(GetMarcQuery(marcFiles));
            List<string> mrcStrings = new List<string>();
            int processAttempts = 0;
            //Loop five times if a failure
            while (processAttempts < 5)
            {
                try
                {
					Log.Info("Pre-Zoom Connection");
                    if (marcRecordType != MarcRecordProvider.Rbd)
                    {
                        MarcRecordProviderValue serverValue = new MarcRecordProviderValue(marcRecordType);

                        Connection lcConnection = new Connection(serverValue.ToString(), 7090)
                        {
                            DatabaseName = "Voyager",
                            Syntax = RecordSyntax.USMARC,
                        };

                        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                        lcConnection.Options.Equals("F");
                        // ReSharper restore ReturnValueOfPureMethodIsNotUsed
                        processAttempts++;
                        lcConnection.Connect();

						Log.Info("Zoom-Connected");
                        
                        //TODO: Should determine here if title matches what we have as a title. 

						foreach (string query in queries)
                        {
                            PrefixQuery prefixQuery = new PrefixQuery(query);
                            ResultSet resultSet = (ResultSet)lcConnection.Search(prefixQuery);
                            
                            for (uint i = 0; i < ((IResultSet)resultSet).Size; i++)
                            {
                                string temp = Encoding.UTF8.GetString(((IResultSet)resultSet)[i].Content);
                                if (temp.Length <= 0) continue;                                
                                mrcStrings.Add(temp);
                                break;
                            }
                            ((IResultSet)resultSet).Dispose();
                        }
                        lcConnection.Dispose();
                    }
                    else
                    {
                        foreach (IMarcFile marcFile in marcFiles)
                        {
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

                            File.Delete(mrkFilePath);
                            File.Delete(mrcFilePath);
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    processAttempts++;
                }
            }
			Log.Info("External Search Complete");
            return mrcStrings;
        }

		/// <summary>
        /// 
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetMarcQuery(List<IMarcFile> marcFiles)
        {
            List<string> queries = new List<string>();
            for (int ii = 0; ii < marcFiles.Count; )
            {
                StringBuilder sbQuery = new StringBuilder();
                int marcRecordsLeft = (marcFiles.Count - ii);
                if (marcRecordsLeft < 10)
                {
                    for (int i = ii; i < ii + marcRecordsLeft; i++)
                    {
                        queries.Add(string.Format(" @attr 1=7 {0} ", marcFiles[i].Product.Isbn13 ?? marcFiles[i].Product.Isbn10));
                    }
                }
                else
                {
                    for (int i = 0; i < 9; i++)
                    {
                        sbQuery.Append("@or ");
                    }
                    int count = ii + 10;
                    for (int i = ii; i < count; i++)
                    {
                        sbQuery.AppendFormat(" @attr 1=7 {0} ", marcFiles[i].Product.Isbn13 ?? marcFiles[i].Product.Isbn10);
                    }
                    queries.Add(sbQuery.ToString());
                }
                ii += 10;
            }
            return queries;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDirectory"></param>


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
        //private string GetRbdMrkFileText(Product product)
        //{
        //    try
        //    {
        //        string publicationYearText = product.PublicationYearText;

        //        if (publicationYearText == "")
        //        {
        //            Log.DebugFormat("Id: {0}, Sku: {1}, Isbn13: {2}", product.Id, product.Sku, product.Isbn13);
        //        }

        //        StringBuilder mrkFileText = new StringBuilder();
        //        string sitepath = Settings.Default.SiteSubDirectory;

        //        mrkFileText.AppendFormat("=LDR  {0}nam  22{1}2a 4500", GetNext5DigitRandomNumber(),
        //                                 GetNext5DigitRandomNumber()).AppendLine();

        //        Log.DebugFormat("PublicationYearText: {0}, PublicationYear: {1}, sku: {2}", product.PublicationYearText,
        //                        product.PublicationYear, product.Sku);

        //        string line008 =
        //            string.Format("=008  {0:yyMMdd}s{1:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ",
        //                          _currentDateTime, product.PublicationYear);

        //        Log.DebugFormat("line008: {0}", line008);
        //        Log.DebugFormat("line008.Length: {0}", line008.Length);

        //        mrkFileText.AppendLine(line008);

        //        //The MRC file requires an additional \\. For some reason it loses when when converted
        //        //string extraSlashes = (mrkOnly) ? "" : "\\";
        //        const string extraSlashes = "";
        //        mrkFileText.AppendFormat("=020  \\\\{1}$a{0}", product.Isbn10, extraSlashes).AppendLine()
        //            .AppendFormat("=020  \\\\{1}$a{0}", product.Isbn13, extraSlashes).AppendLine();

        //        mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc")
        //            .AppendFormat("=100  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
        //            .AppendFormat("=245  10$a{0}", StripOffCarriageReturnAndLineFeed(product.Title)).AppendLine();
        //        if (string.IsNullOrEmpty(product.PublisherName))
        //        {
        //            var test = 1;
        //        }
        //        mrkFileText.AppendFormat("=260  \\\\$b{0},$c{1}", product.PublisherName, publicationYearText).AppendLine();
        //        mrkFileText.AppendFormat("=533  \\\\$a{0}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{1}", product.Format,
        //            publicationYearText)
        //            .AppendLine();
        //        if (string.IsNullOrEmpty(product.CategoryName))
        //        {
        //            var test = 1;
        //        }
        //        mrkFileText.AppendFormat("=650  \\0$a{0}.", product.CategoryName).AppendLine()
        //            .AppendFormat("=700  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
        //            .AppendFormat("=856  4\\$zConnect to this resource online$u{0}Products/Book.aspx?sku={1}", sitepath,
        //                          product.Isbn10).AppendLine()
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
        //            Log.InfoFormat("PublicationDate: {0}", product.PublicationDate);
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

//Query query = new Query();
//query.Database = serverValue.ToString();
//query.Host = "Voyager";
//query.ElementSetName = "F";
//query.Port = 7090;
//query.Syntax = "USMARC";
//query.BatchZ3950Search("C:\\Clients\\Rittenhouse\\RBD\\RBD\\RittenhouseWebLoader\\RittenhouseWebLoader\\MarcRecordsWorking\\batch_20111109_084734530.txt", mrcFilePath, 7, notFoundTextPath);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="marcXmlforProducts"></param>
//        /// <returns></returns>
//        private Dictionary<string, string> MergeMarcRecords(Dictionary<MarcRecordProvider, Dictionary<string, string>> marcXmlforProducts)
//        {
//            Dictionary<string, string> mergedRecords = new Dictionary<string, string>();

//            Dictionary<string, string> rbdDictionary = null;
//            Dictionary<string, string> nlmDictionary = null;
//            Dictionary<string, string> lcDictionary = null;

//            foreach (KeyValuePair<MarcRecordProvider, Dictionary<string, string>> marcXmlforProduct in marcXmlforProducts)
//            {
//                switch (marcXmlforProduct.Key)
//                {
//                        case MarcRecordProvider.Rbd:
//                        rbdDictionary = marcXmlforProduct.Value;
//                        break;
//                        case MarcRecordProvider.Nlm:
//                        nlmDictionary = marcXmlforProduct.Value;
//                        break;
//                        case MarcRecordProvider.Lc:
//                        lcDictionary = marcXmlforProduct.Value;
//                        break;
//                }                
//            }

//            if (rbdDictionary != null)
//                try
//                {
//                    foreach (KeyValuePair<string, string> keyValuePair in rbdDictionary)
//                    {
//                        string recordMerged = keyValuePair.Value;
//                        string recordToMerge;
//                        if (nlmDictionary != null && nlmDictionary.ContainsKey(keyValuePair.Key))
//                        {
//                            nlmDictionary.TryGetValue(keyValuePair.Key, out recordToMerge);
//                            recordMerged = MergeXmlRecord(keyValuePair.Value, recordToMerge);
//                        }
//                        if (lcDictionary != null && lcDictionary.ContainsKey(keyValuePair.Key))
//                        {
//                            lcDictionary.TryGetValue(keyValuePair.Key, out recordToMerge);
//                            recordMerged = MergeXmlRecord(keyValuePair.Value, recordToMerge);
//                        }
//                        mergedRecords.Add(keyValuePair.Key, recordMerged);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    string test = "test";
//                    throw;
//                }

//            return mergedRecords;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="rbdRecord"></param>
//        /// <param name="otherRecord"></param>
//        /// <returns></returns>
//        private string MergeXmlRecord(string rbdRecord, string otherRecord)
//        {
//            List<XmlTagAndNode> rbdControlTagList = ExtractAttributes(rbdRecord, "controlfield");
//            List<XmlTagAndNode> otherControlTagList = ExtractAttributes(otherRecord, "controlfield");
//            List<XmlTagAndNode> rbdDataTagList = ExtractAttributes(rbdRecord, "datafield");
//            List<XmlTagAndNode> otherDataTagList = ExtractAttributes(otherRecord, "datafield");

//            rbdControlTagList = MergeXmlandNodes(rbdControlTagList, otherControlTagList);
//            rbdDataTagList = MergeXmlandNodes(rbdDataTagList, otherDataTagList);

//            rbdControlTagList.Sort(new XmlTagAndNodeAsc());
//            rbdDataTagList.Sort(new XmlTagAndNodeAsc());

//            XmlDocument rbdDoc = new XmlDocument();
//            rbdDoc.LoadXml(rbdRecord);

//            for (int i = 0; i < rbdDoc.GetElementsByTagName("controlfield").Count; i++)
//            {
//                rbdDoc.GetElementsByTagName("controlfield")[i].ParentNode.RemoveChild(rbdDoc.GetElementsByTagName("controlfield")[i]);
//                i--;
//            }

//            foreach (XmlTagAndNode xmlTagAndNode in rbdControlTagList)
//            {
//                rbdDoc.DocumentElement.AppendChild(rbdDoc.ImportNode(xmlTagAndNode.Node, true));
//            }

//            for (int i = 0; i < rbdDoc.GetElementsByTagName("datafield").Count; i++)
//            {
//                rbdDoc.GetElementsByTagName("datafield")[i].ParentNode.RemoveChild(rbdDoc.GetElementsByTagName("datafield")[i]);
//                i--;
//            }

//            foreach (XmlTagAndNode xmlTagAndNode in rbdDataTagList)
//            {
//                rbdDoc.DocumentElement.AppendChild(rbdDoc.ImportNode(xmlTagAndNode.Node, true));
//            }

//            return rbdDoc.InnerXml;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="rbdTagList"></param>
//        /// <param name="otherTagList"></param>
//        /// <returns></returns>
//        private List<XmlTagAndNode> MergeXmlandNodes(List<XmlTagAndNode> rbdTagList, IEnumerable<XmlTagAndNode> otherTagList)
//        {
//            foreach (XmlTagAndNode xmlTagAndNode in otherTagList)
//            {
//                bool rbdHasTag = false;
//                foreach (XmlTagAndNode tagAndNode in rbdTagList)
//                {
//                    if (xmlTagAndNode.Tag == tagAndNode.Tag)
//                    {
//                        rbdHasTag = true;
//                    }
//                }
//                if (!rbdHasTag)
//                {
//                    rbdTagList.Add(xmlTagAndNode);
//                }
//            }
//            return rbdTagList;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="xmlText"></param>
//        /// <param name="tagName"></param>
//        /// <returns></returns>
//        private List<XmlTagAndNode> ExtractAttributes(string xmlText, string tagName)
//        {
//            List<XmlTagAndNode> tagList = new List<XmlTagAndNode>();
//            XmlDocument doc = new XmlDocument();
//            doc.LoadXml(xmlText);

//            foreach (XmlNode node in doc.GetElementsByTagName(tagName))
//            {
//                if (node.Attributes != null)
//                {
//                    XmlTagAndNode newTag = new XmlTagAndNode
//                                            {Tag = Convert.ToInt32(node.Attributes["tag"].InnerText), Node = node};
//                    tagList.Add(newTag);
//                }
//            }
//            return tagList;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="xmlText"></param>
//        /// <returns></returns>
//        private string ExtractIsbn(string xmlText)
//        {
//            try
//            {
//                XmlDocument doc = new XmlDocument();
//                doc.LoadXml(xmlText);

//                XmlNodeList xmlNodeList = doc.GetElementsByTagName("datafield");

//                List<string> isbns = new List<string>();

//                foreach (XmlNode node in xmlNodeList.Cast<XmlNode>().Where(node => node.Attributes != null && Convert.ToInt32(node.Attributes["tag"].InnerText) == 20))
//                {
//                    if (node.HasChildNodes)
//                    {
//                        foreach (XmlNode xmlNode in node.ChildNodes)
//                        {
//                            int space = xmlNode.InnerText.IndexOf(" ");
//                            int seperator = xmlNode.InnerText.IndexOf("(");
//                            if (space > 0 || seperator > 0)
//                            {
//                                isbns.Add(space > 0
//                                          ? xmlNode.InnerText.Substring(0, space)
//                                          : xmlNode.InnerText.Substring(0, xmlNode.InnerText.IndexOf("(")));
//                            }
//                            else
//                            {
//                                isbns.Add(xmlNode.InnerText);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        if (node.InnerText.Length > 13)
//                        {
//                            int space = node.InnerText.IndexOf(" ");
//                            isbns.Add(space > 0
//                                          ? node.InnerText.Substring(0, space)
//                                          : node.InnerText.Substring(0, node.InnerText.IndexOf("(")));
//                        }
//                        else
//                        {
//                            isbns.Add(node.InnerText);
//                        }
//                    }
//                }

//                foreach (string isbn in isbns.Where(isbn => isbn.Length == 10))
//                {
//                    return isbn;
//                }
//                return isbns[0];
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex.Message, ex);
//                throw;
//            }
//        }
//    }
//}
//public class XmlTagAndNode
//{
//    public int Tag { get; set; }
//    public XmlNode Node { get; set; }


//}
//public class XmlTagAndNodeAsc : IComparer<XmlTagAndNode>
//{
//    public int Compare(XmlTagAndNode x, XmlTagAndNode y)
//    {
//        if (x.Tag < y.Tag)
//        {
//            return -1;
//        }
//        return x.Tag > y.Tag ? 1 : 0;

//    }
//}