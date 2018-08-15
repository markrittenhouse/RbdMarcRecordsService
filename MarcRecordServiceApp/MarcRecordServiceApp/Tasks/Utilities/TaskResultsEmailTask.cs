using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcRecordServiceApp.Core;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.Utilities;

namespace MarcRecordServiceApp.Tasks.Utilities
{
    public class TaskResultsEmailTask : TaskBase
    {
        public const string ErrorBackgroundCell = "background-color: #f00;";
        public const string OkBackgroundCell = "";
        public const string TaskResultBackground = "background-color: #aaa;";

        public TaskResultsEmailTask() : base("Task Results Email Task", "TaskResultsEmailTask")
        {

            
        }

        public override void Run()
        {
            TaskResultStep resultsStep = new TaskResultStep
            {
                Name = "Send.TaskResults.ReportEmail",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(resultsStep);

            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-1);

                var taskResults = TaskResultFactory.GetTaskResultsFromDate(startDate, endDate, TaskResult.Id);


                int totalErrors = 0;

                const string rowStyle = "";
                StringBuilder taskHtml = new StringBuilder();

                string bTop = HtmlControlBuilder.BorderTop;
                string bBottom = HtmlControlBuilder.BorderBottom;
                string bLeft = HtmlControlBuilder.BorderLeft;
                string bRight = HtmlControlBuilder.BorderRight;
                string text13 = HtmlControlBuilder.TextStyle13;
                string text12 = HtmlControlBuilder.TextStyle12;
                string text10 = HtmlControlBuilder.TextStyle10;
                string text12Bold = HtmlControlBuilder.TextStyle12Bold;

                foreach (TaskResult taskResult in taskResults)
                {
                    taskHtml.AppendLine($@"
<table border='0' cellpadding='0' cellspacing='0' style='width:668px;'>
    <tr style='{TaskResultBackground}'>
        <td style ='{bTop}{bBottom}{bLeft}width:60px;' align='left' valign='top'>
            <span style='{text12}'>Id: {taskResult.Id}</span>
        </td>
        <td style='{bTop}{bBottom}' align='left' valign='top'>
            <span style='{text12}'>Task: {taskResult.Name} started at {taskResult.StartTime:HH:mm:ss} ({taskResult.Steps?.Select(x => x.CompletedSuccessfully).Count()}/{taskResult.Steps?.Count})</span>
        </td>
        <td style='{bTop}{bBottom}{bRight}{(taskResult.CompletedSuccessfully ? OkBackgroundCell : ErrorBackgroundCell)} width:100px;' align='center' valign='top'>
            <span style='{text12}'>Status: {(taskResult.CompletedSuccessfully ? "OK" : "ERROR")}</span>
        </td>
    </tr>
");

                    if (!taskResult.CompletedSuccessfully)
                    {
                        totalErrors++;
                    }

                    if (taskResult.Steps != null)
                    {


                        foreach (TaskResultStep step in taskResult.Steps) //.Where(x => !x.CompletedSuccessfully))
                        {
                            taskHtml.AppendLine($@"
<tr>
    <td align='left' valign='top' style='{rowStyle}{bLeft}'><div style='{text10}'>&nbsp;</div></td>
    <td align='left' valign='top' style='{rowStyle}'><div style='{text10}'>{step.Id} - {step.Name}</div></td>
    <td align='center' valign='top' style='{rowStyle}{bRight}{
                                    (taskResult.CompletedSuccessfully ? OkBackgroundCell : ErrorBackgroundCell)
                                }'><div style='{text10}'>{(step.CompletedSuccessfully ? "OK" : "ERROR")}</div></td>
</tr>
");

                        }
                    }

                    taskHtml.AppendLine("</table>");
                }

                var totalSteps = taskResults.Sum(x => x.Steps?.Count);


                StringBuilder bodyHtml = new StringBuilder()
                    .AppendLine($@"
<table border='0' cellpadding='0' cellspacing='0' style='width:668px;'>
    <tr>
        <td align='center' valign='top' colspan='2'><div style='{text13}'>Batch Processes from {startDate:g} to {endDate:g}</div></td>
    </tr>
    <tr>
        <td style='width:334px;' align='right' valign='top'><div style='{text12}'>Total Batches:&nbsp;</div></td>
        <td style='width:334px;' align='left' valign='top'><div style='{text12Bold}'>&nbsp;{taskResults.Count}</div></td>
    </tr>
    <tr>
        <td style='width:334px;' align='right' valign='top'><div style='{text12}'>Total Steps:&nbsp;</div></td>
        <td style='width:334px;' align='left' valign='top'><div style='{text12Bold}'>&nbsp;{totalSteps}</div></td>
    </tr>
    <tr>
        <td style='width:334px;' align='right' valign='top'><div style='{text12}'>Total Errors:&nbsp;</div></td>
        <td style='width:334px;' align='left' valign='top'><div style='{text12Bold}'>&nbsp;{totalErrors}</div></td>
    </tr>
</table>");

                string subject =
                    $"{(totalErrors == 0 ? "OK" : $"{totalErrors} ERRORS")} - {Environment.MachineName} - Marc Records Utilities Loader Results ({DatabaseConnection.DataSource})";
                Log.Debug($"subject: {subject}");
                bodyHtml.Append(taskHtml);
                Log.InfoFormat(bodyHtml.ToString());

                EmailMessageData emailMessageData = new EmailMessageData//(_utilitiesSettings)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    MessageBody = bodyHtml.ToString(),
                    ToRecipients = EmailSettings.TaskEmailConfig.ToAddresses.ToArray(),
                    CcRecipients = EmailSettings.TaskEmailConfig.CcAddresses.ToArray(),
                    BccRecipients = EmailSettings.TaskEmailConfig.BccAddresses.ToArray()
                };
                if (!EmailSettings.TaskEmailConfig.Send)
                {
                    StringBuilder warningMsg = new StringBuilder()
                        .AppendLine().AppendLine()
                        .AppendLine("----------------------------------------------------------------------------------------------------")
                        .AppendLine("----------------------------------------------------------------------------------------------------")
                        .AppendLine("------- STATUS EMAIL MESSAGES FOR THIS TASK ARE DISABLED IN THE EMAIL CONFIGURATION XML FILE -------")
                        .AppendLine("----------------------------------------------------------------------------------------------------")
                        .AppendLine("----------------------------------------------------------------------------------------------------")
                        .AppendLine();
                    Log.Warn(warningMsg);
                }
                else
                {
                    bool messageSendOk = emailMessageData.Send();
                    Log.Info($"messageSendOk: {messageSendOk}");
                }


                resultsStep.CompletedSuccessfully = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                resultsStep.CompletedSuccessfully = false;
                resultsStep.Results.Append(ex.Message);
            }
            finally
            {
                resultsStep.EndTime = DateTime.Now;
                TaskResultFactory.InsertTaskResultStep(resultsStep);
            }


        }
    }
}
