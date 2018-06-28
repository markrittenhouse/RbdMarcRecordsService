using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class DailyMarcRecordFactory : FactoryBase
    {


        private static string InsertDailyMarcRecordFiles(int providerId)
        {
            return new StringBuilder()
                .Append("Insert Into DailyMarcRecordFile ")
                .Append("select mr.isbn10, mr.isbn13, mr.sku, mrp.marcRecordProviderTypeId, mrf.fileData ")
                .Append("from MarcRecord mr ")
                .AppendFormat("join MarcRecordProvider mrp on mr.marcRecordId = mrp.marcRecordId and mrp.marcRecordProviderTypeId = {0} ", providerId)
                .Append("join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
                .Append("left join ExcludedMarcRecord emr on mr.sku = emr.sku and mrpt.marcRecordProviderTypeId = emr.marcRecordProviderTypeId ")
                .Append("join MarcRecordFile mrf on mrp.marcRecordProviderId = mrf.marcRecordProviderId and marcRecordFileTypeId = 2 ")
                .Append("left join DailyMarcRecordFile dmrf on mr.isbn10 = dmrf.isbn10 and mr.isbn13 = dmrf.isbn13 and mr.sku = dmrf.sku ")
                .Append("where dmrf.dailyMarcRecordFileId is null and mr.sku not like '%R2P%' and emr.excludedMarcRecordId is null")
                .ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int TruncateDailyMarcRecords()
        {
            int rowCount = ExecuteTruncateTable("DailyMarcRecordFile", Settings.Default.RittenhouseMarcDb);
            return rowCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int InsertDailyNlmMarcRecords()
        {
            var sql = InsertDailyMarcRecordFiles(2);

            return ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int InsertDailyLcMarcRecords()
        {
            var sql = InsertDailyMarcRecordFiles(1);

            return ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int InsertDailyRittenhouseMarcRecords()
        {
            int batchCount = 10000;
            var sql = $@"
                Insert Into DailyMarcRecordFile
                select top {batchCount} mr.isbn10, mr.isbn13, mr.sku, mrp.marcRecordProviderTypeId, mrf.fileData
                from MarcRecord mr
                join MarcRecordProvider mrp on mr.marcRecordId = mrp.marcRecordId and mrp.marcRecordProviderTypeId = 3
                join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId 
                left join ExcludedMarcRecord emr on mr.sku = emr.sku and mrpt.marcRecordProviderTypeId = emr.marcRecordProviderTypeId
                join MarcRecordFile mrf on mrp.marcRecordProviderId = mrf.marcRecordProviderId and marcRecordFileTypeId = 2
                left join DailyMarcRecordFile dmrf on mr.isbn10 = dmrf.isbn10 and mr.isbn13 = dmrf.isbn13 and mr.sku = dmrf.sku
                where dmrf.dailyMarcRecordFileId is null and mr.sku not like '%R2P%' and emr.excludedMarcRecordId is null
            ";

            int totalRecordCount = 0;
            int counter = ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
            totalRecordCount += counter;
            Log.Info($"InsertDailyRittenhouseMarcRecords Count: {totalRecordCount}");
            while (counter == batchCount)
            {
                counter = ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
                totalRecordCount += counter;
                Log.Info($"InsertDailyRittenhouseMarcRecords Count: {totalRecordCount}");
            }

            return totalRecordCount;
        }

        public static void ReIndexDailyMarcRecords()
        {
            RebuildIndexTable("DailyMarcRecordFile");

            var sql = string.Format(" EXEC sp_updatestats ");

            SqlConnection cnn = null;
            SqlCommand command = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 360;

                command.ExecuteNonQuery();

                stopWatch.Stop();
                Log.DebugFormat("update stats time: {0}ms", stopWatch.ElapsedMilliseconds);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command);
            }

        }

        private static void RebuildIndexTable(string tableName)
        {
            var sql = string.Format("DBCC DBREINDEX (\"[MarcRecords]..{0}\", \" \", 80);", tableName);

            SqlConnection cnn = null;
            SqlCommand command = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 120;

                command.ExecuteNonQuery();

                stopWatch.Stop();
                Log.DebugFormat("reindex time: {0}ms", stopWatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command);
            }
        }
    }
}
