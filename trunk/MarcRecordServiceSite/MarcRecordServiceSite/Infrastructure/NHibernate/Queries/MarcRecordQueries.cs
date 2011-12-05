using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using NHibernate;
using NHibernate.Transform;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Queries
{
	public class MarcRecordQueries
	{
		public static IEnumerable<MarcRecord> GetMarcRecords(string isbn13)
		{
			var session = MvcApplication.CreateSession();

			var records = session.QueryOver<MarcRecord>()
				.Where(x => x.Isbn13 == isbn13)
				.Fetch(x => x.Providers)
				.Eager
				.UnderlyingCriteria
				.SetFetchMode("MarcRecord.Providers", FetchMode.Eager)
				.SetResultTransformer(Transformers.DistinctRootEntity)
				//.Where(x => x.Ready == 1)
				//.WhereRestrictionOn(x => x.StatusId)
				//.IsIn(new List<int> { 6, 7 })
				//.Fetch(x => x.InstitutionResources)
				//.Eager
				////.JoinQueryOver<InstitutionResource2>(x => x.InstitutionResources)
				////.Where(y => y.RecordStatus == 1)
				////.Where(y => y.InstitutionId == institutionId)
				//.UnderlyingCriteria
				//.SetFetchMode("Resource2.InstitutionResource2", FetchMode.Eager)
				//.SetResultTransformer(Transformers.DistinctRootEntity)
				//.SetFetchMode("Resource2.FileDocIds", FetchMode.Eager)
				//.SetResultTransformer(Transformers.DistinctRootEntity)
				.Future<MarcRecord>()
				.OrderBy(x => x.Id);

			return records;
		}


		public static IEnumerable<MarcRecord> GetMarcRecords2(string isbn13)
		{
			var session = MvcApplication.CreateSession();

			var records = session.QueryOver<MarcRecord>()
				.Where(x => x.Isbn13 == isbn13)
				.Fetch(x => x.Providers)
				.Eager
				.UnderlyingCriteria
				.SetFetchMode("MarcRecord.Providers", FetchMode.Eager)
				.SetResultTransformer(Transformers.DistinctRootEntity)
				.SetFetchMode("Providers.Files", FetchMode.Eager)
				.SetResultTransformer(Transformers.DistinctRootEntity)
				//.Where(x => x.Ready == 1)
				//.WhereRestrictionOn(x => x.StatusId)
				//.IsIn(new List<int> { 6, 7 })
				//.Fetch(x => x.InstitutionResources)
				//.Eager
				////.JoinQueryOver<InstitutionResource2>(x => x.InstitutionResources)
				////.Where(y => y.RecordStatus == 1)
				////.Where(y => y.InstitutionId == institutionId)
				//.UnderlyingCriteria
				//.SetFetchMode("Resource2.InstitutionResource2", FetchMode.Eager)
				//.SetResultTransformer(Transformers.DistinctRootEntity)
				//.SetFetchMode("Resource2.FileDocIds", FetchMode.Eager)
				//.SetResultTransformer(Transformers.DistinctRootEntity)
				.Future<MarcRecord>()
				.OrderBy(x => x.Id);

			return records;
		}

	}
}