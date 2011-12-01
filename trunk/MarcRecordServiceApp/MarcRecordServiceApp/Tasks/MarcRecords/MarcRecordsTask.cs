using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MarcRecordServiceApp.Core;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class MarcRecordsTask : TaskBase2
    {
        private readonly int _batchSize = Settings.Default.MarcRecordBatchSize;

        private readonly Random _random = new Random();
        private readonly DateTime _currentDateTime = DateTime.Now;
        private readonly StringBuilder _results = new StringBuilder();

        /// <summary>
        /// 
        /// </summary>
        public MarcRecordsTask()
            : base("Generate MArC Records", "CreateMarcRecords")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep { Name = "MarcRecords.Generator", TaskResultId = TaskResult.Id, StartTime = DateTime.Now };
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ProcessMarcRecordBatchs()
        {
            try
            {
                string workingDirectory = (Settings.Default.MarcFilesWorkingDirectory.EndsWith(@"\"))
                                              ? Settings.Default.MarcFilesWorkingDirectory
                                              : string.Format(@"{0}\", Settings.Default.MarcFilesWorkingDirectory);
                Log.InfoFormat("workingDirectory:\n{0}", workingDirectory);
                Log.InfoFormat("MarcRecordGeneratorMaxProducts: {0}", Properties.Settings.Default.MarcRecordGeneratorMaxProducts);
                Log.InfoFormat("MarcRecordBatchSize: {0}", Properties.Settings.Default.MarcRecordBatchSize);

                int productsWithoutMarcRecordsCount = MarcRecordsProductFactory.GetProductsWithoutMarcRecordsCount();
                Log.InfoFormat("productsWithoutMarcRecordsCount: {0}", productsWithoutMarcRecordsCount);

                int productsWithMarcRecordsCountRequiringUpdate = MarcRecordsProductFactory.GetProductsWithMarcRecordsCountForUpdate();
                Log.InfoFormat("productsWithMarcRecordsCountRequiringUpdate: {0}", productsWithMarcRecordsCountRequiringUpdate);

                ClearWorkingDirectory(workingDirectory);

                int newProductsProcessed = 0;
                int existingProductsProcessed = 0;

                int batchProductProcessed; 
                while ((batchProductProcessed = ProcessNextBatchOfNewProducts(_batchSize, workingDirectory)) > 0)
                {
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    newProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("newProductsProcessed: {0}", newProductsProcessed);
                    if (newProductsProcessed >= Properties.Settings.Default.MarcRecordGeneratorMaxProducts)
                    {
                        break;
                    }
                }

                while ((batchProductProcessed = ProcessNextBatchOfUpdatedProducts(_batchSize, workingDirectory)) > 0)
                {
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    existingProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("existingProductsProcessed: {0}", existingProductsProcessed);
                    Log.InfoFormat("total products processed: {0}", (newProductsProcessed + existingProductsProcessed));
                    if ((newProductsProcessed + existingProductsProcessed) >= Properties.Settings.Default.MarcRecordGeneratorMaxProducts)
                    {
                        break;
                    }
                }
                _results.AppendFormat("{0,6} - Total Products Processed", (newProductsProcessed + existingProductsProcessed)).AppendLine();
                _results.AppendFormat("\t{0,6} - Existing Products Processed", existingProductsProcessed).AppendLine();
                _results.AppendFormat("\t{0,6} - New Products Processed", newProductsProcessed).AppendLine().AppendLine();

                _results.AppendFormat("\t{0,6} - Products Without Marc Records Requiring Processing", productsWithoutMarcRecordsCount).AppendLine();
                _results.AppendFormat("\t{0,6} - Products With Marc Records Requiring Processing", productsWithMarcRecordsCountRequiringUpdate).AppendLine().AppendLine();

                _results.AppendFormat("\t{0,6} - Max Products to Process", Properties.Settings.Default.MarcRecordGeneratorMaxProducts).AppendLine();
                _results.AppendFormat("\t{0,6} - Batch Size", Properties.Settings.Default.MarcRecordBatchSize).AppendLine().AppendLine();
                
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                //_results.AppendFormat("EXCEPTION: {0}", ex.Message);
                _results.Insert(0, string.Format("EXCEPTION: {0}\n\r\t", ex.Message));
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDirectory"></param>
        private void ClearWorkingDirectory(string workingDirectory)
        {
            string[] filesToDelete = Directory.GetFiles(workingDirectory);
            foreach (string file in filesToDelete)
            {
                if (file.EndsWith(".mrk") || file.EndsWith(".mrc") || file.EndsWith(".cmd"))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int ProcessNextBatchOfNewProducts(int batchSize, string workingDirectory)
        {
            int productProcessedCount = 0;
          
            // get next batch to process
            Product[] products = MarcRecordsProductFactory.GetProductsWithoutMarcRecords(batchSize);
            Log.InfoFormat("batch contains {0} products", products.Length);
            if (products.Length == 0)
            {
                return 0;
            }

            IEnumerable<MarcFileData> marcFileDatas = GetMarcFileDataForProducts(products, workingDirectory);

            try
            {
                foreach (MarcFileData marcFileData in marcFileDatas)
                {
                    int rowsSaved = MarcRecordsProductFactory.InsertProductmarcRecord(marcFileData.Product.Id, marcFileData.MrkFileText,
                                                                                      marcFileData.MrcFileText);
                    Log.DebugFormat("rowsSaved: {0}", rowsSaved);

                    productProcessedCount++;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            return productProcessedCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private int ProcessNextBatchOfUpdatedProducts(int batchSize, string workingDirectory)
        {
            int productProcessedCount = 0;

            // get next batch to process
            Product[] products = MarcRecordsProductFactory.GetProductsWithMarcRecordsForUpdate(batchSize);
            Log.InfoFormat("batch contains {0} products", products.Length);
            if (products.Length == 0)
            {
                return 0;
            }

            IEnumerable<MarcFileData> marcFileDatas = GetMarcFileDataForProducts(products, workingDirectory);

            try
            {
                foreach (MarcFileData marcFileData in marcFileDatas)
                {
                    int rowsSaved = MarcRecordsProductFactory.UpdateProductmarcRecord(marcFileData.Product.Id, marcFileData.MrkFileText,
                                                                                      marcFileData.MrcFileText);
                    Log.DebugFormat("rowsSaved: {0}", rowsSaved);

                    productProcessedCount++;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            return productProcessedCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="products"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private IEnumerable<MarcFileData> GetMarcFileDataForProducts(Product[] products, string workingDirectory)
        {
            List<MarcFileData> marcFileDatas = new List<MarcFileData>();
                
            StringBuilder compositeMrkFileToProcess = new StringBuilder();

            foreach (Product product in products)
            {
                string mrkFileTextToProcess = GetProductMrkFileText(product, true);
                Log.DebugFormat("mrkFileTextToProcess:\n{0}", mrkFileTextToProcess);
                compositeMrkFileToProcess.Append(mrkFileTextToProcess);
            }

            string batchFileNameBase = string.Format("batch_{0:yyyyMMdd_HHmmssfff}", DateTime.Now);

            string mrkFilePath = string.Format(@"{0}{1}.mrk", workingDirectory, batchFileNameBase);
            Log.DebugFormat("mrkFilePath: {0}", mrkFilePath);

            string mrcFilePath = string.Format(@"{0}{1}.mrc", workingDirectory, batchFileNameBase);
            Log.DebugFormat("mrcFilePath: {0}", mrcFilePath);

            File.WriteAllText(mrkFilePath, compositeMrkFileToProcess.ToString());

            string args = string.Format(" -s \"{0}\" -d \"{1}\" -make -pd", mrkFilePath, mrcFilePath);

            StringBuilder commandLineText = new StringBuilder()
                .AppendFormat("\"{0}\" {1}", Properties.Settings.Default.MarcEditExeLocation, args.Replace(".mrc", "_test.mrc"))
                .AppendLine()
                .AppendLine("pause");
            File.WriteAllText(string.Format(@"{0}{1}.cmd", workingDirectory, batchFileNameBase), commandLineText.ToString());

            Process process = new Process
            {
                StartInfo =
                {
                    FileName = Properties.Settings.Default.MarcEditExeLocation,
                    Arguments = args,
                    CreateNoWindow = false,
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(15000);     // 15 seconds

                Log.DebugFormat("ExitCode: {0}", process.ExitCode);
                process.Close();

                StreamReader streamReaderToParse = File.OpenText(mrcFilePath);
                string mrcFileTextToParse = streamReaderToParse.ReadToEnd();
                streamReaderToParse.Close();
                Log.DebugFormat("mrcFileTextToParse:\n{0}", mrcFileTextToParse);

                if (string.IsNullOrEmpty(mrcFileTextToParse))
                {
                    throw new RittenhouseException(".mrc file was empty", "emptyMrcFile");
                }

                string[] individualMrcFiles = mrcFileTextToParse.Split(MarcFileData.GroupSeparator);

                foreach (string individualMrcFile in individualMrcFiles)
                {
                    if (!string.IsNullOrEmpty(individualMrcFile))
                    {
                        Log.DebugFormat("mrc: {0}", individualMrcFile);

                        MarcFileData marcFileData = new MarcFileData(individualMrcFile);

                        marcFileData.SetProduct(products);

                        if (marcFileData.Product != null)
                        {
                            Log.DebugFormat("sku: {0}", marcFileData.Product.Sku);
                            marcFileData.MrkFileText = GetProductMrkFileText(marcFileData.Product, false);
                            marcFileDatas.Add(marcFileData);
                        }
                        else
                        {
                            Log.Warn("marcFileData.Product is null");
                            Log.DebugFormat("mrc: {0}", individualMrcFile);
                            Log.DebugFormat("mrc: {0}", marcFileData.Isbn10);
                            Log.DebugFormat("mrc: {0}", marcFileData.Isbn13);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }

            return marcFileDatas.ToArray();
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
        /// <param name="product"></param>
        /// <param name="mrkOnly"></param>
        /// <returns></returns>
        private string GetProductMrkFileText(Product product, bool mrkOnly)
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

                mrkFileText.AppendFormat("=LDR  {0}nam  22{1}2a 4500", GetNext5DigitRandomNumber(), GetNext5DigitRandomNumber()).AppendLine();

                Log.DebugFormat("PublicationYearText: {0}, PublicationYear: {1}, sku: {2}", product.PublicationYearText, product.PublicationYear, product.Sku);

                string line008 = string.Format("=008  {0:yyMMdd}s{1:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ",
                                         _currentDateTime, product.PublicationYear);

                Log.DebugFormat("line008: {0}", line008);
                Log.DebugFormat("line008.Length: {0}", line008.Length);

                mrkFileText.AppendLine(line008);

                //The MRC file requires an additional \\. For some reason it loses when when converted
                string extraSlashes = (mrkOnly) ? "" : "\\";
                mrkFileText.AppendFormat("=20  \\\\{1}$a{0}", product.Isbn10, extraSlashes).AppendLine()
                    .AppendFormat("=20  \\\\{1}$a{0}", product.Isbn13, extraSlashes).AppendLine();

                mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc")
                    .AppendFormat("=100  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
                    .AppendFormat("=245  10$a{0}", StripOffCarriageReturnAndLineFeed(product.Title)).AppendLine();

                mrkFileText.AppendFormat("=260  \\\\$b{0},$c{1}", product.PublisherName, publicationYearText).AppendLine();
                mrkFileText.AppendFormat("=533  \\\\$a{0}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{1}", product.Format, publicationYearText)
                    .AppendLine();

                mrkFileText.AppendFormat("=650  \\0$a{0}.", product.CategoryName).AppendLine()
                    .AppendFormat("=700  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
                    .AppendFormat("=856  4\\$zConnect to this resource online$u{0}Products/Book.aspx?sku={1}", sitepath, product.Isbn10).AppendLine()
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
        /// <returns></returns>
        private int GetNext5DigitRandomNumber()
        {
            int next = _random.Next(10000, 99999);
            return next;
        }

    }
}
