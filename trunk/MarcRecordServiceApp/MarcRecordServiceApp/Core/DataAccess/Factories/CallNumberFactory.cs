using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;

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
    }
}
