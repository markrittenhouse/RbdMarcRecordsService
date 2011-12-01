using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Mappings
{
	public sealed class MarcRecordMap : ClassMap<MarcRecord>
	{
		public MarcRecordMap()
		{
			Table("MarcRecord");

			Id(x => x.Id).Column("marcRecordId");
			Map(x => x.Isbn10).Column("isbn10");
			Map(x => x.Isbn13).Column("isbn13");
			Map(x => x.Sku).Column("sku");

			HasMany<MarcRecordProvider>(x => x.Providers).KeyColumn("marcRecordId").AsBag().Cascade.All().Not.LazyLoad();

		}
	}
}