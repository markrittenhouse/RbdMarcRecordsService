using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.CallNumbers
{
    public class LcCallNumberTask : TaskBase2
    {
        private readonly CallNumberFactory _callNumberFactory;

        private int _totalLcMarcRecords;

        public LcCallNumberTask()
            : base("Update Lc Call Numbers", "UpdateLcCallNumbers")
        {
            _callNumberFactory = new CallNumberFactory();
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
                                      {
                                          Name = "Update.LcCallNumbers",
                                          TaskResultId = TaskResult.Id,
                                          StartTime = DateTime.Now
                                      };
            TaskResult.AddStep(step);

            try
            {
                int lcRecordsUpdated = _callNumberFactory.UpdateLcMarcRecordFiles();
                Log.InfoFormat("{0} Lc Marc Records Updated", lcRecordsUpdated );

                int lcRecordsInserted = _callNumberFactory.InsertLcMarcRecordFiles();
                Log.InfoFormat("{0} Lc Marc Records Inserted", lcRecordsInserted);

                const int batchSize = 150;

                List<LcMarcRecordFile> lcMarcFiles = _callNumberFactory.GetBatchedLcMarcRecordFiles(batchSize);

                while (lcMarcFiles.Count == batchSize)
                {
                    ProcessMarcRecordsForCallNumbers(lcMarcFiles);

                    lcMarcFiles = _callNumberFactory.GetBatchedLcMarcRecordFiles(batchSize);
                }

                if (lcMarcFiles.Any())
                {
                    ProcessMarcRecordsForCallNumbers(lcMarcFiles);
                }

                step.CompletedSuccessfully = true;
                step.Results.Insert(0,
                                    string.Format(
                                        "Inserted LC MarcRecords: {0} || Updated LC MarcRecords: {1} || Updated LC Call Numbers : {2}",
                                        lcRecordsInserted, lcRecordsUpdated, +_totalLcMarcRecords));
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

        private void ProcessMarcRecordsForCallNumbers(List<LcMarcRecordFile> lcMarcFiles)
        {
            foreach (var lcMarcRecordFile in lcMarcFiles)
            {
                PopulateLcCallNumber(lcMarcRecordFile);
            }

            var lcMarcRecordFilesToUpdate = lcMarcFiles.Where(x => x.UpdateCallNumber);
            if (lcMarcRecordFilesToUpdate.Any())
            {
                var recordsToUpdate = lcMarcRecordFilesToUpdate.ToList();
                _totalLcMarcRecords += _callNumberFactory.UpdateLcCallNumbers(recordsToUpdate);
                Log.InfoFormat("{0} LC Call Numbers Updated", recordsToUpdate.Count);
            }
        }

        private void PopulateLcCallNumber(LcMarcRecordFile lcMarcRecordFile)
        {
            IEnumerable<XElement> dataFields = SimpleStreamAxis2(lcMarcRecordFile.XmlFileData, "datafield");

            string callNumber = null;
            IEnumerable<XElement> xElements = dataFields.Where(x => x.Attribute("tag") != null && x.Attribute("tag").Value == "050");
            if (xElements.Any())
            {
                StringBuilder callNumberBuilder = new StringBuilder();
                //Check if it is a LC created Call Number
                var lcXelements = xElements.Where(x => x.Attribute("ind2") != null && x.Attribute("ind2").Value == "0");
                if (lcXelements.Any())
                {
                    foreach (XElement xElement in lcXelements.Nodes())
                    {
                        callNumberBuilder.AppendFormat("{0} ", xElement.Value);
                    }
                }
                else
                {
                    //if Not LC created Call Number and there is no LC call number at least get something. 
                    foreach (XElement xElement in xElements.Nodes())
                    {
                        callNumberBuilder.AppendFormat("{0} ", xElement.Value);
                    }
                }
                
                callNumber = callNumberBuilder.ToString(0, callNumberBuilder.Length - 1);
            }

            if (lcMarcRecordFile.LcCallNumber == null || (lcMarcRecordFile.LcCallNumber != callNumber && !string.IsNullOrWhiteSpace(callNumber)))
            {
                lcMarcRecordFile.UpdateCallNumber = true;
                lcMarcRecordFile.LcCallNumber = callNumber;
            }
        }
    }
}
