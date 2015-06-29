using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Entities
{
    public class PrintMarcRecordFile
    {
        public virtual int Id { get; set; }
        public virtual string Isbn10 { get; set; }
        public virtual string Isbn13 { get; set; }
        public virtual string Sku { get; set; }
        public virtual string MarcFile { get; set; }
    }

    public class DigitalMarcRecordFile
    {
        public virtual int Id { get; set; }
        public virtual string Isbn { get; set; }
        public virtual string Isbn10 { get; set; }
        public virtual string Isbn13 { get; set; }
        public virtual string EIsbn { get; set; }
        public virtual string MarcFile { get; set; }
        public virtual int ProviderTypeId { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}