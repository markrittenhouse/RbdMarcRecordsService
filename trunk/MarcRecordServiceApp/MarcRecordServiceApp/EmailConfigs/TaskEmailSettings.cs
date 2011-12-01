using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net;

namespace MarcRecordServiceApp.EmailConfigs
{
    public class TaskEmailSettings
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        public string TaskKey { get; set; }
        public EmailConfiguration SuccessEmailConfig { get; private set; }
        public EmailConfiguration ErrorEmailConfig { get; private set; }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskKey"></param>
        public TaskEmailSettings(string taskKey)
        {
            TaskKey = taskKey;

            SuccessEmailConfig = new EmailConfiguration { Type = "Success" };
            ErrorEmailConfig = new EmailConfiguration { Type = "Error" };

            PopulateEmailConfigurations("Default", SuccessEmailConfig);
            PopulateEmailConfigurations("Default", ErrorEmailConfig);
            PopulateEmailConfigurations(taskKey, SuccessEmailConfig);
            PopulateEmailConfigurations(taskKey, ErrorEmailConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskKey"></param>
        /// <param name="emailConfiguration"></param>
        private static void PopulateEmailConfigurations(string taskKey, EmailConfiguration emailConfiguration)
        {
            Log.DebugFormat("EmailConfigDirectory: {0}", Settings.Default.EmailConfigDirectory);
            string xmlFilename = string.Format(@"{0}\{1}.xml", Settings.Default.EmailConfigDirectory, taskKey);
            Log.DebugFormat("xmlFilename: {0}", xmlFilename);

            if (File.Exists(xmlFilename))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilename);

                if (null != xmlDoc.DocumentElement)
                {
                    XmlNodeList xmlNodes = xmlDoc.DocumentElement.SelectNodes("/RittenhouseWebLoader/EmailConfigurations/EmailConfiguration");
                    if (xmlNodes != null)
                    {
                        foreach (XmlNode xmlNode in xmlNodes)
                        {
                            Log.DebugFormat("xmlNode: {0}", xmlNode.Name);
                            if (xmlNode.Attributes == null)
                            {
                                throw new Exception(string.Format("Invalid Email Config XML in {0}", xmlFilename));
                            }
                            XmlAttribute type = xmlNode.Attributes["type"];
                            XmlAttribute send = xmlNode.Attributes["send"];
                            if (type.Value == emailConfiguration.Type)
                            {
								PopulateEmailAddresses(emailConfiguration, xmlNode);
                                emailConfiguration.Send = (send.Value.ToLower() == "true");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailConfiguration"></param>
        /// <param name="emailConfigNode"></param>
        private static void PopulateEmailAddresses(EmailConfiguration emailConfiguration, XmlNode emailConfigNode)
        {
			List<string> toEmailAddresses = new List<string>();
			List<string> ccEmailAddresses = new List<string>();
			List<string> bccEmailAddresses = new List<string>();

            //XmlNodeList emailAddressNodes = emailConfigNode.SelectNodes("/RittenhouseWebLoader/EmailConfigurations/EmailConfiguration/ToAddresses/EmailAddress");
            //XmlNodeList emailAddressNodes = emailConfigNode.SelectNodes("/RittenhouseWebLoader/EmailConfigurations/EmailConfiguration/ToAddresses/EmailAddress");

            XmlNodeList childNodes = emailConfigNode.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                if (childNode.Name == "ToAddresses")
                {
                    XmlNodeList emailAddressNodes = childNode.ChildNodes;

                    foreach (XmlNode emailAddressNode in emailAddressNodes)
                    {
                        if (emailAddressNode.Name == "EmailAddress")
                        {
                            Log.DebugFormat("emailAddressNode: {0}", emailAddressNode.InnerText);
                            toEmailAddresses.Add(emailAddressNode.InnerText);
                        }
                    }
                }

				if (childNode.Name == "CcAddresses")
				{
					XmlNodeList emailAddressNodes = childNode.ChildNodes;

					foreach (XmlNode emailAddressNode in emailAddressNodes)
					{
						if (emailAddressNode.Name == "EmailAddress")
						{
							Log.DebugFormat("emailAddressNode: {0}", emailAddressNode.InnerText);
							ccEmailAddresses.Add(emailAddressNode.InnerText);
						}
					}
				}

				if (childNode.Name == "BccAddresses")
				{
					XmlNodeList emailAddressNodes = childNode.ChildNodes;

					foreach (XmlNode emailAddressNode in emailAddressNodes)
					{
						if (emailAddressNode.Name == "EmailAddress")
						{
							Log.DebugFormat("emailAddressNode: {0}", emailAddressNode.InnerText);
							bccEmailAddresses.Add(emailAddressNode.InnerText);
						}
					}
				}
			}

			emailConfiguration.ToAddresses.Clear();
			if (toEmailAddresses.Count > 0)
            {
                emailConfiguration.ToAddresses.AddRange(toEmailAddresses);
            }

			emailConfiguration.CcAddresses.Clear();
			if (ccEmailAddresses.Count > 0)
			{
				emailConfiguration.CcAddresses.AddRange(ccEmailAddresses);
			}

			emailConfiguration.BccAddresses.Clear();
			if (bccEmailAddresses.Count > 0)
			{
				emailConfiguration.BccAddresses.AddRange(bccEmailAddresses);
			}
		}
    }
}
