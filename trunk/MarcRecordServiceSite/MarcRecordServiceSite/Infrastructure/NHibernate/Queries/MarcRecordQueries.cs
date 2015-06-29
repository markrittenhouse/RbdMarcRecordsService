
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using log4net;

namespace MarcRecordServiceSite.Infrastructure.NHibernate.Queries
{
    public class MarcRecordQueries
    {

        private readonly ILog _log;

        public MarcRecordQueries(ILog log)
        {
            _log = log;
        }
        //private readonly IQueryable<DailyMarcRecordFile> _dailyMarcRecordFiles;

        //public MarcRecordQueries(IQueryable<DailyMarcRecordFile> dailyMarcRecordFiles)
        //{
        //    _dailyMarcRecordFiles = dailyMarcRecordFiles;
        //}

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

        public static MarcRecordFile GetMnemonicMarcFileForEditing2(string value)
        {
            var session = MvcApplication.CreateSession();

            // Works the way I want it. Returns 1 result with 1 query
            var result = (from x in session.Query<MarcRecordFile>()
                          where (x.Provider.MarcRecord.Isbn13 == value || x.Provider.MarcRecord.Isbn10 == value || x.Provider.MarcRecord.Sku == value)
                          where x.MarcRecordFileTypeId == 2
                          orderby x.Provider.ProviderType.Priority
                          select x).FirstOrDefault();

            return result;
        }

        public static List<MarcRecordFile> GetMnemonicMarcFilesForEditing2(List<string> items)
        {
            List<MarcRecordFile> marcRecordFiles2 = new List<MarcRecordFile>();
            var marcRecordFiles = items.Select(GetMnemonicMarcFileForEditing2).Where(x => x != null);

            if (marcRecordFiles.Any())
            {
                marcRecordFiles2 = marcRecordFiles.ToList();
            }

            return marcRecordFiles2;
        }

        public List<PrintMarcRecordFile> GetDailyMarcRecordFiles(List<string> items, bool isR2Request, bool isRittenhouseRequest)
        {
            StringBuilder sbItemsToFind = new StringBuilder();
            foreach (string item in items)
            {
                sbItemsToFind.AppendFormat("'{0}',", item);
            }

            string itemsToFind = sbItemsToFind.ToString(0, sbItemsToFind.Length - 1);

            StringBuilder sb = new StringBuilder()
                .Append("SELECT {dmrf.*} ")
                .Append("FROM [dbo].[DailyMarcRecordFile] as dmrf ");

            if (isR2Request)
            {
                sb.AppendFormat("where (isbn13 in ({0})) ", itemsToFind);
            }
            else if (isRittenhouseRequest)
            {
                sb.AppendFormat("where (sku in ({0})) ", itemsToFind);
            }
            else
            {
                sb.AppendFormat("where (isbn10 in ({0}) or isbn13 in ({0}) or sku in ({0})) ", itemsToFind);
            }

            var sql = sb.Append("and Len(isbn10) = 10 ")
                .ToString();

            _log.DebugFormat("GetDailyMarcRecordFiles Sql Query : {0}", sql);

            ISession session = MvcApplication.CreateSession();

            IList dailyMarcRecordFilesList = session.CreateSQLQuery(sql)
                .AddEntity("dmrf", typeof(PrintMarcRecordFile))
                .SetTimeout(300000)
                .List()
                ;

            var dailyMarcRecordFiles = dailyMarcRecordFilesList.Cast<PrintMarcRecordFile>().ToList<PrintMarcRecordFile>();

            return dailyMarcRecordFiles;
        }

        public List<DigitalMarcRecordFile> GetEBookMarcRecords(List<string> items)
        {
            StringBuilder sbItemsToFind = new StringBuilder();
            foreach (string item in items)
            {
                sbItemsToFind.AppendFormat("'{0}',", item);
            }

            string itemsToFind = sbItemsToFind.ToString(0, sbItemsToFind.Length - 1);

            var sql = new StringBuilder()
                .Append("SELECT {wrmr.*} ")
                .Append("FROM [dbo].[WebR2LibraryMarcRecords] as wrmr ")
                .AppendFormat("where (isbn10 in ({0}) or isbn13 in ({0}) or isbn in ({0}) or eisbn in ({0})) ", itemsToFind)
                .ToString();

            _log.DebugFormat("GetDailyMarcRecordFiles Sql Query : {0}", sql);

            ISession session = MvcApplication.CreateSession();

            IList digitalMarcRecordFiles = session.CreateSQLQuery(sql)
                .AddEntity("wrmr", typeof(DigitalMarcRecordFile))
                .SetTimeout(300000)
                .List()
                ;

            return digitalMarcRecordFiles.Cast<DigitalMarcRecordFile>().ToList<DigitalMarcRecordFile>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public IEnumerable<MarcRecordFile> GetMnemonicMarcFilesForEditing(List<string> items)
        {
            ISession session = MvcApplication.CreateSession();

            StringBuilder test123 = new StringBuilder();
            foreach (string item in items)
            {
                test123.AppendFormat("'{0}',", item);
            }

            string test1234 = test123.ToString(0, test123.Length - 1);

            string sql = new StringBuilder()
                .Append("SELECT     {mrf.*}, {mr.*}, {mrp.*}, {mrpt.*} ")
                .Append("FROM         MarcRecordFile mrf ")
                .Append("left outer JOIN MarcRecordProvider mrp ON mrf.marcRecordProviderId = mrp.marcRecordProviderId ")
                .Append("left outer JOIN MarcRecord mr ON mrp.marcRecordId = mr.marcRecordId ")
                .Append("left outer JOIN MarcRecordProviderType mrpt ON mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
                .AppendFormat("where mrf.marcRecordFileTypeId = 2 and (mr.isbn10 in ({0}) or mr.isbn13 in ({0}) or mr.sku in ({0})) ", test1234)
                .Append("order by mr.isbn13, mrpt.priority asc ")
                .ToString();

            _log.DebugFormat("GetMnemonicMarcFilesForEditing Sql Query : {0}", sql);

            IList asdasd = session.CreateSQLQuery(sql)
                .AddEntity("mrf", typeof (MarcRecordFile))
                .AddJoin("mrp", "mrf.Provider")
                .AddJoin("mr", "mrp.MarcRecord")
                .AddJoin("mrpt", "mrp.ProviderType")
                .SetTimeout(300000)
                .List()
                ;

            var tesatsadasdf = asdasd.Cast<MarcRecordFile>().AsQueryable<MarcRecordFile>();

            return tesatsadasdf;


        }


            //    // Works the way I want it. Returns too many results
            //    IOrderedQueryable<MarcRecordFile> result = (from x in session.Query<MarcRecordFile>()
            //                  where items.Contains(x.Provider.MarcRecord.Isbn13) || items.Contains(x.Provider.MarcRecord.Isbn10) || items.Contains(x.Provider.MarcRecord.Sku)
            //                  where x.MarcRecordFileTypeId == 2
            //                  orderby x.Provider.MarcRecord.Isbn13, x.Provider.ProviderType.Priority ascending
            //                  select x);
            //    return result;
            //}

          
    }
}