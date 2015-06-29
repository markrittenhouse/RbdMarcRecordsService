using System;

namespace MarcRecordServiceSite.Models
{
    public class PingData
    {
        public string DatabaseStatus { get; set; }
        public string ClientIpAddress { get; set; }
        public string Version { get; private set; }
        public string MachineName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PingData()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Version = version.ToString();
        }
    }
}