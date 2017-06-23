using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class MarcRecordsProductFactory : FactoryBase
    {
		/// <summary>
		/// This string contains all product related fields except synopsis.  Use PopulateProduct() to populate
		/// </summary>
		public static readonly string ProductSelectFields = new StringBuilder()
            .Append("p.productId, p.sku, p.isbn10, p.isbn13, p.title, p.subTitle, p.authors, p.firstAuthorLastName, p.productStatusId ")
            .Append(", p.copyright, p.publicationDate, p.format ")
            .Append(", pub.publisherId, pub.publisherName ")
            .Append(", cat.categoryId, cat.categoryName ")
			.ToString();

        public static readonly string SelectMarcRecordandProviderFields = new StringBuilder()
            .Append("  , mr.MarcRecordId, mr.Isbn10, mr.Isbn13, mr.sku, mrp.MarcRecordProviderId, mrp.MarcRecordId,  ")
            .Append(" mrp.MarcRecordProviderTypeId, mrp.EncodingLevel, mrp.DateCreated, mrp.DateUpdated  ")
            .ToString();

        public static string FromProductAndMarcRecords(int providerId)
        {
            var sb =  new StringBuilder()
            .Append("from   RittenhouseWeb.dbo.Product p ")
            .Append(" left join  RittenhouseWeb.dbo.Category cat on p.categoryId = cat.categoryId ")
            .Append(" left join  RittenhouseWeb.dbo.Publisher pub on p.publisherId = pub.publisherId ")
            .Append(" left join dbo.MarcRecord mr on mr.sku = p.sku ")
            .Append(" left join dbo.MarcRecordProvider mrp on mrp.MarcRecordId = mr.MarcRecordId ")
            .AppendFormat(" and mrp.MarcRecordProviderTypeId = {0} ", providerId)
            .Append(" where p.isAvailableForSale = 1 ")
            .Append(" and  p.productStatusId in (1, 2, 4, 5, 8) and p.sku not like '%R2P%' ");
            if (providerId == 3)
            {
                sb.AppendFormat(" and  (p.copyright is not null or p.publicationDate is not null) ");
            }
            sb.Append(" and  (mrp.DateUpdated is null or mrp.DateUpdated < getdate() - 7) ");

            return sb.ToString();
        }

        private static readonly string BaseRittenhouseMarcRecords = new StringBuilder()
            .Append(" from   RittenhouseWeb.dbo.Product p ")
            .Append(" left join  RittenhouseWeb.dbo.Category cat on p.categoryId = cat.categoryId ")
            .Append(" left join  RittenhouseWeb.dbo.Publisher pub on p.publisherId = pub.publisherId ")
            .Append(" left join dbo.MarcRecord mr on mr.isbn13 = p.isbn13 ")
            .Append(" left join dbo.MarcRecordProvider mrp on mrp.MarcRecordId = mr.MarcRecordId and mrp.MarcRecordProviderTypeId = 3 ")
            .Append(" where (p.copyright is not null or p.publicationDate is not null)  and p.sku not like '%R2P%'")
            .ToString();


        public static readonly string FromMissingMarcRecordsRittenhouseOnly = new StringBuilder()
            .Append(BaseRittenhouseMarcRecords)
            .Append(" and (mrp.dateCreated is null) ")
            .ToString();

        public static readonly string FromAllMarcRecordsRittenhouseOnly = new StringBuilder()
                    .Append(BaseRittenhouseMarcRecords)
                    .ToString();


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
        /// <param name="batchSize"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static List<IMarcFile> GetProductsWithoutMarcRecords(int batchSize, MarcRecordProvider providerType)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            List<IMarcFile> marcFiles = new List<IMarcFile>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
            .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
            .Append(FromProductAndMarcRecords((int)providerType))
            .Append(" order by mrp.DateUpdated, p.productStatusId asc, p.orderByDate ")
            .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductEntity productEntity = new ProductEntity();
                    productEntity.Populate(reader);
                    switch (providerType)
                    {
                        case MarcRecordProvider.Lc:
                            marcFiles.Add(new LcMarcFile(productEntity.GetProduct()));
                            break;
                        case MarcRecordProvider.Nlm:
                            marcFiles.Add(new NlmMarcFile(productEntity.GetProduct()));
                            break;
                            case MarcRecordProvider.Rbd:
                            marcFiles.Add(new RittenhouseMarcFile(productEntity.GetProduct()));
                            break;
                    }
                }
                return marcFiles;
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
        }

        /// <summary>
        /// This will only Rittenhouse Products WITHOUT MARC records
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static List<IMarcFile> GetProductsWithoutMarcRecords(int batchSize)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            List<IMarcFile> marcFiles = new List<IMarcFile>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
            .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
            .Append(FromMissingMarcRecordsRittenhouseOnly)
            .Append(" order by mrp.DateUpdated, p.productStatusId asc, p.orderByDate ")
            .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductEntity productEntity = new ProductEntity();
                    productEntity.Populate(reader);

                    marcFiles.Add(new RittenhouseMarcFile(productEntity.GetProduct()));
                }
                return marcFiles;
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static int GetProductsWithoutMarcRecordsCount(MarcRecordProvider providerType)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append("select count(*) ")
                .Append(FromProductAndMarcRecords((int)providerType))
                .ToString();
            
            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                int count = -2;
                if (reader.Read())
                {
                    count = GetInt32Value(reader, 0, -1);
                }
                return count;
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
        }

        /// <summary>
        /// This will only get the count for Rittenhouse Products WITHOUT MARC records
        /// </summary>
        /// <returns></returns>
        public static int GetProductsWithoutMarcRecordsCount()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append("select count(*) ")
                .Append(FromMissingMarcRecordsRittenhouseOnly)
                .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                int count = -2;
                if (reader.Read())
                {
                    count = GetInt32Value(reader, 0, -1);
                }
                return count;
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


        }

        public static int GetAllRittenhouseMarcRecordProductsCount()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append("select count(*) ")
                .Append(FromAllMarcRecordsRittenhouseOnly)
                .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                int count = -2;
                if (reader.Read())
                {
                    count = GetInt32Value(reader, 0, -1);
                }
                return count;
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
        }

        /// <summary>
        /// This will only Rittenhouse Products WITHOUT MARC records
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static List<IMarcFile> GetAllRittenhouseMarcRecordProducts(int batchSize)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            List<IMarcFile> marcFiles = new List<IMarcFile>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
            .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
            .Append(FromAllMarcRecordsRittenhouseOnly)
            .Append(" order by mrp.DateUpdated, p.productStatusId asc, p.orderByDate ")
            .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductEntity productEntity = new ProductEntity();
                    productEntity.Populate(reader);

                    marcFiles.Add(new RittenhouseMarcFile(productEntity.GetProduct()));
                }                
                return marcFiles;
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
        }

        public static int GetFullRittenhouseMarcRecordProductsCount()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append("select count(*) ")
                .Append(FromMissingMarcRecordsRittenhouseOnly)
                .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                int count = -2;
                if (reader.Read())
                {
                    count = GetInt32Value(reader, 0, -1);
                }
                return count;
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <returns></returns>
        public static int InsertUpdateMarcRecordFile(IMarcFile marcFile)
        {
            int marcRecordId = GetProductMarcRecordId(marcFile);

            string sql = new StringBuilder()
                .Append(" select count(*) from MarcRecordProvider mrp ")
                .Append(" where mrp.marcRecordId = @MarcRecordId and mrp.marcRecordProviderTypeId = @MarcRecordProviderTypeId ")
                .ToString();

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new Int32Parameter("MarcRecordId", marcRecordId),
                                                            new Int32Parameter("MarcRecordProviderTypeId", (int)marcFile.RecordProvider)
                                                        };
            int recordCount = ExecuteBasicCountQuery(sql, parameters, true, Settings.Default.RittenhouseMarcDb);
            int marcRecordProvider;
            if (recordCount > 0)
            {
                marcRecordProvider = UpdateMarcRecordProvider(marcFile, marcRecordId);
                if (marcFile.MrcFileText != null)
                {
                    return UpdateMarcRecordFile(marcFile, marcRecordProvider);
                }
                Log.DebugFormat("UpdateMarcRecordFile -- marcFile.MrcFileText is null || For MarcRecordId :{0}", marcRecordId);
            }
            else
            {
                marcRecordProvider = InsertMarcRecordProvider(marcFile, marcRecordId);
                if (marcFile.MrcFileText != null)
                {
                    return InsertMarcRecordFile(marcFile, marcRecordProvider);
                }
                Log.DebugFormat("InsertMarcRecordFile -- marcFile.MrcFileText is null || For MarcRecordId :{0}", marcRecordId);
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <param name="marcRecordId"></param>
        /// <returns></returns>
        public static int InsertMarcRecordProvider(IMarcFile marcFile, int marcRecordId)
        {
            string sql = new StringBuilder()
                .Append(" insert into MarcRecordProvider (MarcRecordId, MarcRecordProviderTypeId, EncodingLevel, DateCreated, dateUpdated) ")
                .Append(" VALUES (@MarcRecordId, @MarcRecordProviderTypeId, @EncodingLevel, @DateCreated, @DateUpdated) ")
                .ToString();

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new Int32Parameter("MarcRecordId", marcRecordId),
                                                            new Int32Parameter("MarcRecordProviderTypeId", (int)marcFile.RecordProvider),
                                                            new DateTimeParameter("DateCreated",marcFile.ProcessedDate),
                                                            new DateTimeParameter("DateUpdated",marcFile.ProcessedDate),
                                                            new StringParameter("EncodingLevel", marcFile.EncodingLevel)
                                                        };
            int marcRecordProviderId = ExecuteInsertStatementReturnIdentity(sql, parameters.ToArray(), true, Settings.Default.RittenhouseMarcDb);

            return marcRecordProviderId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <param name="marcRecordId"></param>
        /// <returns></returns>
        public static int UpdateMarcRecordProvider(IMarcFile marcFile, int marcRecordId)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            int id = -1;

            try
            {
                string sql = new StringBuilder()
                    .Append(" update MarcRecordProvider set EncodingLevel = @EncodingLevel, DateUpdated = @DateUpdated ")
                    .Append(" where MarcRecordId = @MarcRecordId and MarcRecordProviderTypeId = @MarcRecordProviderTypeId; ")
                    .Append(" select marcRecordProviderId from MarcRecordProvider where MarcRecordId = @MarcRecordId and MarcRecordProviderTypeId = @MarcRecordProviderTypeId; ")
                    .ToString();

                List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                            {
                                                                new Int32Parameter("MarcRecordId", marcRecordId),
                                                                new Int32Parameter("MarcRecordProviderTypeId", (int)marcFile.RecordProvider),
                                                                new DateTimeParameter("DateUpdated",marcFile.ProcessedDate),
                                                                new StringParameter("EncodingLevel", marcFile.EncodingLevel == "" ? "": marcFile.EncodingLevel)
                                                            };
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = GetSqlCommand(cnn, sql, parameters.ToArray());

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = GetInt32Value(reader, "marcRecordProviderId", 0);
                }

                return id;
            }
            catch (Exception ex)
            {
                LogCommandInfo(command);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <param name="marcRecordProviderId"></param>
        /// <returns></returns>
        public static int InsertMarcRecordFile(IMarcFile marcFile, int marcRecordProviderId)
        {
            int rowCount = 0;
            if (marcFile.MrcFileText != null)
            {
                string sql = new StringBuilder()
                    .Append(" insert into MarcRecordFile (MarcRecordProviderId, MarcRecordFileTypeId, FileData) ")
                    .Append(" VALUES (@MarcRecordProviderId, @MarcRecordFileTypeId1, @FileData1); ")
                    .Append(" insert into MarcRecordFile (MarcRecordProviderId, MarcRecordFileTypeId, FileData) ")
                    .Append(" VALUES (@MarcRecordProviderId, @MarcRecordFileTypeId2, @FileData2); ")
                    .Append(" insert into MarcRecordFile (MarcRecordProviderId, MarcRecordFileTypeId, FileData) ")
                    .Append(" VALUES (@MarcRecordProviderId, @MarcRecordFileTypeId3, @FileData3); ")
                    .ToString();

                List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new Int32Parameter("MarcRecordProviderId", marcRecordProviderId),
                                                            new Int32Parameter("MarcRecordFileTypeId1", 1),
                                                            new Int32Parameter("MarcRecordFileTypeId2", 2),
                                                            new Int32Parameter("MarcRecordFileTypeId3", 3),
                                                            new StringParameter("FileData1", marcFile.MrcFileText),
                                                            new StringParameter("FileData2", marcFile.MrkFileText),
                                                            new StringParameter("FileData3", marcFile.XmlFileText)
                                                        };

                rowCount = ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
            }
            return rowCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <param name="marcRecordProviderId"></param>
        /// <returns></returns>
        public static int UpdateMarcRecordFile(IMarcFile marcFile, int marcRecordProviderId)
        {
            string sql2 = new StringBuilder()
                .Append(" select count(*) from MarcRecordFile ")
                .Append(" where marcRecordProviderId = @MarcRecordProviderId ")
                .ToString();

            List<ISqlCommandParameter> parameters2 = new List<ISqlCommandParameter>
                                                         {
                                                             new Int32Parameter("MarcRecordProviderId",
                                                                                marcRecordProviderId),
                                                         };
            int recordCount = ExecuteBasicCountQuery(sql2, parameters2, true, Settings.Default.RittenhouseMarcDb);

            if (recordCount > 0)
            {
                
                string sql = new StringBuilder()
                    .Append(" update MarcRecordFile set FileData = @FileData1 ")
                    .Append(" where MarcRecordProviderId = @MarcRecordProviderId and MarcRecordFileTypeId = @MarcRecordFileTypeId1; ")
                    .Append(" update MarcRecordFile set FileData = @FileData2 ")
                    .Append(" where MarcRecordProviderId = @MarcRecordProviderId and MarcRecordFileTypeId = @MarcRecordFileTypeId2; ")
                    .Append(" update MarcRecordFile set FileData = @FileData3 ")
                    .Append(" where MarcRecordProviderId = @MarcRecordProviderId and MarcRecordFileTypeId = @MarcRecordFileTypeId3; ")
                    .ToString();

                List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                    {
                                                        new Int32Parameter("MarcRecordProviderId", marcRecordProviderId),
                                                        new Int32Parameter("MarcRecordFileTypeId1", 1),
                                                        new Int32Parameter("MarcRecordFileTypeId2", 2),
                                                        new Int32Parameter("MarcRecordFileTypeId3", 3),
                                                        new StringParameter("FileData1", marcFile.MrcFileText),
                                                        new StringParameter("FileData2", marcFile.MrkFileText),
                                                        new StringParameter("FileData3", marcFile.XmlFileText)
                                                    };

                return ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
            }            
            return InsertMarcRecordFile(marcFile, marcRecordProviderId);            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <returns></returns>
        public static int GetProductMarcRecordId(IMarcFile marcFile)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            
            string sql = string.Format("select MarcRecordId from MarcRecord where sku = '{0}' ", marcFile.Product.Sku);
            try
            {
                cnn = GetConnection(Settings.Default.RittenhouseMarcDb);
                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                command = GetSqlCommand(cnn, sql);


                reader = command.ExecuteReader();

                int id = reader.Read() ? GetInt32Value(reader, "MarcRecordId", -1) : InsertProductMarcRecord(marcFile);
                return id;
            }
            catch (Exception ex)
            {
                LogCommandInfo(command);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcFile"></param>
        /// <returns></returns>
        public static int InsertProductMarcRecord(IMarcFile marcFile)
        {
            StringBuilder sql = new StringBuilder()
                .Append("Insert into MarcRecord (isbn10,isbn13,sku) ")
                .Append("VALUES (@ISBN10, @ISBN13, @Sku)");

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                    {
                                        new StringParameter("ISBN10",marcFile.Product.Isbn10),
                                        new StringParameter("ISBN13",marcFile.Product.Isbn13),
                                        new StringParameter("Sku",marcFile.Product.Sku)
                                    };
            int id = ExecuteInsertStatementReturnIdentity(sql.ToString(), parameters.ToArray(), true, Settings.Default.RittenhouseMarcDb);
            return id;
        }
       


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int TruncateDailyMarcRecords()
        {
            int rowCount = ExecuteTrancateTable("DailyMarcRecordFile", Settings.Default.RittenhouseMarcDb);
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
            var sql = InsertDailyMarcRecordFiles(3);

            return ExecuteInsertStatementReturnRowCount(sql, null, true, Settings.Default.RittenhouseMarcDb);
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
