using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Rittenhouse.RBD.Core.Utilities;
using log4net;

namespace MarcRecordServiceApp.Core.Utilities
{
    public class EmailMessageData
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        public string MessageBody { get; set;}
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public string[] ToRecipients { get; set; }
        public string[] CcRecipients { get; set; }
        public string[] BccRecipients { get; set; }
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public string ReplyToAddress { get; set; }
        public string ReplyToDisplayName { get; set; }
        public List<EmailTextAttachment> TextAttachments { get; set; }
        public DateTime QueueDate { get; set; }
        public int SendAttempts { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmailMessageData()
        {
            TextAttachments = new List<EmailTextAttachment>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private MailMessage GetMailMessage()
        {
            MailMessage message = new MailMessage();

            // To addresses
            if (null != ToRecipients)
            {
                foreach (string address in ToRecipients)
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        string cleanedAddress = address.Replace(",", ".").Trim().Replace(" ", "");  // Added to handle invalid email address with comma instead of periods.  (The IU was supposed to fix this!) SJS-4/26/2011
                        Log.DebugFormat("Adding TO address: <{0}>", cleanedAddress); 
                        message.To.Add(cleanedAddress);
                    }
                }
            }

            // CC addresses
            if (null != CcRecipients)
            {
                foreach (string address in CcRecipients)
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        Log.DebugFormat("Adding CC address: <{0}>", address);
                        message.CC.Add(address);
                    }
                }
            }

            // bcc address
            if (null != BccRecipients)
            {
                foreach (string address in BccRecipients)
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        Log.DebugFormat("Adding BCC address: <{0}>", address);
                        message.Bcc.Add(address);
                    }
                }
            }

            // attachments
            if (null != TextAttachments)
            {
                foreach (EmailTextAttachment textAttachment in TextAttachments)
                {
                    Attachment attachment = Attachment.CreateAttachmentFromString(textAttachment.FileText, textAttachment.FileName, Encoding.ASCII, textAttachment.MediaType);
                    Log.DebugFormat("Adding attachment text: <{0}>", TextAttachments);
                    message.Attachments.Add(attachment);
                }
            }

            // from address
            if (!string.IsNullOrEmpty(FromAddress))
            {
                Log.DebugFormat("Adding FROM address: <{0}>, '{1}'", FromAddress, FromDisplayName);
                message.From = (string.IsNullOrEmpty(FromDisplayName)) ? new MailAddress(FromAddress) : new MailAddress(FromAddress, FromDisplayName);
            }
            else
            {
				message.From = new MailAddress(Settings.Default.DefaultFromAddress, Settings.Default.DefaultFromAddressName);
            }
            
            // reply-to address
            if (!string.IsNullOrEmpty(ReplyToAddress))
            {
                Log.DebugFormat("Adding REPLY-TO address: <{0}>, '{1}'", ReplyToAddress, ReplyToDisplayName);
                message.ReplyTo = (string.IsNullOrEmpty(ReplyToDisplayName)) ? new MailAddress(ReplyToAddress) : new MailAddress(ReplyToAddress, ReplyToDisplayName);
            }

            message.Subject = Subject;
            message.Body = MessageBody;
            message.IsBodyHtml = IsBodyHtml;

            return message;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Send()
        {
            try
            {
                Log.Debug(ToString());
                MailMessage message = GetMailMessage();

                SmtpClient client = new SmtpClient();

                SendAttempts++;

                Log.DebugFormat("Host: {0}, SendAttempts: {1}", client.Host, SendAttempts);

                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("EmailMessageDate = [")
                .AppendFormat("Subject: {0}", Subject).AppendLine()
                .AppendFormat(", IsBodyHtml: {0}", IsBodyHtml)
                .AppendFormat(", QueueDate: {0}", QueueDate)
                .AppendFormat(", SendAttempts: {0}", SendAttempts).AppendLine()
                .AppendFormat(", FromAddress: <{0}>", FromAddress)
                .AppendFormat(", FromDisplayName: {0}", FromDisplayName)
                .AppendFormat(", ReplyToAddress: <{0}>", ReplyToAddress)
                .AppendFormat(", ReplyToDisplayName: {0}", ReplyToDisplayName).AppendLine();

            if (ToRecipients == null)
            {
                sb.Append(", ToRecipients[]: null").AppendLine();
            }
            else
            {
                sb.AppendFormat(", ToRecipients[{0}]: <{1}>", ToRecipients.Length, string.Join(",", ToRecipients)).AppendLine();
            }

            if (CcRecipients == null)
            {
                sb.Append(", CcRecipients[]: null").AppendLine();
            }
            else
            {
                sb.AppendFormat(", CcRecipients[{0}]: <{1}>", CcRecipients.Length, string.Join(",", CcRecipients)).AppendLine();
            }

            if (BccRecipients == null)
            {
                sb.Append(", BccRecipients[]: null").AppendLine();
            }
            else
            {
                sb.AppendFormat(", BccRecipients[{0}]: <{1}>", BccRecipients.Length, string.Join(",", BccRecipients)).AppendLine();
            }

            if ((!string.IsNullOrEmpty(MessageBody)) && (MessageBody.Length > 500))
            {
                sb.AppendFormat(", MessageBody: {0} ... TRUNCATED!", MessageBody.Substring(0, 500)).AppendLine();
            }
            else
            {
                sb.AppendFormat(", MessageBody: {0}", MessageBody).AppendLine();
            }

            sb.AppendFormat(", TextAttachments[{0}]:", TextAttachments.Count).AppendLine();

            foreach (EmailTextAttachment attachment in TextAttachments)
            {
                sb.AppendFormat("\tFileName: {0}", attachment.FileName).AppendLine();
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
