using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class MarcRecordsTask : TaskBase
    {

        private readonly StringBuilder _results = new StringBuilder();

        private MarcRecordProviderType ProviderType{ get; }

        private readonly MarcRecordService _marcRecordService;
        private int BatchSize { get; }

        private int _recordsFound;


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
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = $"MarcRecords.Generator  - {ProviderType.ToString()}",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {
                step.CompletedSuccessfully = ProcessMarcRecordBatchs(step);
                step.Results.Append($"Marc Record Provider: {ProviderType.ToString()}");
                step.Results.Append($"Records To Find {BatchSize} || Records Found: {_recordsFound} ");
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

        private bool ProcessMarcRecordBatchs(TaskResultStep step)
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

                Stopwatch timer = new Stopwatch();
                timer.Start();
                var fiveHours = 1.8e+7;
                while ((batchProductProcessed = ProcessNextBatchOfNewProducts(Settings.Default.MarcRecordBatchSize)) > 0)
                {
                    if (timer.ElapsedMilliseconds >= fiveHours)
                    {
                        step.Results.Append("Ending Process after 5 hours of run time.");
                        break;
                    }
                    Log.InfoFormat("batchProductProcessed: {0}", batchProductProcessed);
                    newProductsProcessed += batchProductProcessed;
                    Log.InfoFormat("newProductsProcessed: {0}", newProductsProcessed);
                    if (newProductsProcessed >= BatchSize)
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
    }
}
