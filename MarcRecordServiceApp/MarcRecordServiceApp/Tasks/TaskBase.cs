using System;
using System.Data.SqlClient;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.Utilities;
using MarcRecordServiceApp.EmailConfigs;
using log4net;

namespace MarcRecordServiceApp.Tasks
{
    public abstract class TaskBase2 : ITask
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        protected TaskEmailSettings EmailSettings { get; private set; }

		//protected SqlConnectionStringBuilder RittenhouseWebSqlConnection = new SqlConnectionStringBuilder(Properties.Settings.Default.RittenhouseWebDb);
		protected SqlConnectionStringBuilder DatabaseConnection = new SqlConnectionStringBuilder(Settings.Default.RittenhouseMarcDb);

        public TaskResult TaskResult { get; private set; }
        public TaskResult PreviousTaskResult { get; private set; }

        protected string BulkInsertDataFileName { get; set; }
        protected string BulkInsertDatabaseTable { get; set; }
        protected string BulkInsertDelimiter { get; set; }
        protected string WorkingDatabasePrefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="taskKey"></param>
        protected TaskBase2(string taskName, string taskKey)
        {
            //TaskName = taskName;

            EmailSettings = new TaskEmailSettings(taskKey);

            PreviousTaskResult = TaskResultFactory.GetPreviousTaskResult(taskName) ?? new TaskResult();

            TaskResult = new TaskResult {Name = taskName, StartTime = DateTime.Now, RunComments = "Init Complete"};
            TaskResultFactory.InsertTaskResult(TaskResult);
            Log.Debug(TaskResult);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendCompleteEmail()
        {
            // email results
            StringBuilder emailBody = new StringBuilder();
            AppendTaskReportText(emailBody);

            StringBuilder bodyHeader = new StringBuilder()
                .AppendFormat("Task Name: {0}", TaskResult.Name).AppendLine()
                .AppendFormat("Task Id:   {0}", TaskResult.Id).AppendLine()
                .AppendFormat("Run Time:  {0:0.00#}s - {1:M/d/yy hh:mm:ss.fff tt} to {2:M/d/yy hh:mm:ss.fff tt}", TaskResult.GetRunTime().TotalSeconds, TaskResult.StartTime, TaskResult.EndTime)
                .AppendLine()
                .AppendFormat("Status:    {0}", (TaskResult.CompletedSuccessfully) ? "Ok" : "ERROR").AppendLine().AppendLine();

            emailBody.Insert(0, bodyHeader.ToString());

        	string machineName = Environment.MachineName.ToLower();

            string subject = string.Format("{0} - Rittenhouse MARC Record Service on {1} - {2}", (TaskResult.CompletedSuccessfully) ? "Ok" : "ERROR",
										   machineName, TaskResult.Name);

            Log.DebugFormat("Send Complete Email: {0}", EmailSettings.SuccessEmailConfig.Send);
            Log.InfoFormat("subject: {0}\n{1}", subject, emailBody);

            if (!EmailSettings.SuccessEmailConfig.Send)
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
                return;
            }

			if ((TaskResult.CompletedSuccessfully))
			{
				SendCompleteEmail(subject, emailBody.ToString(), EmailSettings.SuccessEmailConfig);				
			}
			else
			{
				SendCompleteEmail(subject, emailBody.ToString(), EmailSettings.ErrorEmailConfig);
			}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private void AppendTaskReportText(StringBuilder text)
        {
            for (int i = (TaskResult.Steps.Count - 1); i >= 0; i--)
            {
                TaskResultStep step = TaskResult.Steps[i];

                text.AppendFormat("{0} - {1:0.000}s - Id: {2} - [{3}]", (step.CompletedSuccessfully) ? "ok" : "ERROR", step.GetRunTime().TotalSeconds, step.Id,
                                       step.Name).AppendLine();
                text.AppendFormat("\t{0}", step.Results).AppendLine().AppendLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
		/// <param name="emailConfiguration"></param>
        protected void SendCompleteEmail(string subject, string body, EmailConfiguration emailConfiguration)
        {
            EmailMessageData emailMessageData = new EmailMessageData
                                                    {
                                                        IsBodyHtml = false,
                                                        Subject = subject,
                                                        MessageBody = body,
														ToRecipients = emailConfiguration.ToAddresses.ToArray(),
														CcRecipients = emailConfiguration.CcAddresses.ToArray(),
														BccRecipients = emailConfiguration.BccAddresses.ToArray()
                                                    };

            bool messageSendOk = emailMessageData.Send();
            Log.InfoFormat("messageSendOk: {0}", messageSendOk);
        }

        public abstract void Run();

        /// <summary>
        /// 
        /// </summary>
        public void Cleanup()
        {
            TaskResult.EndTime = DateTime.Now;
            TaskResultFactory.UpdateTaskResult(TaskResult);
            SendCompleteEmail();
        }

    }
}
