using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using log4net;

namespace MarcRecordServiceSite.Models
{
    public class FtpService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MarcFtpCredientials FtpCredientials { get; set; }

        public bool IsEligibleForFtp { get; set; }

        public FtpService(MarcFtpCredientials ftpCredientials)
        {
            FtpCredientials = ftpCredientials;

            IsEligibleForFtp = !string.IsNullOrWhiteSpace(ftpCredientials.Host) 
                                && !string.IsNullOrWhiteSpace(ftpCredientials.UserName) 
                                && !string.IsNullOrWhiteSpace(ftpCredientials.Password);
        }

        public bool UploadFileToFtp(string source)
        {
            try
            {                
                //FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(FtpCredientials.Host);
                //ftp.Credentials = new NetworkCredential(FtpCredientials.UserName, FtpCredientials.Password);

                //ftp.KeepAlive = true;
                //ftp.UseBinary = true;
                //ftp.Method = WebRequestMethods.Ftp.UploadFile;

                //FileStream fs = File.OpenRead(source);
                //byte[] buffer = new byte[fs.Length];
                //fs.Read(buffer, 0, buffer.Length);
                //fs.Close();

                //Stream ftpstream = ftp.GetRequestStream();
                //ftpstream.Write(buffer, 0, buffer.Length);
                //ftpstream.Close();
                FileInfo fi = new FileInfo(source);

                FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(FtpCredientials.Host + "/" +  fi.Name);
                ftpClient.Credentials = new NetworkCredential(FtpCredientials.UserName, FtpCredientials.Password);
                ftpClient.Method = WebRequestMethods.Ftp.UploadFile;
                ftpClient.UseBinary = true;
                ftpClient.KeepAlive = true;
                
                ftpClient.ContentLength = fi.Length;
                byte[] buffer = new byte[4097];
                int bytes = 0;
                int total_bytes = (int)fi.Length;
                FileStream fs = fi.OpenRead();
                Stream rs = ftpClient.GetRequestStream();
                while (total_bytes > 0)
                {
                    bytes = fs.Read(buffer, 0, buffer.Length);
                    rs.Write(buffer, 0, bytes);
                    total_bytes = total_bytes - bytes;
                }
                //fs.Flush();
                fs.Close();
                rs.Close();
                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                var value = uploadResponse.StatusDescription;
                uploadResponse.Close();


                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error with FtpService: {0}", ex.Message);
                
                return false;
            }
        }
    }
}