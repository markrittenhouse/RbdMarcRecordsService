using System;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.CallNumbers
{
    public class CallNumberTask : TaskBase2
    {
        private readonly CallNumberFactory _callNumberFactory;
        public CallNumberTask() 
            : base("Insert/Update Call Numbers from NLM and LC into MarcRecordDataCallNumber.", "CallNumberTask")
        {
            _callNumberFactory = new CallNumberFactory();
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "CallNumberTask.Run",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {
                Log.Debug("Start BuildLcCallNumbers");
                var lcCallNumbersAddedOrUpdated = _callNumberFactory.BuildLcCallNumbers();

                Log.Debug("Start BuildNlmCallNumbers");
                var nlmCallNumbersAddedOrUpdated = _callNumberFactory.BuildNlmCallNumbers();

                step.CompletedSuccessfully = true;

                step.Results.Append($"LC Call Numbers Inserted/Updated {lcCallNumbersAddedOrUpdated}");
                step.Results.Append($"NLM Call Numbers Inserted/Updated {nlmCallNumbersAddedOrUpdated}");
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
    }
}
