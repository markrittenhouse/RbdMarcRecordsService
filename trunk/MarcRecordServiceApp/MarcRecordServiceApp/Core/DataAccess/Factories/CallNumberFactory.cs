using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class CallNumberFactory : FactoryBase
    {
        #region "LC Sql"

        private const string LcCallNumberInsert = @"
insert into MarcRecordDataCallNumber(marcRecordId, providerId, dateCreated, callNumber)
select x.marcRecordId, x.marcRecordProviderTypeId, GETDATE()
    , STUFF(( SELECT '' + sub.subFieldValue
                from MarcRecordDataSubField sub 
                WHERE x.marcRecordDataFieldId = sub.marcRecordDataFieldId
                FOR XML PATH('')), 1, 0,'') AS value
from (select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId
        from MarcRecordDataField mrf 
        where mrf.fieldNumber = '050' and mrf.marcRecordProviderTypeId = 1
        group by mrf.marcRecordId, mrf.marcRecordProviderTypeId
    ) x
    left join MarcRecordDataCallNumber mrcn on x.marcRecordId = mrcn.marcRecordId and x.marcRecordProviderTypeId = mrcn.providerId
    where mrcn.marcRecordId is null
";

        private const string LcCallNumberUpdate = @"
update MarcRecordDataCallNumber
set callNumber = xx.value, dateUpdated = GETDATE()
from MarcRecordDataCallNumber mrcn
join (
    select x.marcRecordId, x.marcRecordProviderTypeId, x.dateCreated
        , STUFF(( SELECT '' + c2.subFieldValue
            from MarcRecordDataSubField c2 
            WHERE x.marcRecordDataFieldId = c2.marcRecordDataFieldId
            FOR XML PATH('')), 1, 0,'') AS value
    from (select min(xx.marcRecordDataFieldId) marcRecordDataFieldId, xx.marcRecordId, xx.marcRecordProviderTypeId, xx.dateCreated
            from MarcRecordDataField xx 
            where xx.fieldNumber = '050' and xx.marcRecordProviderTypeId = 1
            group by xx.marcRecordId, xx.marcRecordProviderTypeId, xx.dateCreated
        ) x
    ) xx on mrcn.marcRecordId = xx.marcRecordId and mrcn.providerId = xx.marcRecordProviderTypeId
where xx.dateCreated > isnull(mrcn.dateUpdated, mrcn.dateCreated)

";

        private const string LcCategoryUpdate = @"
update MarcRecordDataCallNumber
set category = substring(replace(subTest.subFieldValue, '''', ''''), 0, 500), dateUpdated = GETDATE()
from MarcRecordDataCallNumber mrcn
join (
    select p.marcRecordId, min(sub.marcRecordDataSubFieldsId) marcRecordDataSubFieldsId, p.marcRecordProviderTypeId
    from MarcRecordDataSubField sub
    join MarcRecordDataField p on p.marcRecordDataFieldId = sub.marcRecordDataFieldId and p.marcRecordProviderTypeId = 1
    where p.fieldNumber = '650' and sub.subFieldIndicator = '$a' 
    group by p.marcRecordId, p.marcRecordProviderTypeId
) as x on x.marcRecordId = mrcn.marcRecordId and mrcn.providerId = x.marcRecordProviderTypeId
join MarcRecordDataSubField subTest on x.marcRecordDataSubFieldsId = subTest.marcRecordDataSubFieldsId 
where category is null or category <> substring(replace(subTest.subFieldValue, '''', ''''), 0, 500)

";




        #endregion

        #region "NLM Sql"

        private const string NlmCallNumberInsert = @"
insert into MarcRecordDataCallNumber(marcRecordId, providerId, dateCreated, callNumber)
select x.marcRecordId, x.marcRecordProviderTypeId, GETDATE()
, STUFF(( SELECT ', ' + sub.subFieldValue
                from MarcRecordDataSubField sub 
                WHERE x.marcRecordDataFieldId = sub.marcRecordDataFieldId
                FOR XML PATH('')), 1, 2,'') AS value
from (
    select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId
    from MarcRecordDataField mrf
    where mrf.fieldNumber = '060' and mrf.marcRecordProviderTypeId = 2 and mrf.fieldIndicator like '1%'
    group by mrf.marcRecordId, mrf.marcRecordProviderTypeId
    ) x
    left join MarcRecordDataCallNumber mrcn on x.marcRecordId = mrcn.marcRecordId and x.marcRecordProviderTypeId = mrcn.providerId
    where mrcn.marcRecordId is null
";

        private const string NlmCallNumberUpdate = @"
update MarcRecordDataCallNumber
set callNumber = xx.value, dateUpdated = GETDATE()
from MarcRecordDataCallNumber mrcn
join (
    select x.marcRecordId, x.marcRecordProviderTypeId, x.dateCreated
    , STUFF(( SELECT ', ' + sub.subFieldValue
                    from MarcRecordDataSubField sub 
                    WHERE x.marcRecordDataFieldId = sub.marcRecordDataFieldId
                    FOR XML PATH('')), 1, 2,'') AS value
    from (select min(xx.marcRecordDataFieldId) marcRecordDataFieldId, xx.marcRecordId, xx.marcRecordProviderTypeId, xx.dateCreated
            from MarcRecordDataField xx 
            where xx.fieldNumber = '060' and xx.marcRecordProviderTypeId = 2 and xx.fieldIndicator like '1%'
            group by xx.marcRecordId, xx.marcRecordProviderTypeId, xx.dateCreated
        ) x
) as xx on xx.marcRecordId = mrcn.marcRecordId and mrcn.providerId = xx.marcRecordProviderTypeId
where xx.dateCreated > isnull(mrcn.dateUpdated, mrcn.dateCreated)
";

        private const string NlmCategoryUpdate = @"
update MarcRecordDataCallNumber
set category = replace(subTest.subFieldValue, '''', ''''), dateUpdated = GETDATE()
from MarcRecordDataCallNumber mrcn
join (
    select mrf.marcRecordId, min(sub.marcRecordDataSubFieldsId) marcRecordDataSubFieldsId, mrf.marcRecordProviderTypeId
    from MarcRecordDataSubField sub
    join MarcRecordDataField mrf on mrf.marcRecordDataFieldId = sub.marcRecordDataFieldId and mrf.marcRecordProviderTypeId = 2
    where mrf.fieldNumber = '650' and sub.subFieldIndicator = '$a' 
    group by mrf.marcRecordId, mrf.marcRecordProviderTypeId
) as x on x.marcRecordId = mrcn.marcRecordId and mrcn.providerId = x.marcRecordProviderTypeId
join MarcRecordDataSubField subTest on x.marcRecordDataSubFieldsId = subTest.marcRecordDataSubFieldsId 
";
        #endregion

        public int BuildLcCallNumbers()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            int callNumbersInserted = 0;

            

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, LcCallNumberInsert, new ISqlCommandParameter[0], Settings.Default.DatabaseCommandTimeout);
                LogCommandDebug(command);
                int rows = command.ExecuteNonQuery();
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"LC Call Numbers Inserted: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");


                stopwatch = new Stopwatch();
                stopwatch.Start();
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, LcCallNumberUpdate, new ISqlCommandParameter[0], Settings.Default.DatabaseCommandTimeout);
                LogCommandDebug(command);
                rows = command.ExecuteNonQuery();
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"LC Call Numbers Updated: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");


                stopwatch = new Stopwatch();
                stopwatch.Start();
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, LcCategoryUpdate, new ISqlCommandParameter[0], Settings.Default.DatabaseCommandTimeout);
                LogCommandDebug(command);
                var categoryRows = command.ExecuteNonQuery();
                stopwatch.Stop();
                Log.Debug($"LC Categories Updated: {categoryRows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                return callNumbersInserted;
            }
            catch (Exception ex)
            {
                LogCommandInfo(command);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command);
            }
        }


        public int BuildNlmCallNumbers()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;

            int callNumbersInserted = 0;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, NlmCallNumberInsert, new ISqlCommandParameter[0], Settings.Default.DatabaseCommandTimeout);
                LogCommandDebug(command);
                int rows = command.ExecuteNonQuery();
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"NLM Call Numbers Inserted: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");


                stopwatch = new Stopwatch();
                stopwatch.Start();
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, NlmCallNumberUpdate, new ISqlCommandParameter[0], Settings.Default.DatabaseCommandTimeout);
                LogCommandDebug(command);
                rows = command.ExecuteNonQuery();
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"NLM Call Numbers Updated: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                stopwatch = new Stopwatch();
                stopwatch.Start();
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, NlmCategoryUpdate, new ISqlCommandParameter[0], Settings.Default.DatabaseCommandTimeout);
                LogCommandDebug(command);
                var categoryRows = command.ExecuteNonQuery();
                stopwatch.Stop();
                Log.Debug($"NLM Categories effected: {categoryRows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                return callNumbersInserted;
            }
            catch (Exception ex)
            {
                LogCommandInfo(command);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command);
            }
        }





        private readonly string GetExternalMarcRecords = @"
