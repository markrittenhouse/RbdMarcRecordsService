using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Mappings
{
	public sealed class MarcRecordProviderMap : ClassMap<MarcRecordProvider>
	{
		public MarcRecordProviderMap()
		{
			Table("MarcRecordProvider");

			//mrp.marcRecordProviderId, mrp.marcRecordId, mrp.marcRecordProviderTypeId, mrp.encodingLevel, mrp.dateCreated, mrp.dateUpdated

			Id(x => x.Id).Column("marcRecordProviderId");
			Map(x => x.MarcRecordProviderTypeId).Column("marcRecordProviderTypeId");
			Map(x => x.EncodingLevel).Column("encodingLevel");
			Map(x => x.DateUpdated).Column("dateUpdated");

			References<MarcRecord>(x => x.MarcRecord).Column("marcRecordId");
			HasMany<MarcRecordFile>(x => x.Files).KeyColumn("marcRecordProviderId").AsBag().Cascade.All().Not.LazyLoad();

		}
	}
}