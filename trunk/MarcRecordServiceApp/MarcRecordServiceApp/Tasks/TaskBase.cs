using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.Utilities;
using MarcRecordServiceApp.EmailConfigs;
using log4net;

namespace MarcRecordServiceApp.Tasks
{
    public abstract class TaskBase2 : ITask
    {
        private readonly Random _random = new Random();
        private readonly DateTime _currentDateTime = DateTime.Now;

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


        public string GetRbdMrkFileText(Product product)
        {
            try
            {
                string publicationYearText = product.PublicationYearText;

                if (publicationYearText == "")
                {
                    Log.DebugFormat("Id: {0}, Sku: {1}, Isbn13: {2}", product.Id, product.Sku, product.Isbn13);
                }

                StringBuilder mrkFileText = new StringBuilder();
                string sitepath = Settings.Default.SiteSubDirectory;

                mrkFileText.AppendFormat("=LDR  {0}nam  22{1}2a 4500", GetNext5DigitRandomNumber(),
                                         GetNext5DigitRandomNumber()).AppendLine();

                Log.DebugFormat("PublicationYearText: {0}, PublicationYear: {1}, sku: {2}", product.PublicationYearText,
                                product.PublicationYear, product.Sku);


                string line001 = string.Format("=001  {0}", product.Sku);
                string line005 = string.Format("=005  {0:yyyyMMddhhmmss}.0", DateTime.Now);

                string line008 =
                    string.Format("=008  {0:yyMMdd}s{1:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ",
                                  _currentDateTime, product.PublicationYear);

                Log.DebugFormat("line008: {0}", line008);
                Log.DebugFormat("line008.Length: {0}", line008.Length);

                mrkFileText.AppendLine(line001);
                mrkFileText.AppendLine(line005);
                mrkFileText.AppendLine(line008);

                //The MRC file requires an additional \\. For some reason it loses when when converted
                //string extraSlashes = (mrkOnly) ? "" : "\\";
                const string extraSlashes = "";
                mrkFileText.AppendFormat("=020  \\\\{1}$a{0}", product.Isbn10, extraSlashes).AppendLine()
                    .AppendFormat("=020  \\\\{1}$a{0}", product.Isbn13, extraSlashes).AppendLine();

                mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc")
                    .AppendFormat("=100  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
                    .AppendFormat("=245  10$a{0}", StripOffCarriageReturnAndLineFeed(product.Title)).AppendLine();

                mrkFileText.AppendFormat("=260  \\\\$b{0},$c{1}", product.PublisherName, publicationYearText).AppendLine
                    ();
                mrkFileText.AppendFormat(
                    "=533  \\\\$a{0}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{1}", product.Format,
                    publicationYearText)
                    .AppendLine();

                mrkFileText.AppendFormat("=650  \\0$a{0}.", product.CategoryName).AppendLine()
                    .AppendFormat("=700  1\\$a{0}", StripOffCarriageReturnAndLineFeed(product.Authors)).AppendLine()
                    .AppendFormat("=856  4\\$zConnect to this resource online$u{0}Products/Book.aspx?sku={1}", sitepath,
                                  product.Isbn10).AppendLine()
                    .AppendLine();

                return mrkFileText.ToString();
            }
            catch (Exception ex)
            {
                if (product != null)
                {
                    Log.Info(product.ToString());
                    Log.InfoFormat("Id: {0}", product.Id);
                    Log.InfoFormat("Sku: {0}", product.Sku);
                    Log.InfoFormat("Isbn10: {0}", product.Isbn10);
                    Log.InfoFormat("Isbn13: {0}", product.Isbn13);
                    Log.InfoFormat("Title: {0}", product.Title);
                    Log.InfoFormat("Authors: {0}", product.Authors);
                    Log.InfoFormat("PublicationYearText: {0}", product.PublicationYearText);
                    Log.InfoFormat("PublicationDate: {0}", product.PublicationDate);
                    Log.InfoFormat("Copyright: {0}", product.Copyright);
                    Log.InfoFormat("Format: {0}", product.Format);
                    Log.InfoFormat("CategoryName: {0}", product.CategoryName);
                }
                else
                {
                    Log.Info("Product is null!");
                }
                Log.Error(ex.Message, ex);
                throw;
            }

        }

        private static string StripOffCarriageReturnAndLineFeed(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        private int GetNext5DigitRandomNumber()
        {
            int next = _random.Next(10000, 99999);
            return next;
        }

        public IEnumerable<XElement> SimpleStreamAxis2(string marcXml, string elementName)
        {
            XmlReaderSettings settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };

            using (XmlReader reader = XmlReader.Create(new StringReader(marcXml), settings))
            {
                reader.MoveToContent(); // will not advance reader if already on a content node; if successful, ReadState is Interactive
                reader.Read();          // this is needed, even with MoveToContent and ReadState.Interactive
                while (!reader.EOF && reader.ReadState == ReadState.Interactive)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(elementName))
                    {
                        var matchedElement = XNode.ReadFrom(reader) as XElement;
                        if (matchedElement != null)
                            yield return matchedElement;
                    }
                    else
                    {
                        reader.Read();
                    }
                }
            }
        }

    }
}
