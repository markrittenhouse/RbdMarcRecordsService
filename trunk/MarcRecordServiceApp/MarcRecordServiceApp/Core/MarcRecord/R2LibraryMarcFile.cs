using System;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class R2LibraryMarcFile
    {
        public string Isbn { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string EIsbn { get; set; }

        public string MrkText { get; set; }

        public DateTime CreatedDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public DateTime UpdatedDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public int ProviderSourceId { get; set; }

    }
}
