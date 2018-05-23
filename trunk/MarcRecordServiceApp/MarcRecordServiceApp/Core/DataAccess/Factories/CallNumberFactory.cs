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
        private readonly string _lcMarcRecordFileUpdateSql = new StringBuilder()
            .Append(" Update LcMarcRecordFile ")
            .Append(" set  ")
            .Append("     LcMarcRecordFile.xmlFileData = mrfXML.filedata, ")
            .Append("     LcMarcRecordFile.dateUpdated = getdate() ")
            .Append(" from MarcRecord mr ")
            .Append(" join MarcRecordProvider mrp on mr.marcRecordId = mrp.marcRecordId and mrp.marcRecordProviderTypeId = 1 ")
            .Append(" join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
            .Append(" join MarcRecordFile mrfXML on mrp.marcRecordProviderId = mrfXML.marcRecordProviderId and mrfXML.marcRecordFileTypeId = 3 ")
            .Append(" join LcMarcRecordFile lmrf on mr.sku = lmrf.sku ")
            .Append(" where mr.sku not like '%R2P%' and lmrf.XMLfileData != mrfXML.filedata ; ")
            .ToString();

        private readonly string _lcMarcRecordFileInsertSql = new StringBuilder()
            .Append(" Insert Into LcMarcRecordFile (isbn10, isbn13, sku, xmlFiledata, dateUpdated) ")
            .Append("     select mr.isbn10, mr.isbn13, mr.sku, mrfXML.filedata, getdate() ")
            .Append(" from MarcRecord mr ")
            .Append(" join MarcRecordProvider mrp on mr.marcRecordId = mrp.marcRecordId and mrp.marcRecordProviderTypeId = 1 ")
            .Append(" join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
            .Append(" join MarcRecordFile mrfXML on mrp.marcRecordProviderId = mrfXML.marcRecordProviderId and mrfXML.marcRecordFileTypeId = 3 ")
            .Append(" left join LcMarcRecordFile lmrf on mr.sku = lmrf.sku ")
            .Append(" where mr.sku not like '%R2P%' and lmrf.lcMarcRecordFileId is null; ")
            .ToString();

        private readonly string _lcGetMarcRecordFileXml = new StringBuilder()
            .Append(" select top {0} lcMarcRecordFileId, xmlFileData, lcCallNumber ")
            .Append(" from LcMarcRecordFile ")
            .Append(" where (lcCallNumber is null and dateCallNumberProcessed is null) or dateUpdated > dateCallNumberProcessed ")
            .ToString();

        private readonly string _lcUpdateMarcRecordFileCallNumber = new StringBuilder()
            .Append(" Update LcMarcRecordFile ")
            .Append(" set ")
            .Append("     dateCallNumberProcessed = GETDATE(), ")
            .Append("     lcCallNumber = @LcCallNumber_{0} ")
            .Append(" where lcMarcRecordFileId = @LcMarcRecordFileId_{0}; ")
            .ToString();
        #endregion

        #region "NLM Sql"
        private readonly string _nlmMarcRecordFileUpdateSql = new StringBuilder()
            .Append(" Update NlmMarcRecordFile ")
            .Append(" set  ")
            .Append("     NlmMarcRecordFile.xmlFileData = mrfXML.filedata, ")
            .Append("     NlmMarcRecordFile.dateUpdated = getdate() ")
            .Append(" from MarcRecord mr ")
            .Append(" join MarcRecordProvider mrp on mr.marcRecordId = mrp.marcRecordId and mrp.marcRecordProviderTypeId = 2 ")
            .Append(" join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
            .Append(" join MarcRecordFile mrfXML on mrp.marcRecordProviderId = mrfXML.marcRecordProviderId and mrfXML.marcRecordFileTypeId = 3 ")
            .Append(" join NlmMarcRecordFile nmrf on mr.sku = nmrf.sku ")
            .Append(" where mr.sku not like '%R2P%' and nmrf.XMLfileData != mrfXML.filedata ; ")
            .ToString();

        private readonly string _nlmMarcRecordFileInsertSql = new StringBuilder()
            .Append(" Insert Into NlmMarcRecordFile (isbn10, isbn13, sku, xmlFiledata, dateUpdated) ")
            .Append("     select mr.isbn10, mr.isbn13, mr.sku, mrfXML.filedata, getdate() ")
            .Append(" from MarcRecord mr ")
            .Append(" join MarcRecordProvider mrp on mr.marcRecordId = mrp.marcRecordId and mrp.marcRecordProviderTypeId = 2 ")
            .Append(" join MarcRecordProviderType mrpt on mrp.marcRecordProviderTypeId = mrpt.marcRecordProviderTypeId ")
            .Append(" join MarcRecordFile mrfXML on mrp.marcRecordProviderId = mrfXML.marcRecordProviderId and mrfXML.marcRecordFileTypeId = 3 ")
            .Append(" left join NlmMarcRecordFile nmrf on mr.sku = nmrf.sku ")
            .Append(" where mr.sku not like '%R2P%' and nmrf.nlmMarcRecordFileId is null; ")
            .ToString();

        private readonly string _nlmGetMarcRecordFileXml = new StringBuilder()
            .Append(" select top {0} nlmMarcRecordFileId, xmlFileData, nlmCallNumber ")
            .Append(" from NlmMarcRecordFile ")
            .Append(" where (nlmCallNumber is null and dateCallNumberProcessed is null) or dateUpdated > dateCallNumberProcessed ")
            .ToString();

        private readonly string _nlmUpdateMarcRecordFileCallNumber = new StringBuilder()
            .Append(" Update NlmMarcRecordFile ")
            .Append(" set ")
            .Append("     dateCallNumberProcessed = GETDATE(), ")
            .Append("     nlmCallNumber = @NlmCallNumber_{0} ")
            .Append(" where nlmMarcRecordFileId = @NlmMarcRecordFileId_{0}; ")
            .ToString();

        private readonly string _nlmMarcFilesNeedToBeParseCount = new StringBuilder()
            .Append("SELECT count(nlmMarcRecordFileId) ")
            .Append("      FROM NlmMarcRecordFile ")
            .Append("WHERE (nlmCallNumber is null and dateCallNumberProcessed is null) or dateUpdated > dateCallNumberProcessed ")

            .ToString();
        #endregion

        #region "LC Queries"
        public int UpdateLcMarcRecordFiles()
        {
            var sql = _lcMarcRecordFileUpdateSql;

            return ExecuteUpdateStatement(sql, null, true, Settings.Default.RittenhouseMarcDb);
        }

        public int InsertLcMarcRecordFiles()
        {
            var sql = _lcMarcRecordFileInsertSql;

            return ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
        }

        public List<LcMarcRecordFile> GetBatchedLcMarcRecordFiles(int batchSize)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = string.Format(_lcGetMarcRecordFileXml, batchSize);

            List<LcMarcRecordFile> lcMarcRecordFiles = new List<LcMarcRecordFile>();
            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                command.Parameters.AddWithValue("BatchSize", batchSize);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    LcMarcRecordFile lcMarcRecordFile = new LcMarcRecordFile();
                    lcMarcRecordFile.Populate(reader);
                    lcMarcRecordFiles.Add(lcMarcRecordFile);

                }
            }
            catch (Exception ex)
            {
                Log.InfoFormat("sql: {0}", sql);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
            return lcMarcRecordFiles;
        }

        public int UpdateLcCallNumbers(List<LcMarcRecordFile> lcMarcRecordFiles)
        {
            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>();
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < lcMarcRecordFiles.Count; i++)
            {
                sql.AppendFormat(_lcUpdateMarcRecordFileCallNumber, i);

                parameters.Add(new StringParameter(string.Format("LcCallNumber_{0}", i), lcMarcRecordFiles[i].LcCallNumber));
                parameters.Add(new Int32Parameter(string.Format("LcMarcRecordFileId_{0}", i), lcMarcRecordFiles[i].Id));
            }
            return ExecuteUpdateStatement(sql.ToString(), parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
        }
        #endregion

        #region "NLM Queries"
        public int UpdateNlmMarcRecordFiles()
        {
            var sql = _nlmMarcRecordFileUpdateSql;

            return ExecuteUpdateStatement(sql, null, true, Settings.Default.RittenhouseMarcDb);
        }

        public int InsertNlmMarcRecordFiles()
        {
            var sql = _nlmMarcRecordFileInsertSql;

            return ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
        }

        public List<NlmMarcRecordFile> GetBatchedNlmMarcRecordFiles(int batchSize)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = string.Format(_nlmGetMarcRecordFileXml, batchSize);

            List<NlmMarcRecordFile> nlmMarcRecordFiles = new List<NlmMarcRecordFile>();
            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                command.Parameters.AddWithValue("BatchSize", batchSize);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    NlmMarcRecordFile nlmMarcRecordFile = new NlmMarcRecordFile();
                    nlmMarcRecordFile.Populate(reader);
                    nlmMarcRecordFiles.Add(nlmMarcRecordFile);

                }
            }
            catch (Exception ex)
            {
                Log.InfoFormat("sql: {0}", sql);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
            return nlmMarcRecordFiles;
        }

        public int UpdateNlmCallNumbers(List<NlmMarcRecordFile> nlmMarcRecordFiles)
        {
            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>();
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < nlmMarcRecordFiles.Count; i++)
            {
                sql.AppendFormat(_nlmUpdateMarcRecordFileCallNumber, i);

                parameters.Add(new StringParameter(string.Format("NlmCallNumber_{0}", i), nlmMarcRecordFiles[i].NlmCallNumber));
                parameters.Add(new Int32Parameter(string.Format("NlmMarcRecordFileId_{0}", i), nlmMarcRecordFiles[i].Id));
            }
            return ExecuteUpdateStatement(sql.ToString(), parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
        }

        public int GetNlmCallNumberCount()
        {
            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>();

            var sql = _nlmMarcFilesNeedToBeParseCount;
            return ExecuteBasicCountQuery(sql, parameters, false, Settings.Default.RittenhouseMarcDb);
        }
        #endregion





        private readonly string InsertNlmFields = @"
