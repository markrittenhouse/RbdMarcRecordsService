using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.Utilities
{
    public class ParseMarcFieldsTask : TaskBase2
    {
        private readonly CallNumberFactory _callNumberFactory;
        private readonly MarcFieldParsingFactory _marcFieldParsingFactory;

        public ParseMarcFieldsTask()
            : base("Parse Marc Fields From LC and NLM Records", "ParseMarcFieldsTask")
        {
            _callNumberFactory = new CallNumberFactory();
            _marcFieldParsingFactory = new MarcFieldParsingFactory();
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "ParseMarcFieldsTask.Run",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);


            try
            {
                int batchSize = 10000;

                _marcFieldParsingFactory.ClearMarcRecordDataFieldTables();
                //_marcFieldParsingFactory.ClearOldMarcRecordData();

                var counterBatch = 0;
                int marcRecordDataFieldsCounter = 0;
                Stopwatch timer = new Stopwatch();
                timer.Start();

                List<MarcRecordData> marcRecords = _callNumberFactory.GetNlmAndLcMarcRecords(batchSize);

                while (marcRecords.Count > 0)
                {
                    List<MarcRecordDataField> marcRecordDataFieldsBatch = new List<MarcRecordDataField>();

                    foreach (MarcRecordData nlmMarcRecord in marcRecords)
                    {
                        List<MarcRecordDataField> marcRecordDataFields = GetMarcRecordDataFields(nlmMarcRecord);
                        marcRecordDataFieldsCounter += marcRecordDataFields.Count;
                        marcRecordDataFieldsBatch.AddRange(marcRecordDataFields);
                        counterBatch++;
                    }
                    Stopwatch timer2 = new Stopwatch();
                    timer2.Start();
                    Console.WriteLine("Insert Starting");
                    _marcFieldParsingFactory.InsertMarcRecordDataFields(marcRecordDataFieldsBatch);
                    Console.WriteLine(timer2.ElapsedMilliseconds);

                    marcRecords = _callNumberFactory.GetNlmAndLcMarcRecords(batchSize);
                }

                Console.WriteLine(timer.ElapsedMilliseconds);

                step.CompletedSuccessfully = true;

                step.Results.Append($"Marc Records found {marcRecords.Count}");
                step.Results.Append($", Fields found: {marcRecordDataFieldsCounter}");
                step.Results.Append($", Records Inserted {counterBatch}");
                step.Results.Append($" in {timer.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                step.Results.Insert(0, $"EXCEPTION: {ex.Message}");
                step.CompletedSuccessfully = false;
                throw;
            }
            finally
            {
                step.EndTime = DateTime.Now;
                TaskResultFactory.InsertTaskResultStep(step);
            }
        }

        private List<MarcRecordDataField> GetMarcRecordDataFields(MarcRecordData marcRecord)
        {
            List<MarcRecordDataField> marcRecordDataFields = new List<MarcRecordDataField>();

            var fields = marcRecord.FileData.Split('\r').Select(x=> x.Replace("\n", ""));
            foreach (var field in fields.Where(x=> !string.IsNullOrWhiteSpace(x)))
            {
                MarcRecordDataField marcRecordDataField =
                    new MarcRecordDataField
                    {
                        MarcRecordId = marcRecord.MarcRecordId,
                        ProviderId = marcRecord.ProviderId,
                        MarcValue = field,
                        FieldNumber = field.Substring(1, 3)
                    };

                if (field.Length >= 9 && field.Substring(8, 1) == "$")
                {
                    marcRecordDataField.FieldIndicator = field.Substring(6, 2);
                    var subFields = Regex.Split(field, @"(\$[a-zA-Z0-9])");
                    var testSubFields = subFields.Skip(1);
                    MarcRecordDataSubField marcRecordDataSubField = new MarcRecordDataSubField();
                    foreach (var subField in testSubFields)
                    {
                        
                        if (subField.Contains("$"))
                        {
                            marcRecordDataSubField = new MarcRecordDataSubField {SubFieldIndicator = subField};
                        }
                        else
                        {
                            marcRecordDataSubField.SubFieldValue = subField;
                            marcRecordDataField.MarcRecordDataSubFields.Add(marcRecordDataSubField);
                        }
                    }
                }
                marcRecordDataFields.Add(marcRecordDataField);

            }


            return marcRecordDataFields;
        }
    }
}