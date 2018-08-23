using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class CallNumberFactory : FactoryBase
    {
        #region "LC Sql"
        private const string LcCategoryUpdate = @"
update MarcRecordDataCallNumber
set category = substring(replace(subTest.subFieldValue, '''', ''''), 0, 500), dateUpdated = isnull(mrcn.dateUpdated, mrcn.dateCreated)
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


        private int BuildLcCallNumbers(bool isInsert)
        {
            var sql = isInsert ?
                @"
select x.marcRecordId, x.marcRecordProviderTypeId, sub.subFieldValue, x.dateCreated, null as callNumber
from (select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
        from MarcRecordDataField mrf 
		left join MarcRecordDataCallNumber xxx on mrf.marcRecordId = xxx.marcRecordId and mrf.marcRecordProviderTypeId = xxx.providerId
        where mrf.fieldNumber = '050' and mrf.marcRecordProviderTypeId = 1
		and xxx.marcRecordId is null
        group by mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
    ) x
join MarcRecordDataSubField sub on x.marcRecordDataFieldId = sub.marcRecordDataFieldId
order by sub.marcRecordDataSubFieldsId
"
                :
                @"
select mrcn.marcRecordId, mrcn.callNumber, x.marcRecordProviderTypeId, sub.subFieldValue, x.dateCreated
from MarcRecordDataCallNumber mrcn
join 
(select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
        from MarcRecordDataField mrf 
		left join MarcRecordDataCallNumber xxx on mrf.marcRecordId = xxx.marcRecordId and mrf.marcRecordProviderTypeId = xxx.providerId
        where mrf.fieldNumber = '050' and mrf.marcRecordProviderTypeId = 1
		--and xxx.marcRecordId is null
        group by mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
    ) x on x.marcRecordId = mrcn.marcRecordId and x.marcRecordProviderTypeId = mrcn.providerId
join MarcRecordDataSubField sub on x.marcRecordDataFieldId = sub.marcRecordDataFieldId
where x.dateCreated > isnull(mrcn.dateUpdated, mrcn.dateCreated)
order by sub.marcRecordDataSubFieldsId
";
            List<CallNumberItem> callNumberItems = EntityFactory.GetEntityList<CallNumberItem>(sql, null, true, Settings.Default.RittenhouseMarcDb);
            if (!callNumberItems.Any())
            {
                return 0;
            }
            List<CallNumberItem> callNumbersToSave = BuildCallNumbers(callNumberItems, true);

            return isInsert ? InsertCallNumbers(callNumbersToSave) : UpdateCallNumbers(callNumbersToSave);
        }


        private int BuildNlmCallNumbers(bool isInsert)
        {
            var sql = isInsert ?
                @"
select x.marcRecordId, x.marcRecordProviderTypeId, sub.subFieldValue, x.dateCreated, null as callNumber
from(select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
        from MarcRecordDataField mrf
        left join MarcRecordDataCallNumber xxx on mrf.marcRecordId = xxx.marcRecordId and mrf.marcRecordProviderTypeId = xxx.providerId
        where mrf.fieldNumber = '060' and mrf.marcRecordProviderTypeId = 2 and mrf.fieldIndicator like '1%'
        and xxx.marcRecordId is null
        group by mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
    ) x
join MarcRecordDataSubField sub on x.marcRecordDataFieldId = sub.marcRecordDataFieldId
order by sub.marcRecordDataSubFieldsId
"
                :
                @"
select mrcn.marcRecordId, mrcn.callNumber, x.marcRecordProviderTypeId, sub.subFieldValue, x.dateCreated
from MarcRecordDataCallNumber mrcn
join 
(select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
        from MarcRecordDataField mrf 
        left join MarcRecordDataCallNumber xxx on mrf.marcRecordId = xxx.marcRecordId and mrf.marcRecordProviderTypeId = xxx.providerId
        where mrf.fieldNumber = '060' and mrf.marcRecordProviderTypeId = 2 and mrf.fieldIndicator like '1%'
        group by mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
    ) x on x.marcRecordId = mrcn.marcRecordId and x.marcRecordProviderTypeId = mrcn.providerId
join MarcRecordDataSubField sub on x.marcRecordDataFieldId = sub.marcRecordDataFieldId
where x.dateCreated > isnull(mrcn.dateUpdated, mrcn.dateCreated)
order by sub.marcRecordDataSubFieldsId
";
            List<CallNumberItem> callNumberItems = EntityFactory.GetEntityList<CallNumberItem>(sql, null, true, Settings.Default.RittenhouseMarcDb);

            if (!callNumberItems.Any())
            {
                return 0;
            }
            List<CallNumberItem> callNumbersToSave = BuildCallNumbers(callNumberItems, false);

            return isInsert ? InsertCallNumbers(callNumbersToSave) : UpdateCallNumbers(callNumbersToSave);
        }



        #region Generic
        private List<CallNumberItem> BuildCallNumbers(List<CallNumberItem> callNumberItems, bool isLcCallNumber)
        {
            List<CallNumberItem> callNumbersToSave = new List<CallNumberItem>();
            CallNumberItem lastCallNumberItem = null;
            bool itemIsAdded = false;
            foreach (var callNumberItem in callNumberItems)
            {
                if (lastCallNumberItem == null)
                {
                    lastCallNumberItem = new CallNumberItem
                    {
                        MarcRecordId = callNumberItem.MarcRecordId,
                        CallNumber = callNumberItem.SubFieldValue,
                        DateCreated = callNumberItem.DateCreated,
                        ProviderId = callNumberItem.ProviderId
                    };
                    continue;
                }



                if (callNumberItem.MarcRecordId != lastCallNumberItem.MarcRecordId)
                {
                    if (lastCallNumberItem.MarcRecordId > 0)
                    {
                        callNumbersToSave.Add(lastCallNumberItem);
                        itemIsAdded = true;
                    }

                    lastCallNumberItem = new CallNumberItem
                    {
                        MarcRecordId = callNumberItem.MarcRecordId,
                        CallNumber = callNumberItem.SubFieldValue,
                        DateCreated = callNumberItem.DateCreated,
                        ProviderId = callNumberItem.ProviderId
                    };
                }
                else
                {
                    lastCallNumberItem.CallNumber = $"{lastCallNumberItem.CallNumber}{(isLcCallNumber ? "" : " ")}{callNumberItem.SubFieldValue}";
                    itemIsAdded = false;
                }

            }

            if (!itemIsAdded)
            {
                callNumbersToSave.Add(lastCallNumberItem);
            }

            return callNumbersToSave;
        }
        private static int InsertCallNumbers(List<CallNumberItem> callNumberItems)
        {
            int totalInserted = 0;
            var sqlBuilder = new StringBuilder();
            int counter = 0;
            foreach (var callNumberItem in callNumberItems)
            {
                counter++;
                sqlBuilder.Append($@"
insert into MarcRecordDataCallNumber(marcRecordId, providerId, dateCreated, callNumber)
values({callNumberItem.MarcRecordId}, {callNumberItem.ProviderId}, '{callNumberItem.DateCreated}', '{callNumberItem.CallNumber}');");
                if (counter >= 50)
                {
                    totalInserted += ExecuteInsertStatementReturnRowCount(sqlBuilder.ToString(), null, true, Settings.Default.RittenhouseMarcDb);
                    counter = 0;
                    sqlBuilder = new StringBuilder();
                }
            }

            if (sqlBuilder.Length > 0)
            {
                totalInserted += ExecuteInsertStatementReturnRowCount(sqlBuilder.ToString(), null, true, Settings.Default.RittenhouseMarcDb);
            }

            return totalInserted;
        }
        private static int UpdateCallNumbers(List<CallNumberItem> callNumberItems)
        {
            int totalUpdated = 0;
            var sqlBuilder = new StringBuilder();
            int counter = 0;
            foreach (var callNumberItem in callNumberItems)
            {
                counter++;
                sqlBuilder.Append($@"
update MarcRecordDataCallNumber set callNumber = '{callNumberItem.CallNumber}', dateUpdated = '{callNumberItem.DateCreated}'
where marcRecordId = {callNumberItem.MarcRecordId} and providerId = {callNumberItem.ProviderId};");
                if (counter >= 50)
                {
                    totalUpdated += ExecuteUpdateStatement(sqlBuilder.ToString(), null, true, Settings.Default.RittenhouseMarcDb);
                    counter = 0;
                    sqlBuilder = new StringBuilder();
                }
            }

            if (sqlBuilder.Length > 0)
            {
                totalUpdated += ExecuteUpdateStatement(sqlBuilder.ToString(), null, true, Settings.Default.RittenhouseMarcDb);
            }

            return totalUpdated;
        }
        #endregion


        public int BuildLcCallNumbers()
        {
            int callNumbersInserted = 0;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var rows = BuildLcCallNumbers(true);
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"LC Call Numbers Inserted: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                
                stopwatch = new Stopwatch();
                stopwatch.Start();
                rows = BuildLcCallNumbers(false);
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"LC Call Numbers Updated: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                //Category Update is fine
                stopwatch = new Stopwatch();
                stopwatch.Start();
                var categoryRows = ExecuteUpdateStatement(LcCategoryUpdate, null, true, Settings.Default.RittenhouseMarcDb);
                stopwatch.Stop();
                Log.Debug($"LC Categories Updated: {categoryRows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                return callNumbersInserted;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }

        }


        public int BuildNlmCallNumbers()
        {
            int callNumbersInserted = 0;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var rows = BuildNlmCallNumbers(true);
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"NLM Call Numbers Inserted: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");


                stopwatch = new Stopwatch();
                stopwatch.Start();
                rows = BuildNlmCallNumbers(false);
                callNumbersInserted += rows;
                stopwatch.Stop();
                Log.Debug($"NLM Call Numbers Updated: {rows}, statement time: {stopwatch.ElapsedMilliseconds}ms");


                stopwatch = new Stopwatch();
                stopwatch.Start();
                var categoryRows = ExecuteUpdateStatement(NlmCategoryUpdate, null, true, Settings.Default.RittenhouseMarcDb);
                stopwatch.Stop();
                Log.Debug($"NLM Categories effected: {categoryRows}, statement time: {stopwatch.ElapsedMilliseconds}ms");

                return callNumbersInserted;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
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
            DailyMarcRecordFactory.ReIndexTable("MarcRecordFile");
            DailyMarcRecordFactory.ReIndexTable("MarcRecordProvider");
            DailyMarcRecordFactory.ReIndexTable("MarcRecord");
            DailyMarcRecordFactory.ReIndexTable("MarcRecordProviderType");
            DailyMarcRecordFactory.ReIndexTable("MarcRecordDataField");
            
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

        //x.marcRecordId, x.marcRecordProviderTypeId, sub.subFieldValue
        private class CallNumberItem : FactoryBase, IDataEntity
        {
            public int MarcRecordId { get; set; }
            public int ProviderId { get; set; }
            public string SubFieldValue { get; set; }
            public string CallNumber { get; set; }
            public DateTime DateCreated { get; set; }
            public void Populate(SqlDataReader reader)
            {
                //x.marcRecordId, x.marcRecordProviderTypeId, sub.subFieldValue
                MarcRecordId = GetInt32Value(reader, "marcRecordId", 0);
                ProviderId = GetInt32Value(reader, "marcRecordProviderTypeId", 0);
                SubFieldValue = GetStringValue(reader, "subFieldValue");

                CallNumber = GetStringValue(reader, "callNumber");
                DateCreated = GetDateValue(reader, "dateCreated", DateTime.MinValue);
                //mrcn.marcRecordId, mrcn.callNumber, sub.subFieldValue, mrdf.dateCreated
            }
        }
    }

    

}
