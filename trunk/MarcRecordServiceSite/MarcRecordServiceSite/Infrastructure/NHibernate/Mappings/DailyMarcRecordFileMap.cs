using FluentNHibernate.Mapping;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Mappings
{
    public sealed class DailyMarcRecordFileMap : ClassMap<DailyMarcRecordFile>
	{
        public DailyMarcRecordFileMap()
		{
            Table("DailyMarcRecordFile");

            // dailyMarcRecordFileId, isbn10, isbn13, sku, fileData
            Id(x => x.Id).Column("dailyMarcRecordFileId");
            Map(x => x.Isbn10).Column("isbn10");
            Map(x => x.Isbn13).Column("isbn13");
            Map(x => x.Sku).Column("sku");
            Map(x => x.MarcFile).Column("fileData");
		}
	}
}