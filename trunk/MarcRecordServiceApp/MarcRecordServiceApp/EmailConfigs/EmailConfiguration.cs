using System.Collections.Generic;

namespace MarcRecordServiceApp.EmailConfigs
{
    public class EmailConfiguration
    {
        public bool Send { get; set; }
        public string Type { get; set; }
		public List<string> ToAddresses { get; private set; }
		public List<string> CcAddresses { get; private set; }
		public List<string> BccAddresses { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EmailConfiguration()
        {
            // Send = true;
			// ToAddresses = new List<string> { "scottscheider@technotects.com", "KenHaberle@technotects.com" };
			// CcAddresses = new List<string> { "rittenhouse@lane4solutions.com" };
			// BccAddresses = new List<string> { "scott@lane4solutions.com" };
			
			Send = true;
			ToAddresses = new List<string> { "KenHaberle@technotects.com" };
			CcAddresses = new List<string> { "KenHaberle@technotects.com" };
			BccAddresses = new List<string> { "KenHaberle@technotects.com" };
		}
    }
}
