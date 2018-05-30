using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MARCEngine5;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class RittenhouseOnlyMarcRecordsTask : TaskBase2
    {
        
        private readonly StringBuilder _results = new StringBuilder();
        private readonly MarcRecordService _marcRecordService;
        private readonly MarcRecordFactory _marcRecordProductFactory;
        private int BatchSize { get; }

        private int _recordsProcessed;

        private int _recordCountBeingProcessed;

        private readonly bool _onlyMissingFiles;

        public RittenhouseOnlyMarcRecordsTask(bool onlyMissingFiles)
            : base("Generate Missing Rittenhouse Only MArC Records", "CreateMarcRecords")
        {
            _onlyMissingFiles = onlyMissingFiles;
            BatchSize = Settings.Default.RbdMarcRecordMaxProducts;
            _marcRecordService = new MarcRecordService(MarcRecordProviderType.Rbd);
            _marcRecordProductFactory = new MarcRecordFactory();
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
                Log.InfoFormat("MarcRecordGeneratorMaxProducts: {0}", BatchSize);
                Log.InfoFormat("MarcRecordBatchSize: {0}", Settings.Default.MarcRecordBatchSize);

                //Set Marc Records to get
                _recordCountBeingProcessed = _marcRecordProductFactory.GetRittenhouseOnlyMarcFilesCount(_onlyMissingFiles);
                Log.InfoFormat("productsWithoutMarcRecordsCount: {0}", _recordCountBeingProcessed);


                int newProductsProcessed = 0;
                int batchProductProcessed;
                while ((batchProductProcessed = ProcessNextBatchOfNewProducts(Settings.Default.MarcRecordBatchSize)) > 0)
                {
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    newProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("newProductsProcessed: {0}", newProductsProcessed);
                    //Want to process them ALL!!!
                    if (newProductsProcessed >= _recordCountBeingProcessed)
                    {
                        break;
                    }
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
        private int ProcessNextBatchOfNewProducts(int batchSize)
        {

            int productProcessedCount = 0;
            int processedAttempts = 0;
            //Loop three times if a failure
            while (processedAttempts < 3)
            {
                try
                {
                    // get next batch to process
                    List<IMarcFile> marcFiles = _marcRecordProductFactory.GetRittenhouseOnlyMarcFiles(batchSize, _onlyMissingFiles);

                    Log.InfoFormat("batch contains {0} products", marcFiles.Count);
                    if (marcFiles.Count == 0)
                    {
                        return 0;
                    }
                    var timer = new Stopwatch();
                    timer.Start();
                    _marcRecordService.SetMarcRecords(marcFiles);
                    Log.Info($"_marcRecordService.SetMarcRecords. It took {timer.ElapsedMilliseconds}ms");

                    timer = new Stopwatch();
                    timer.Start();
                    _marcRecordProductFactory.InsertUpdateMarcRecordFiles(marcFiles, MarcRecordProviderType.Rbd);
                    Log.Info($"_marcRecordProductFactory.InsertUpdateMarcRecordFile. It took {timer.ElapsedMilliseconds}ms");

                    productProcessedCount = marcFiles.Count;


                //    foreach (int rowsSaved in marcFiles.Select(MarcRecordsProductFactory.InsertUpdateMarcRecordFile))
                //    {
                //        Log.DebugFormat("rowsSaved: {0}", rowsSaved);
                //        productProcessedCount++;
                //    }
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
                    //MarcFieldParsingFactory marcFieldParsingFactory = new MarcFieldParsingFactory();
                    //var skuList = marcFiles.Select(x => x.Product.Sku).ToList();
                    //List<ParsedMarcField> allParsedMarcFields = marcFieldParsingFactory.GetParsedMarcFields(skuList);

                    foreach (IMarcFile marcFile in marcFiles)
                    {
                        //List<ParsedMarcField> parsedMarcFields = allParsedMarcFields?.Where(x => x.Sku == marcFile.Product.Sku).ToList();
                        //if (parsedMarcFields != null && parsedMarcFields.Any())
                        //{
                        //    var test = 1;
                        //}
                        _recordsProcessed++;
                        Log.InfoFormat("Processing {0} of {1}", _recordsProcessed, _recordCountBeingProcessed);

                        Product product = marcFile.Product;
                        //string mrkFileText = GetRbdMrkFileText(product, parsedMarcFields);
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
                            int space = nodeInnterText.IndexOf(" ", StringComparison.Ordinal);
                            int seperator = nodeInnterText.IndexOf("(", StringComparison.Ordinal);
                            if (space > 0 || seperator > 0)
                            {
                                isbns.Add(space > 0
                                          ? nodeInnterText.Substring(0, space)
                                          : nodeInnterText.Substring(0, nodeInnterText.IndexOf("(", StringComparison.Ordinal)));
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
                            int space = nodeInnterText.IndexOf(" ", StringComparison.Ordinal);
                            isbns.Add(space > 0
                                          ? nodeInnterText.Substring(0, space)
                                          : nodeInnterText.Substring(0, node.InnerText.IndexOf("(", StringComparison.Ordinal)));
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

    }
}