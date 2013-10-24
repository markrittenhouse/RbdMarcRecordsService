using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Models
{
    [Serializable]
    public class JsonMarcRecordRequest
    {
        public string Format { get; set; }
        public string AccountNumber { get; set; }
        public List<JsonIsbnAndCustomerField> IsbnAndCustomerFields { get; set; }
        public bool IsDeleteFile { get; set; }
        public MarcFtpCredientials FtpCredientials { get; set; }
        
        //TODO: Implement this in Rittenhouse.com to minimize how much redundant information we are sending.
        //TODO: This can be used for all custom fields that are written to EVERY record. 
        public List<JsonCustomMarcField> CustomMarcFields { get; set; }

        public bool IsR2Request { get; set; }
        public bool IsRittenhouseRequest { get; set; }
    }

    [Serializable]
    public class JsonIsbnAndCustomerField
    {
        public string IsbnOrSku { get; set; }
        public List<JsonCustomMarcField> CustomMarcFields { get; set; }
    }

    [Serializable]
    public class JsonCustomMarcField
    {
        public int FieldNumber { get; set; }
        public string FieldIndicator1 { get; set; }
        public string FieldIndicator2 { get; set; }
        public string FieldValue { get; set; }
        public List<JsonCustomMarcSubfield> MarcSubfields { get; set; }
    }

    [Serializable]
    public class JsonCustomMarcSubfield
    {
        public string Subfield { get; set; }
        public string SubfieldValue { get; set; }
    }

    [Serializable]
    public class MarcFtpCredientials
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}