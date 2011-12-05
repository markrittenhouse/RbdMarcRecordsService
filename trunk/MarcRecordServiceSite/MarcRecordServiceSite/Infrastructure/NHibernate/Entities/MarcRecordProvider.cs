using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Entities
{
	public class MarcRecordProvider
	{
		public virtual int Id { get; set; }
		public virtual int MarcRecordProviderTypeId { get; set; }		
		public virtual string EncodingLevel { get; set; }
		public virtual DateTime DateUpdated { get; set; }

		public virtual MarcRecord MarcRecord { get; set; }
		public virtual IList<MarcRecordFile> Files { get; set; }		
	}
}