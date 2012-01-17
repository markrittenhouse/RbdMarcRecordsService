using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Mappings
{
	public sealed class MarcRecordFileMap : ClassMap<MarcRecordFile>
	{
		public MarcRecordFileMap()
		{
			Table("MarcRecordFile");

			// mrf.marcRecordFileId, mrf.marcRecordProviderId, mrf.marcRecordFileTypeId, mrf.fileData
			Id(x => x.Id).Column("marcRecordFileId");
			Map(x => x.MarcRecordFileTypeId).Column("marcRecordFileTypeId");
			Map(x => x.FileData).Column("fileData");
			References(x => x.Provider).Column("marcRecordProviderId");
		}
	}
}