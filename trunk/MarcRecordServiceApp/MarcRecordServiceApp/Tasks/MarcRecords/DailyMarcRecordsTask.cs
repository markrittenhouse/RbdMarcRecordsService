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
            Log.Info(">>>>>>>>Truncating DailyMarcRecordFile");
            int originalCount = DailyMarcRecordFactory.TruncateDailyMarcRecords();
            Log.InfoFormat("files truncated : {0}", originalCount);

            //Log.Info(">>>>>>>>Adding NLM Marc Records");
            //int nlmCount = DailyMarcRecordFactory.InsertDailyNlmMarcRecords();
            //Log.InfoFormat("files added : {0}", nlmCount);

            //Log.Info(">>>>>>>>Adding LC Marc Records");
            //int lcCount = DailyMarcRecordFactory.InsertDailyLcMarcRecords();
            //Log.InfoFormat("files added : {0}", lcCount);

            Log.Info(">>>>>>>>Adding Rittenhouse Marc Records");
            int rittenhouseCount = DailyMarcRecordFactory.InsertDailyRittenhouseMarcRecords();
            Log.InfoFormat("files added : {0}", rittenhouseCount);

            //Log.InfoFormat("files truncated : {0}  ||  files added : {1}", originalCount, (nlmCount + lcCount + rittenhouseCount));
            Log.InfoFormat("files truncated : {0}  ||  files added : {1}", originalCount, rittenhouseCount);

            DailyMarcRecordFactory.ReIndexDailyMarcRecords();
            return true;
        }
    }
}
