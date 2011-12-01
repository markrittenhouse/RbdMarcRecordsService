using System;

namespace Rittenhouse.RBD.Core.Utilities
{
    [Serializable] 
    public class EmailTextAttachment
    {
        public string FileText { get; set; }
        public string FileName { get; set; }
        //public Encoding FileEncoding { get; set; }
        public string MediaType { get; set; }
    }
}
