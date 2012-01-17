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
}