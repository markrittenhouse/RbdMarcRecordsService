using FluentNHibernate.Mapping;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Mappings
{
    public sealed class PrintMarcRecordFileMap : ClassMap<PrintMarcRecordFile>
    {
        public PrintMarcRecordFileMap()
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

    public sealed class DigitalMarcRecordFileMap : ClassMap<DigitalMarcRecordFile>
    {
        public DigitalMarcRecordFileMap()
        {
            Table("WebR2LibraryMarcRecords");

            // dailyMarcRecordFileId, isbn10, isbn13, sku, fileData
            Id(x => x.Id).Column("dailyMarcRecordFileId");
            Map(x => x.Isbn).Column("isbn");
            Map(x => x.Isbn10).Column("isbn10");
            Map(x => x.Isbn13).Column("isbn13");
            Map(x => x.EIsbn).Column("eisbn");
            Map(x => x.MarcFile).Column("fileData");
            Map(x => x.ProviderTypeId).Column("marcRecordProviderTypeId");
            Map(x => x.CreatedDate).Column("createdDate");
        }
    }
}