using System;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class DailyMarcRecordsTask : TaskBase2
    {
        public DailyMarcRecordsTask()
            : base("Generate Daily MArC Records", "CreateDailyMarcRecords")
        {
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "DailyMarcRecords.Generator",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {
                step.CompletedSuccessfully = AggregateMarcRecords();
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

        private bool AggregateMarcRecords()
        {
            Log.Debug(">>>>>>>>Truncating DailyMarcRecordFile");
            int originalCount = MarcRecordsProductFactory.TruncateDailyMarcRecords();
            Log.DebugFormat("files truncated : {0}", originalCount);

            Log.Debug(">>>>>>>>Adding NLM Marc Records");
            int nlmCount = MarcRecordsProductFactory.InsertDailyNlmMarcRecords();
            Log.DebugFormat("files added : {0}", nlmCount);

            Log.Debug(">>>>>>>>Adding LC Marc Records");
            int lcCount = MarcRecordsProductFactory.InsertDailyLcMarcRecords();
            Log.DebugFormat("files added : {0}", lcCount);

            Log.Debug(">>>>>>>>Adding Rittenhouse Marc Records");
            int rittenhouseCount = MarcRecordsProductFactory.InsertDailyRittenhouseMarcRecords();
            Log.DebugFormat("files added : {0}", rittenhouseCount);

            Log.DebugFormat("files truncated : {0}  ||  files added : {1}", rittenhouseCount, (nlmCount + lcCount + rittenhouseCount));

            MarcRecordsProductFactory.ReIndexDailyMarcRecords();
            return true;
        }
    }
}
