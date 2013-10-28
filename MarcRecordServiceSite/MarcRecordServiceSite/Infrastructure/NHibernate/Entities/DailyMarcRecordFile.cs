using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Entities
{
    public class DailyMarcRecordFile
    {
        public virtual int Id { get; set; }
        public virtual string Isbn10 { get; set; }
        public virtual string Isbn13 { get; set; }
        public virtual string Sku { get; set; }
        public virtual string MarcFile { get; set; }
    }
}