select top {0} mr.marcRecordId, mrf.fileData, par.dateCreated, mrp.dateCreated, mrp.dateUpdated, mrpt.marcRecordProviderTypeId
from MarcRecordFile mrf
join MarcRecordProvider mrp on mrf.marcRecordProviderId = mrp.marcRecordProviderId
join MarcRecord mr on mr.marcRecordId = mrp.marcRecordId
join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId
left join MarcRecordDataField par on mr.marcRecordId = par.marcRecordId
where mrf.marcRecordFileTypeId = 2
and mrpt.marcRecordProviderTypeId in (1,2)
--and mrpt.marcRecordProviderTypeId = 1 -- LC
--and mrpt.marcRecordProviderTypeId = 2	-- NLM
and par.marcRecordDataFieldId is null
order by 1
";

        public List<MarcRecordData> GetNlmAndLcMarcRecords(int batchSize)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<MarcRecordData> nlmMarcFields = new List<MarcRecordData>();
            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = string.Format(GetExternalMarcRecords, batchSize);
                command.CommandTimeout = Settings.Default.DatabaseCommandTimeout;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MarcRecordData marcRecordData = new MarcRecordData();
                    marcRecordData.Populate(reader);
                    nlmMarcFields.Add(marcRecordData);

                }
            }
            catch (Exception ex)
            {
                Log.InfoFormat("sql: {0}", GetExternalMarcRecords);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
            return nlmMarcFields;
        }
    }


}
