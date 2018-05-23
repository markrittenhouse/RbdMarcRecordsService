using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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


        //public string GetRbdMrkFileText(Product product, List<ParsedMarcField> marcFieldsToAdd)
        public string GetRbdMrkFileText(Product product)
        {
            try
            {
                string publicationYearText = product.PublicationYearText;

                if (publicationYearText == "")
                {
                    Log.Debug($"Id: {product.Id}, Sku: {product.Sku}, Isbn13: {product.Isbn13}");
                }

                StringBuilder mrkFileText = new StringBuilder();
                string sitepath = Settings.Default.SiteSubDirectory;

                mrkFileText.AppendLine($"=LDR  {GetNext5DigitRandomNumber()}nam  22{GetNext5DigitRandomNumber()}2a 4500");

                Log.Debug($"PublicationYearText: {product.PublicationYearText}, PublicationYear: {product.PublicationYear}, sku: {product.Sku}");


                string line008 = $"=008  {_currentDateTime:yyMMdd}s{product.PublicationYear:0000}\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\eng\\d ";

                Log.DebugFormat("line008: {0}", line008);
                Log.DebugFormat("line008.Length: {0}", line008.Length);

                mrkFileText.AppendLine($"=001  {product.Sku}");
                mrkFileText.AppendLine($"=005  {DateTime.Now:yyyyMMddhhmmss}.0");
                mrkFileText.AppendLine(line008);

                if (!string.IsNullOrWhiteSpace(product.Isbn10))
                {
                    mrkFileText.AppendLine($"=020  \\\\$a{product.Isbn10}");
                }

                if (!string.IsNullOrWhiteSpace(product.Isbn13))
                {
                    mrkFileText.AppendLine($"=020  \\\\$a{product.Isbn13}");
                }

                //var parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.OclcNumber).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}

                mrkFileText.AppendLine("=037  \\\\$bRittenhouse Book Distributors, Inc");

                //parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.NlmNumber).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}

                //parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.LcNumber).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}
                mrkFileText.AppendLine($"=100  1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}");
                mrkFileText.AppendLine($"=245  10$a{StripOffCarriageReturnAndLineFeed(product.Title)}");
                mrkFileText.AppendLine($"=260  \\\\$b{product.PublisherName},$c{publicationYearText}");
                mrkFileText.AppendLine($"=533  \\\\$a{product.Format}.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{publicationYearText}");
                mrkFileText.AppendLine($"=650  \\4$a{product.CategoryName}.");


                //parsedMarcFields = marcFieldsToAdd?.Where(x => x.Type == MarcFieldType.NlmSubject).ToList();
                //if (parsedMarcFields != null && parsedMarcFields.Any())
                //{
                //    foreach (var parsedMarcField in parsedMarcFields)
                //    {
                //        mrkFileText.AppendLine(parsedMarcField.Value);
                //    }
                //}
                mrkFileText.AppendLine($"=700  1\\$a{StripOffCarriageReturnAndLineFeed(product.Authors)}");
                mrkFileText.AppendLine($"=856  4\\$zConnect to this resource online$u{sitepath}Products/Book.aspx?sku={product.Isbn10}").AppendLine();

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

        public static string StripOffCarriageReturnAndLineFeed(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        public int GetNext5DigitRandomNumber()
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

        public void ClearWorkingDirectory(string workingDirectory)
        {
            string[] filesToDelete = Directory.GetFiles(workingDirectory);
            foreach (string file in filesToDelete)
            {
                if (file.EndsWith(".mrk") || file.EndsWith(".mrc") || file.EndsWith(".xml"))
                {
                    File.Delete(file);
                }
            }
        }

        public static void WriteCategories(StringBuilder mrkFileText, R2Resource resource)
        {
            if (resource.Categories != null || resource.SubCategories != null)
            {
                var categories = resource.Categories != null ? resource.Categories.ToList() : null;
                var subCategories = resource.SubCategories != null ? resource.SubCategories.ToList() : null;
                //If categories = subCategories
                if (categories != null && subCategories != null)
                {
                    if (categories.Count == subCategories.Count)
                    {
                        for (int i = 0; i < categories.Count; i++)
                        {
                            mrkFileText.AppendFormat("=650  {0}4$a{1}$x{2}", GetSpace(1), categories[i].Category, subCategories[i].SubCategory).AppendLine();
                        }

                    }
                    else if (categories.Count > subCategories.Count && subCategories.Count == 1)
                    {
                        if (subCategories.Count == 1)
                        {
                            foreach (var r2Category in categories)
                            {
                                mrkFileText.AppendFormat("=650  {0}4$a{1}$x{2}", GetSpace(1), r2Category.Category, subCategories[0].SubCategory).AppendLine();
                            }
                        }
                    }
                    else
                    {
                        foreach (var r2Category in categories)
                        {
                            foreach (var r2SubCategory in subCategories)
                            {
                                mrkFileText.AppendFormat("=650  {0}4$a{1}$x{2}", GetSpace(1), r2Category.Category,
                                    r2SubCategory.SubCategory).AppendLine();
                            }
                        }
                    }

                }
                else if (categories != null) //subCategories == null
                {
                    foreach (var r2Category in categories)
                    {
                        mrkFileText.AppendFormat("=650  {0}4$a{1}", GetSpace(1), r2Category.Category).AppendLine();
                    }
                }
                else if (subCategories != null)
                {
                    foreach (var r2SubCategories in subCategories)
                    {
                        mrkFileText.AppendFormat("=650  {0}4$a{1}", GetSpace(1), r2SubCategories.SubCategory).AppendLine();
                    }
                }
            }
        }

        public static void WriteAuthors(StringBuilder mrkFileText, R2Resource resource)
        {
            if (resource.AuthorList != null)
            {
                foreach (var r2Author in resource.AuthorList)
                {
                    mrkFileText.AppendFormat("=700  1{0}$a{1}", GetSpace(1), r2Author.ToDisplayName()).AppendLine();
                }
            }
            else
            {
                mrkFileText.AppendFormat("=700  1{0}$a{1}", GetSpace(1), resource.FirstAuthor).AppendLine();
            }
        }

        public static string GetSpace(int count)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append("\\");
            }
            return sb.ToString();
        }

        public static readonly string FieldsToRemove = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                              "=900\t=901\t=902\t=903\t=904\t=905\t=906\t=907\t=908\t=909",
                              "\t=910\t=911\t=912\t=913\t=914\t=915\t=916\t=917\t=918\t=919",
                              "\t=920\t=921\t=922\t=923\t=924\t=925\t=926\t=927\t=928\t=929",
                              "\t=930\t=931\t=932\t=933\t=934\t=935\t=936\t=937\t=938\t=939",
                              "\t=940\t=941\t=942\t=943\t=944\t=945\t=946\t=947\t=948\t=949",
                              "\t=950\t=951\t=952\t=953\t=954\t=955\t=956\t=957\t=958\t=959",
                              "\t=960\t=961\t=962\t=963\t=964\t=965\t=966\t=967\t=968\t=969",
                              "\t=970\t=971\t=972\t=973\t=974\t=975\t=976\t=977\t=978\t=979",
                              "\t=980\t=981\t=982\t=983\t=984\t=985\t=986\t=987\t=988\t=989",
                              "\t=990\t=991\t=992\t=993\t=994\t=995\t=996\t=997\t=998\t=999");

    }
}