select top {0} mr.marcRecordId, mrf.fileData, par.dateCreated, mrp.dateCreated, mrp.dateUpdated
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
                command.CommandText = string.Format(InsertNlmFields, batchSize);
                command.CommandTimeout = 150;

                //command.Parameters.AddWithValue("BatchSize", batchSize);

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
                Log.InfoFormat("sql: {0}", InsertNlmFields);
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




    public class MarcRecordData : FactoryBase, IDataEntity
    {
        public int MarcRecordId { get; set; }
        public string FileData { get; set; }

        //public List<MarcRecordField> MarcRecordFields { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                MarcRecordId = GetInt32Value(reader, "marcRecordId", -1);
                FileData = GetStringValue(reader, "fileData");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }

    //public class MarcRecordField
    //{
    //    public string Identifier { get; set; }
    //    public string Value { get; set; }

    //    public string EntireField { get; set; }
    //}


    public class MarcRecordDataField : FactoryBase, IDataEntity
    {
        public MarcRecordDataField()
        {
            MarcRecordDataSubFields = new List<MarcRecordDataSubField>();
        }
        public int Id { get; set; }
        public int MarcRecordId { get; set; }
        public string FieldNumber { get; set; }
        public string FieldIndicator { get; set; }
        public string MarcValue { get; set; }
        public List<MarcRecordDataSubField> MarcRecordDataSubFields { get; set; }
        public void Populate(SqlDataReader reader)
        {
            try
            {
                Id = GetInt32Value(reader, "marcRecordDataFieldId", -1);
                MarcRecordId = GetInt32Value(reader, "marcRecordId", -1);
                FieldNumber = GetStringValue(reader, "fieldNumber");
                FieldIndicator = GetStringValue(reader, "fieldIndicator");
                MarcValue = GetStringValue(reader, "marcValue");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }

    public class MarcRecordDataSubField : FactoryBase, IDataEntity
    {
        public int Id { get; set; }
        public int MarcRecordDataFieldId{ get; set; }
        public string SubFieldIndicator { get; set; }
        public string SubFieldValue { get; set; }


        public void Populate(SqlDataReader reader)
        {
            try
            {
                Id = GetInt32Value(reader, "marcRecordDataSubFieldsId", -1);
                MarcRecordDataFieldId = GetInt32Value(reader, "marcRecordDataFieldId", -1);
                SubFieldIndicator = GetStringValue(reader, "subFieldIndicator");
                SubFieldValue = GetStringValue(reader, "subFieldValue");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }
}
