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
        
        private readonly StringBuilder _results = new StringBuilder();

        private int BatchSize { get; set; }

        private int _recordsProcessed;

        private int _recordCountBeingProcessed;

        private readonly bool _onlyMissingFiles;

        public RittenhouseOnlyMarcRecordsTask(bool onlyMissingFiles)
            : base("Generate Missing Rittenhouse Only MArC Records", "CreateMarcRecords")
        {
            _onlyMissingFiles = onlyMissingFiles;
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
                _recordCountBeingProcessed = _onlyMissingFiles
                                                 ? MarcRecordsProductFactory.GetProductsWithoutMarcRecordsCount()
                                                 : MarcRecordsProductFactory.GetAllRittenhouseMarcRecordProductsCount();

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
                    List<IMarcFile> marcFiles = _onlyMissingFiles
                                                    ? MarcRecordsProductFactory.GetProductsWithoutMarcRecords(batchSize)
                                                    : MarcRecordsProductFactory.GetAllRittenhouseMarcRecordProducts(batchSize);

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
            foreach (var file in filesToDelete)
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

            //foreach (string file in filesToDelete.Where(file => file.EndsWith(".mrk") || file.EndsWith(".mrc") || file.EndsWith(".xml")))
            //{
            //    try
            //    {
            //        File.Delete(file);
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.InfoFormat("Error Deleteing file: {0} || Exception: {1}", file, ex.Message);
            //    }
            //}
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

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