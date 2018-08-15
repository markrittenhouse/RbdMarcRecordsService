using System;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;

namespace MarcRecordServiceApp.Tasks.Utilities
{
    public class AdditionalMarcFieldTask : TaskBase
    {
        private readonly AdditionalMarcFieldFactory _additionalMarcFieldFactory;
        public AdditionalMarcFieldTask() : base("Additional Marc Field Task", "AdditionalMarcFieldTask")
        {
            _additionalMarcFieldFactory = new AdditionalMarcFieldFactory();
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "AdditionalMarcFieldTask.Run",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {
                var rowsTruncated = _additionalMarcFieldFactory.TruncateAdditionalMarcFields();
                var oclcCallNumberCount = _additionalMarcFieldFactory.InsertOclcControlNumbers();
                var oclcCrossReferenceControlNumberCount = _additionalMarcFieldFactory.InsertOclcCrossReferenceControlNumbers();
                var lcCallNumberCount = _additionalMarcFieldFactory.InsertLcCallNumber();
                var nlmCallNumberCount = _additionalMarcFieldFactory.InsertNlmCallNumber();

                step.CompletedSuccessfully = true;
                step.Results.Append($"AdditionalMarcField rows truncated: {rowsTruncated}").AppendLine();
                step.Results.Append($"OCLC Control Numbers found: {oclcCallNumberCount}").AppendLine();
                step.Results.Append($"OCLC Cross Refernece Numbers found: {oclcCrossReferenceControlNumberCount}").AppendLine();
                step.Results.Append($"LC Call Numbers found: {lcCallNumberCount}").AppendLine();
                step.Results.Append($"NLM Call Numbers found: {nlmCallNumberCount}").AppendLine();

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
