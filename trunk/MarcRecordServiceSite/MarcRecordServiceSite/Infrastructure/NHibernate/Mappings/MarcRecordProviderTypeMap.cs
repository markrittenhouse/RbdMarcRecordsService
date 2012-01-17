using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Mappings
{
    public sealed class MarcRecordProviderTypeMap : ClassMap<MarcRecordProviderType>
    {
        public MarcRecordProviderTypeMap()
		{
			Table("MarcRecordProviderType");

            Id(x => x.Id).Column("marcRecordProviderTypeId");
            Map(x => x.Priority).Column("priority");
            
		}
    }
}