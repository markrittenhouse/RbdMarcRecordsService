
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

                .Append("SELECT {wmrOUTER.*} ")
                .Append("from WebR2LibraryMarcRecords wmrOUTER ")
                .Append("where wmrOUTER.dailyMarcRecordFileId in ( ")
                .Append("    select top (1) wmrINNER.dailyMarcRecordFileId ")
                .Append("    from WebR2LibraryMarcRecords wmrINNER ")
                .Append("    join MarcRecordProviderType mrpt on wmrINNER.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
                .Append("    where wmrINNER.isbn = wmrOUTER.isbn ")
                .Append("    order by mrpt.[priority] ")
                .Append(") ")



                //.Append("SELECT {wrmr.*} ")
                //.Append("FROM [dbo].[WebR2LibraryMarcRecords] as wrmr ")
//                .AppendFormat("where (isbn10 in ({0}) or isbn13 in ({0}) or isbn in ({0}) or eisbn in ({0})) ", itemsToFind)
                .AppendFormat("and (isbn10 in ({0}) or isbn13 in ({0}) or isbn in ({0}) or eisbn in ({0})) ", itemsToFind)

                .ToString();

            _log.DebugFormat("GetDailyMarcRecordFiles Sql Query : {0}", sql);

            ISession session = MvcApplication.CreateSession();

            IList digitalMarcRecordFiles = session.CreateSQLQuery(sql)
                .AddEntity("wmrOUTER", typeof(DigitalMarcRecordFile))
                .SetTimeout(300000)
                .List()
                ;

            return digitalMarcRecordFiles.Cast<DigitalMarcRecordFile>().ToList<DigitalMarcRecordFile>();
        }

        public List<DigitalMarcRecordFile> GetOclcEBookMarcRecords(List<string> items)
        {
            StringBuilder sbItemsToFind = new StringBuilder();
            foreach (string item in items)
            {
                sbItemsToFind.AppendFormat("'{0}',", item);
            }

            string itemsToFind = sbItemsToFind.ToString(0, sbItemsToFind.Length - 1);

            var sql = new StringBuilder()
                .Append("SELECT {wrmr.*} ")
                .Append("FROM [dbo].[OclcR2LibraryMarcRecords] as wrmr ")
                .AppendFormat("where (isbn10 in ({0}) or isbn13 in ({0}) or isbn in ({0}) or eisbn in ({0})) ", itemsToFind)
                .ToString();

            _log.DebugFormat("GetOclcEBookMarcRecords Sql Query : {0}", sql);

            ISession session = MvcApplication.CreateSession();

            IList digitalMarcRecordFiles = session.CreateSQLQuery(sql)
                .AddEntity("wrmr", typeof(DigitalMarcRecordFile))
                .SetTimeout(300000)
                .List()
                ;

            return digitalMarcRecordFiles.Cast<DigitalMarcRecordFile>().ToList<DigitalMarcRecordFile>();
        }

        public static bool IsDatabaseConnectionWorking()
        {
            var session = MvcApplication.CreateSession();

            var test = (from x in session.Query<MarcRecordFile>() select x).FirstOrDefault();
            return test != null;
        }
          
    }
}