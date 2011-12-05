using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Entities
{
	public class MarcRecordFile
	{
		public virtual int Id { get; set; }
		public virtual int MarcRecordFileTypeId { get; set; }
		public virtual string FileData { get; set; }

		public virtual MarcRecordProvider Provider { get; set; }
	}
}