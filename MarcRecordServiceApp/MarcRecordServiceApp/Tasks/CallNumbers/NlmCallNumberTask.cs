using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.CallNumbers
{
    public class NlmCallNumberTask : TaskBase2
    {
        private readonly CallNumberFactory _callNumberFactory;

        private int _totalNlmMarcRecordsParsed;
        private int _totalNlmMarcRecordsToBeParsed;

        public NlmCallNumberTask()
            : base("Update NLM Call Numbers", "UpdateNlmCallNumbers")
        {
            _callNumberFactory = new CallNumberFactory();
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "Update.NlmCallNumbers",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {
                int nlmRecordsUpdated = _callNumberFactory.UpdateNlmMarcRecordFiles();
                Log.InfoFormat("{0} NLM Marc Records Updated", nlmRecordsUpdated);

                int nlmRecordsInserted = _callNumberFactory.InsertNlmMarcRecordFiles();
                Log.InfoFormat("{0} NLM Marc Records Inserted", nlmRecordsInserted);

                _totalNlmMarcRecordsToBeParsed = _callNumberFactory.GetNlmCallNumberCount();

                const int batchSize = 150;

                List<NlmMarcRecordFile> nlmMarcFiles = _callNumberFactory.GetBatchedNlmMarcRecordFiles(batchSize);

                while (nlmMarcFiles.Count == batchSize)
                {
                    ProcessMarcRecordsForCallNumbers(nlmMarcFiles);

                    nlmMarcFiles = _callNumberFactory.GetBatchedNlmMarcRecordFiles(batchSize);
                }

                if (nlmMarcFiles.Any())
                {
                    ProcessMarcRecordsForCallNumbers(nlmMarcFiles);
                }

                step.CompletedSuccessfully = true;
                var result = new StringBuilder()
                    .AppendFormat("Inserted NLM MarcRecords: {0} || ", nlmRecordsInserted)
                    .AppendFormat("Updated NLM MarcRecords: {0} || ", nlmRecordsUpdated)
                    .AppendFormat("NLM Marc Records Parsed for Call Numbers : {0}", _totalNlmMarcRecordsParsed)
                    .ToString();

                step.Results.Insert(0, result);
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
        /// <param name="nlmMarcFiles"></param>
        private void ProcessMarcRecordsForCallNumbers(List<NlmMarcRecordFile> nlmMarcFiles)
        {
            foreach (var nlmMarcRecordFile in nlmMarcFiles)
            {
                PopulateNlmCallNumber(nlmMarcRecordFile);
            }

            if (nlmMarcFiles.Any())
            {
                var recordsToUpdate = nlmMarcFiles.ToList();
                _totalNlmMarcRecordsParsed += _callNumberFactory.UpdateNlmCallNumbers(recordsToUpdate);
                Log.InfoFormat("{0}/{1} NLM Call Numbers Updated", _totalNlmMarcRecordsParsed, _totalNlmMarcRecordsToBeParsed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nlmMarcRecordFile"></param>
        private void PopulateNlmCallNumber(NlmMarcRecordFile nlmMarcRecordFile)
        {
            IEnumerable<XElement> dataFields = SimpleStreamAxis2(nlmMarcRecordFile.XmlFileData, "datafield");
            string callNumber = ParseCallNumber(dataFields);
            string description = ParseDescription(dataFields);
            
            if (callNumber != null)
            {
                var nlmCallNumber = string.Format("[DNLM: 1.{0}]",
                                                  string.Format("{0}{1}",
                                                                description != null
                                                                    ? string.Format("{0}", description)
                                                                    : "", callNumber));
                nlmMarcRecordFile.NlmCallNumber = nlmCallNumber;
            }

        }

        /// <summary>
        /// Parses the Call Number from the xElements
        /// </summary>
        /// <param name="dataFields"></param>
        /// <returns></returns>
        private string ParseCallNumber(IEnumerable<XElement> dataFields)
        {
            IEnumerable<XElement> xElements =
                dataFields.Where(x => x.Attribute("tag") != null && x.Attribute("tag").Value == "060");
            if (xElements.Any())
            {
                StringBuilder callNumberBuilder = new StringBuilder();
                //Check if it is a LC created Call Number
                var nlmXelements = xElements.Where(x => x.Attribute("ind1") != null && x.Attribute("ind1").Value == "0");
                if (nlmXelements.Any())
                {
                    foreach (XElement xElement in nlmXelements.Nodes())
                    {
                        callNumberBuilder.AppendFormat("{0} ", xElement.Value);
                    }
                }
                if (callNumberBuilder.Length > 0)
                {
                    return callNumberBuilder.ToString(0, callNumberBuilder.Length - 1);
                }

            }
            return null;
        }

        /// <summary>
        /// Parses the Author or Category from the xElements
        /// </summary>
        /// <param name="dataFields"></param>
        /// <returns></returns>
        private string ParseDescription(IEnumerable<XElement> dataFields)
        {
            StringBuilder descriptionBuilder = new StringBuilder();
            //Author if exists
            var xElements = dataFields.Where(x => x.Attribute("tag") != null && x.Attribute("tag").Value == "600");
            if (xElements.Any())
            {
                var authorElements = xElements.FirstOrDefault();
                //Primary
                if (authorElements != null)
                {
                    foreach (XElement xElement in authorElements.Nodes())
                    {
                        descriptionBuilder.AppendFormat("{0} ", xElement.Value);
                    }
                }
            }
            //Catagories
            else
            {
                xElements = dataFields.Where(x => x.Attribute("tag") != null && x.Attribute("tag").Value == "650");
                if (xElements.Any())
                {
                    var catagoryElements = xElements.FirstOrDefault();

                    if (catagoryElements != null)
                    {
                        foreach (XElement xElement in catagoryElements.Nodes())
                        {
                            descriptionBuilder.AppendFormat("{0} ", xElement.Value);
                        }
                    }
                }
            }

            return descriptionBuilder.Length > 0 ? descriptionBuilder.ToString() : null;
        }
    }
}
