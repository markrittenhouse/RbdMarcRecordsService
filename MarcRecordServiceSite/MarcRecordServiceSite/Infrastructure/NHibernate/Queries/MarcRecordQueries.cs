using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using MarcRecordServiceSite.Models;
using NHibernate;
using NHibernate.Transform;
using MarcRecordServiceSite.Infrastructure.NHibernate.Extensions;
using NHibernate.Linq;

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

        /// <summary>
        /// Works for Single Marc Record. 
        /// </summary>
        /// <param name="isbn13"></param>
        /// <returns></returns>
        public static MarcRecordFile GetMnemonicMarcFileForEditing(string isbn13)
        {
            var session = MvcApplication.CreateSession();

            // Works the way I want it. Returns 1 result with 1 query
            var result = (from x in session.Query<MarcRecordFile>()
                          where x.Provider.MarcRecord.Isbn13 == isbn13
                          where x.MarcRecordFileTypeId == 2
                          orderby x.Provider.ProviderType.Priority
                          select x).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<MarcRecordFile> GetMnemonicMarcFilesForEditing(MarcRecordRequestItems items)
        {
            var session = MvcApplication.CreateSession();

            // Works the way I want it. Returns too many results
            var result = (from x in session.Query<MarcRecordFile>()
                          where items.Items.Contains(x.Provider.MarcRecord.Isbn13) || items.Items.Contains(x.Provider.MarcRecord.Isbn10) || items.Items.Contains(x.Provider.MarcRecord.Sku)
                          where x.MarcRecordFileTypeId == 2
                          orderby x.Provider.MarcRecord.Isbn13, x.Provider.ProviderType.Priority ascending
                          select x);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<MarcRecordFile> GetMnemonicMarcFilesForEditing(List<string> items)
        {
            var session = MvcApplication.CreateSession();

            // Works the way I want it. Returns too many results
            var result = (from x in session.Query<MarcRecordFile>()
                          where items.Contains(x.Provider.MarcRecord.Isbn13) || items.Contains(x.Provider.MarcRecord.Isbn10) || items.Contains(x.Provider.MarcRecord.Sku)
                          where x.MarcRecordFileTypeId == 2
                          orderby x.Provider.MarcRecord.Isbn13, x.Provider.ProviderType.Priority ascending
                          select x);
            return result;
        }

	}
}