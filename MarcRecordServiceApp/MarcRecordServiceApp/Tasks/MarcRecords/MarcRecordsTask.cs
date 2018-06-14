using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class MarcRecordsTask : TaskBase2
    {
        //private readonly int _batchSize = Properties.Settings.Default.MarcRecordBatchSize;
        //private readonly int _batchSize = 100;

        private readonly StringBuilder _results = new StringBuilder();

        private MarcRecordProviderType ProviderType{ get; }

        private readonly MarcRecordService _marcRecordService;
        private int BatchSize { get; }

        private int _recordsFound;
        private int _recordsToFind;


        private readonly MarcRecordFactory _marcRecordProductFactory;

        /// <summary>
        /// 
        /// </summary>
        public MarcRecordsTask(MarcRecordProviderType recordProviderType)
            : base($"Generate MArC Records - {recordProviderType.ToString()}", "CreateMarcRecords")
        {
            ProviderType = recordProviderType;
            switch (recordProviderType)
            {
                case MarcRecordProviderType.Lc:
                    BatchSize = Settings.Default.LcMarcRecordMaxProducts;
                    break;
                case MarcRecordProviderType.Nlm:
                    BatchSize = Settings.Default.NlmMarcRecordMaxProducts;
                    break;
                case MarcRecordProviderType.Rbd:
                    BatchSize = Settings.Default.RbdMarcRecordMaxProducts;
                    break;
            }

            _marcRecordProductFactory = new MarcRecordFactory();
            _marcRecordService = new MarcRecordService(recordProviderType);
            _recordsFound = 0;
            _recordsToFind = 0;
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
                step.Results.Append($"Marc Record Provider: {ProviderType.ToString()}");
                step.Results.Append($"Records To Find {_recordsToFind} || Records Found: {_recordsFound} ");
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
                int productsWithoutMarcRecordsCount = _marcRecordProductFactory.GetProductsWithoutMarcRecordsCount(ProviderType);
                Log.InfoFormat("productsWithoutMarcRecordsCount: {0}", productsWithoutMarcRecordsCount);

                int newProductsProcessed = 0;
                int batchProductProcessed;


                while ((batchProductProcessed = ProcessNextBatchOfNewProducts(Settings.Default.MarcRecordBatchSize)) > 0)
                {
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    newProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("newProductsProcessed: {0}", newProductsProcessed);
                    if (newProductsProcessed >= BatchSize)
                    {
                        break;
                    }
                }

                _recordsToFind += newProductsProcessed;


                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                _results.Insert(0, string.Format("EXCEPTION: {0}\n\r\t", ex.Message));
                return false;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private int ProcessNextBatchOfNewProducts(int batchSize, string workingDirectory)
        //{
        //    var batchTimer = new Stopwatch();
        //    batchTimer.Start();
            
        //    int productProcessedCount = 0;
        //    int processedAttempts = 0;
        //    //Loop three times if a failure
        //    while (processedAttempts < 3)
        //    {
        //        try
        //        {
        //            // get next batch to process
        //            Stopwatch timer = new Stopwatch();
        //            timer.Start();
        //            List<IMarcFile> marcFiles = MarcRecordsProductFactory.GetProductsWithoutMarcRecords(batchSize, ProviderType);
        //            Log.Info("");
        //            Log.Info($"GetProductsWithoutMarcRecords count[{marcFiles.Count}] products. It took {timer.ElapsedMilliseconds}ms");
        //            if (marcFiles.Count == 0)
        //            {
        //                return 0;
        //            }
        //            timer = new Stopwatch();
        //            timer.Start();
        //            marcFiles = SetMarcDataForProductsOld(marcFiles, workingDirectory);
        //            Log.Info($"SetMarcDataForProductsOld. It took {timer.ElapsedMilliseconds}ms");

        //            timer = new Stopwatch();
        //            timer.Start();
        //            foreach (int rowsSaved in marcFiles.Select(MarcRecordsProductFactory.InsertUpdateMarcRecordFile))
        //            {
        //                Log.DebugFormat("rowsSaved: {0}", rowsSaved);
        //                productProcessedCount++;
        //            }
        //            Log.Info($"MarcRecordsProductFactory.InsertUpdateMarcRecordFile loop . It took {timer.ElapsedMilliseconds}ms");
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex.Message, ex);
        //            processedAttempts++;
        //        }
        //    }
        //    Log.Info($"ProcessNextBatchOfNewProducts Batch took {batchTimer.ElapsedMilliseconds}ms");
        //    return productProcessedCount;
        //}

        private int ProcessNextBatchOfNewProducts(int batchSize)
        {
            var batchTimer = new Stopwatch();
            batchTimer.Start();

            int productProcessedCount = 0;
            int processedAttempts = 0;
            //Loop three times if a failure
            while (processedAttempts < 3)
            {
                try
                {
                    // get next batch to process
                    var timer = new Stopwatch();
                    timer.Start();
                    List<IMarcFile> marcFiles = _marcRecordProductFactory.GetProductsWithoutMarcRecords(batchSize, ProviderType);

                    Log.Info("");
                    Log.Info($"GetProductsWithoutMarcRecords count[{marcFiles.Count}] products. It took {timer.ElapsedMilliseconds}ms");

                    if (marcFiles.Count == 0)
                    {
                        return 0;
                    }
                    timer = new Stopwatch();
                    timer.Start();
                    var recordsFound = _marcRecordService.SetMarcRecords(marcFiles);
                    Log.Info($"SetMarcRecords. It took {timer.ElapsedMilliseconds}ms");

                    timer = new Stopwatch();
                    timer.Start();
                    _marcRecordProductFactory.InsertUpdateMarcRecordFiles(marcFiles, ProviderType);
                    Log.Info($"_marcRecordProductFactory.InsertUpdateMarcRecordFile. It took {timer.ElapsedMilliseconds}ms");
                    productProcessedCount = marcFiles.Count;
                    _recordsFound += recordsFound;
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    processedAttempts++;
                }
            }
            Log.Info($"ProcessNextBatchOfNewProducts2 Batch took {batchTimer.ElapsedMilliseconds}ms");
            return productProcessedCount;
        }


        //private List<IMarcFile> SetMarcDataForProductsOld(List<IMarcFile> marcFiles, string workingDirectory)
        //{
        //    List<string> mrcStrings = new List<string>(GetMarcRecordsOld(marcFiles, ProviderType, workingDirectory));

        //    try
        //    {
        //        foreach (string mrcString in mrcStrings)
        //        {
        //            string batchFileNameBase = string.Format("batch_{0:yyyyMMdd_HHmmssfff}", DateTime.Now);

        //            string mrkFilePath = string.Format(@"{0}{1}.mrk", workingDirectory, batchFileNameBase);
        //            Log.DebugFormat("mrkFilePath: {0}", mrkFilePath);

        //            string mrcFilePath = string.Format(@"{0}{1}.mrc", workingDirectory, batchFileNameBase);
        //            Log.DebugFormat("mrcFilePath: {0}", mrcFilePath);

        //            string xmlFilePath = string.Format(@"{0}{1}.xml", workingDirectory, batchFileNameBase);
        //            Log.DebugFormat("xmlFilePath: {0}", xmlFilePath);

        //            File.WriteAllText(mrcFilePath, mrcString);

        //            MARC21 marc21 = new MARC21();
        //            marc21.MarcFile(mrcFilePath, mrkFilePath);

        //            if (new FileInfo(mrkFilePath).Length == 0)
        //            {
        //                //Indicates an issue with the record or the process
        //                continue;
        //            }
        //            string mrkFileText = ReadMarcFile(mrkFilePath);
        //            marc21.MARC2MARC21XML(mrcFilePath, xmlFilePath, false);

        //            if (new FileInfo(xmlFilePath).Length == 0)
        //            {
        //                //Indicates an issue with the record or the process
        //                continue;
        //            }
        //            string xmlFileText = ReadMarcFile(xmlFilePath);

        //            IEnumerable<string> isbns = ExtractIsbnsFromXml(xmlFileText);

        //            foreach (string isbn in isbns)
        //            {
        //                foreach (IMarcFile marcFile in marcFiles.Where(marcFile => marcFile.Product.Isbn10 == isbn || marcFile.Product.Isbn13 == isbn))
        //                {
        //                    marcFile.MrcFileText = mrcString;
        //                    marcFile.XmlFileText = xmlFileText;
        //                    marcFile.MrkFileText = mrkFileText;
        //                    break;
        //                }
        //            }

        //            File.Delete(mrkFilePath);
        //            File.Delete(mrcFilePath);
        //            File.Delete(xmlFilePath);

        //        }
        //        return marcFiles;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //}

        //private IEnumerable<string> GetMarcRecordsOld(List<IMarcFile> marcFiles, MarcRecordProviderType marcRecordType, string workingDirectory)
        //{
        //    int foundRecords = 0;
        //    Stopwatch timer = new Stopwatch();
        //    timer.Start();
        //    List<string> queries = new List<string>(GetMarcQuery(marcFiles));
        //    List<string> mrcStrings = new List<string>();
        //    int processAttempts = 0;
        //    //Loop five times if a failure
        //    while (processAttempts < 5)
        //    {
        //        try
        //        {
        //            if (marcRecordType != MarcRecordProviderType.Rbd)
        //            {
        //                MarcRecordProviderValue serverValue = new MarcRecordProviderValue(marcRecordType);

        //                Connection lcConnection = new Connection(serverValue.ToString(), 7090)
        //                {
        //                    DatabaseName = "Voyager",
        //                    Syntax = RecordSyntax.USMARC,
        //                    //Options = { ["1.2.840.10003.5.10"] = "F"}
        //                };

        //                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        //                //lcConnection.Options.Equals("F");
        //                // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        //                processAttempts++;
        //                lcConnection.Connect();

        //                Log.Info("Zoom-Connected");

        //                //TODO: Should determine here if title matches what we have as a title. 

        //                foreach (string query in queries)
        //                {
        //                    PrefixQuery prefixQuery = new PrefixQuery(query);
        //                    ResultSet resultSet = (ResultSet)lcConnection.Search(prefixQuery);

        //                    for (uint i = 0; i < ((IResultSet)resultSet).Size; i++)
        //                    {
        //                        string temp = Encoding.UTF8.GetString(((IResultSet)resultSet)[i].Content);
        //                        if (temp.Length <= 0)
        //                        {
        //                            continue;
        //                        }
        //                        mrcStrings.Add(temp);
        //                        foundRecords++;
        //                        break;
        //                    }
        //                    ((IResultSet)resultSet).Dispose();
        //                }
        //                lcConnection.Dispose();
        //                Log.Info("Zoom-Disconnected");
        //            }
        //            else
        //            {
        //                //MarcFieldParsingFactory marcFieldParsingFactory = new MarcFieldParsingFactory();
        //                //var skuList = marcFiles.Select(x => x.Product.Sku).ToList();
        //                //List<ParsedMarcField> allParsedMarcFields = marcFieldParsingFactory.GetParsedMarcFields(skuList);

        //                foreach (IMarcFile marcFile in marcFiles)
        //                {
        //                    //List<ParsedMarcField> parsedMarcFields = allParsedMarcFields?.Where(x => x.Sku == marcFile.Product.Sku).ToList();

        //                    Product product = marcFile.Product;
        //                    //string mrkFileText = GetRbdMrkFileText(product, parsedMarcFields);
        //                    string mrkFileText = GetRbdMrkFileText(product);
        //                    string mrkFilePath = string.Format(@"{0}{1}.mrk", workingDirectory, product.Isbn13);
        //                    Log.DebugFormat("mrkFilePath: {0}", mrkFilePath);

        //                    string mrcFilePath = string.Format(@"{0}{1}.mrc", workingDirectory, product.Isbn13);
        //                    Log.DebugFormat("mrcFilePath: {0}", mrcFilePath);

        //                    File.WriteAllText(mrkFilePath, mrkFileText);
        //                    MARC21 marc21 = new MARC21();

        //                    marc21.MMaker(mrkFilePath, mrcFilePath);

        //                    mrcStrings.Add(ReadMarcFile(mrcFilePath));
        //                    foundRecords++;
        //                    File.Delete(mrkFilePath);
        //                    File.Delete(mrcFilePath);
        //                }
        //            }
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex.Message, ex);
        //            processAttempts++;
        //        }
        //    }
        //    Log.Info($"GetMarcRecordsOld --- It took {timer.ElapsedMilliseconds}ms to set {foundRecords} out of {marcFiles.Count} Marc Records");
        //    return mrcStrings;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="marcFiles"></param>
        ///// <param name="marcRecordType"></param>
        //private void SetMarcRecords(List<IMarcFile> marcFiles, MarcRecordProviderType marcRecordType)
        //{
        //    int foundRecords = 0;
        //    int processAttempts = 0;
        //    //Loop five times if a failure
        //    while (processAttempts < 5)
        //    {
        //        try
        //        {
        //            Stopwatch timer = new Stopwatch();
        //            timer.Start();
        //            MARC21 marc21 = new MARC21();

        //            if (marcRecordType != MarcRecordProviderType.Rbd)
        //            {
        //                List<string> queries = new List<string>(GetMarcQuery(marcFiles));

        //                MarcRecordProviderValue serverValue = new MarcRecordProviderValue(marcRecordType);

        //                using (Connection lcConnection =
        //                    new Connection(serverValue.ToString(), 7090)
        //                    {
        //                        DatabaseName = "Voyager",
        //                        Syntax = RecordSyntax.USMARC
        //                    })
        //                {
        //                    processAttempts++;
        //                    lcConnection.Connect();

        //                    Log.Info("Zoom-Connected");
        //                    foreach (string query in queries)
        //                    {
        //                        PrefixQuery prefixQuery = new PrefixQuery(query);
        //                        using (var resultSet = lcConnection.Search(prefixQuery))
        //                        {
        //                            foreach (IRecord record in resultSet)
        //                            {
        //                                string mrcFileText = Encoding.UTF8.GetString(record.Content);
        //                                if (mrcFileText.Length <= 0)
        //                                {
        //                                    continue;
        //                                }

        //                                string mrkFileText = marc21.MARC2Stream(mrcFileText);
        //                                string xmlFileText = marc21.MARC2MARC21XML_Stream(mrcFileText, false);

        //                                var isbns = ExtractIsbnsFromXml(xmlFileText);

        //                                foreach (string isbn in isbns)
        //                                {
        //                                    var marcFile = marcFiles.FirstOrDefault(x => x.Product.Isbn10 == isbn || x.Product.Isbn13 == isbn);
        //                                    if (marcFile != null)
        //                                    {
        //                                        marcFile.MrcFileText = mrcFileText;
        //                                        marcFile.XmlFileText = xmlFileText;
        //                                        marcFile.MrkFileText = mrkFileText;
        //                                        foundRecords++;
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    Log.Info("Zoom-Disconnected");
        //                }
        //            }
        //            else
        //            {
        //                //MarcFieldParsingFactory marcFieldParsingFactory = new MarcFieldParsingFactory();
        //                //var skuList = marcFiles.Select(x => x.Product.Sku).ToList();
        //                //List<ParsedMarcField> allParsedMarcFields = marcFieldParsingFactory.GetParsedMarcFields(skuList);
        //                //MARC21 marc21 = new MARC21();

        //                foreach (IMarcFile marcFile in marcFiles)
        //                {
        //                    //List<ParsedMarcField> parsedMarcFields = allParsedMarcFields?.Where(x => x.Sku == marcFile.Product.Sku).ToList();

        //                    Product product = marcFile.Product;
        //                    string mrkFileText = GetRbdMrkFileText(product);
        //                    string mrcFileText = marc21.Mnemonic2Stream(mrkFileText);
        //                    string xmlFileText = marc21.MARC2MARC21XML_Stream(mrcFileText, false);

        //                    marcFile.MrcFileText = mrcFileText;
        //                    marcFile.XmlFileText = xmlFileText;
        //                    marcFile.MrkFileText = mrkFileText;
        //                    foundRecords++;
        //                }
        //            }
        //            Log.Info($"GetMarcRecords --- It took {timer.ElapsedMilliseconds}ms to set {foundRecords} out of {marcFiles.Count} Marc Records");
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex.Message, ex);
        //            processAttempts++;
        //        }
        //    }
        //}


        //private static List<string> GetMarcQuery(List<IMarcFile> marcFiles)
        //{
        //    List<string> queries = new List<string>();

        //    int batchCount = 10;
        //    int skipCounter = 0;
        //    var marcRecords = marcFiles.Skip(skipCounter * batchCount).Take(batchCount).ToList();
        //    while (marcRecords.Any())
        //    {
        //        var queryBuilder = new StringBuilder();
        //        var orBuilder = new StringBuilder();
                
        //        foreach (var marcRecord in marcRecords)
        //        {
        //            if (queryBuilder.Length > 0)
        //            {
        //                orBuilder.Append("@or ");
        //            }
        //            queryBuilder.Append($" @attr 1=7 {marcRecord.Product.Isbn13 ?? marcRecord.Product.Isbn10} ");
        //        }
        //        queries.Add($"{orBuilder}{queryBuilder}");

        //        skipCounter++;
        //        marcRecords = marcFiles.Skip(skipCounter * batchCount).Take(batchCount).ToList();
        //    }

        //    return queries;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filepath"></param>
        ///// <returns></returns>
        //private string ReadMarcFile(string filepath)
        //{

        //    //while (IsFileLocked(new FileInfo(filepath)))
        //    //{

        //    //}
        //    StreamReader streamReaderToParse = File.OpenText(filepath);
        //    string parsedText = streamReaderToParse.ReadToEnd();
        //    streamReaderToParse.Close();
        //    return parsedText;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="xmlString"></param>
        ///// <returns></returns>
        //private IEnumerable<string> ExtractIsbnsFromXml(string xmlString)
        //
        //    try
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(xmlString);

        //        XmlNodeList xmlNodeList = doc.GetElementsByTagName("datafield");

        //        List<string> isbns = new List<string>();

        //        foreach (XmlNode node in xmlNodeList.Cast<XmlNode>().Where(node => node.Attributes != null && Convert.ToInt32(node.Attributes["tag"].InnerText) == 20))
        //        {
        //            if (node.HasChildNodes)
        //            {
        //                foreach (XmlNode xmlNode in node.ChildNodes)
        //                {
        //                    string nodeInnterText = xmlNode.InnerText.Replace(".", "");
        //                    int space = nodeInnterText.IndexOf(" ");
        //                    int seperator = nodeInnterText.IndexOf("(");
        //                    if (space > 0 || seperator > 0)
        //                    {
        //                        isbns.Add(space > 0
        //                                  ? nodeInnterText.Substring(0, space)
        //                                  : nodeInnterText.Substring(0, nodeInnterText.IndexOf("(")));
        //                    }
        //                    else
        //                    {
        //                        isbns.Add(nodeInnterText);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                string nodeInnterText = node.InnerText.Replace(".", "");
        //                if (nodeInnterText.Length > 13)
        //                {
        //                    int space = nodeInnterText.IndexOf(" ");
        //                    isbns.Add(space > 0
        //                                  ? nodeInnterText.Substring(0, space)
        //                                  : nodeInnterText.Substring(0, node.InnerText.IndexOf("(")));
        //                }
        //                else
        //                {
        //                    isbns.Add(nodeInnterText);
        //                }
        //            }
        //        }

        //        return isbns;
        //    }
        //    catch (Exception ex)
        //    {
        //        og.Error(ex.Message, ex);
        //        throw;
        //    }
        //}
    }
}
