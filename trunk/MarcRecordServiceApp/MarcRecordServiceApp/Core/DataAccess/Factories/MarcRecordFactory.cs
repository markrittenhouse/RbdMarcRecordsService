using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class MarcRecordFactory : FactoryBase
    {
        private readonly SqlBulkCopyFactory _sqlBulkCopyFactory;

        public MarcRecordFactory()
        {
            _sqlBulkCopyFactory = new SqlBulkCopyFactory();
        }


        private string ProductSelectFields = @"
mr.marcRecordId, mrp.marcRecordProviderId
, p.productId, p.sku, p.isbn10, p.isbn13, p.title, p.subTitle, p.authors, p.firstAuthorLastName, p.productStatusId
, p.copyright, p.publicationDate, p.format
, pub.publisherId, pub.publisherName
, cat.categoryId, cat.categoryName
";

        private string FromProductAndMarcRecords(int providerId)
        {
            return $@"
from   RittenhouseWeb.dbo.Product p
left join  RittenhouseWeb.dbo.Category cat on p.categoryId = cat.categoryId
left join  RittenhouseWeb.dbo.Publisher pub on p.publisherId = pub.publisherId
left join dbo.MarcRecord mr on mr.sku = p.sku
left join dbo.MarcRecordProvider mrp on mrp.MarcRecordId = mr.MarcRecordId and mrp.MarcRecordProviderTypeId = {providerId}
where p.isAvailableForSale = 1
and  p.productStatusId in (1, 2, 4, 5, 8) and p.sku not like '%R2P%'
{(providerId == 3 ? " and  (p.copyright is not null or p.publicationDate is not null)" : "")}
and (mrp.DateUpdated is null or mrp.DateUpdated < getdate() - 7)
";
        }


        #region Handles MarcRecords Set/Insert/Update

        /// <summary>
        /// Only used by GetAndBuildSkuMarcRecordIdDictionary and InsertMissingMarcRecords
        /// </summary>
        /// <param name="skuMarcRecordIds"></param>
        /// <returns></returns>
        private void SetSkuMarcRecordIdDictionary(Dictionary<string, int> skuMarcRecordIds)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var skuInClause = string.Join(",", skuMarcRecordIds.Where(x => x.Value == 0).Select(x => $"'{x.Key}'"));

            string sql = $@"
select mr.marcRecordId, mr.sku
from MarcRecord mr
where sku in ({skuInClause})";

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var sku = GetStringValue(reader, "sku");
                    skuMarcRecordIds[sku] = GetInt32Value(reader, "marcRecordId", 0);

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
        }

        /// <summary>
        /// Only used by GetAndBuildSkuMarcRecordIdDictionary
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <param name="skuMarcRecordIds"></param>
        /// <returns></returns>
        private void InsertMissingMarcRecords(List<IMarcFile> marcFiles, Dictionary<string, int> skuMarcRecordIds)
        {
            if (!marcFiles.Any())
            {
                return;
            }

            var marcRecordDataTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecord");

            foreach (var marcFile in marcFiles)
            {
                DataRow row = marcRecordDataTable.NewRow();

                row.SetField(0, marcFile.Product.Isbn10);
                row.SetField(1, marcFile.Product.Isbn13);
                row.SetField(2, marcFile.Product.Sku);

                marcRecordDataTable.Rows.Add(row);
            }

            _sqlBulkCopyFactory.BulkInsertData("MarcRecord", marcRecordDataTable);

            SetSkuMarcRecordIdDictionary(skuMarcRecordIds);
        }

        /// <summary>
        /// Used to get a sku/marcRecordId dictionary based on the marcFiles. If no MarcRecord Exists it will build one
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <returns></returns>
        private Dictionary<string, int> GetAndBuildSkuMarcRecordIdDictionary(List<IMarcFile> marcFiles)
        {
            Dictionary<string, int> marcRecordIdDictionary = marcFiles.ToDictionary(x => x.Product.Sku, y => y.MarcRecordId.GetValueOrDefault(0));

            var missingSkuMarcRecordIds = marcRecordIdDictionary.Where(x => x.Value == 0).ToDictionary(x => x.Key, y => y.Value);

            var missingMarcRecords = marcFiles.Where(x => missingSkuMarcRecordIds.ContainsKey(x.Product.Sku)).ToList();

            InsertMissingMarcRecords(missingMarcRecords, marcRecordIdDictionary);
            return marcRecordIdDictionary;
        }

        #endregion

        #region Handles MarcRecordProviders Set/Insert/Update

        /// <summary>
        /// Only used by GetAndBuildMarcRecordIdMarcProviderIdDicitonary and InsertMissingMarcRecordProviders
        /// </summary>
        /// <param name="marcRecordIdMarcProviderIdDicitonary"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        private void SetMarcRecordIdMarcRecordProviderIdDictionary(
            Dictionary<int, int> marcRecordIdMarcProviderIdDicitonary, MarcRecordProviderType providerType)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var marcRecordIdsInClause = string.Join(",",
                marcRecordIdMarcProviderIdDicitonary.Where(x => x.Value == 0).Select(x => $"{x.Key}"));

            string sql = $@"
select marcRecordId, marcRecordProviderId
from MarcRecordProvider 
where marcRecordProviderTypeId = {(int) providerType} and marcRecordId in ({marcRecordIdsInClause})";

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var marcRecordId = GetInt32Value(reader, "marcRecordId", 0);
                    marcRecordIdMarcProviderIdDicitonary[marcRecordId] =
                        GetInt32Value(reader, "marcRecordProviderId", 0);
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
        }

        /// <summary>
        /// Only used by GetAndBuildMarcRecordIdMarcProviderIdDicitonary
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <param name="marcRecordIdMarcProviderIdDicitonary"></param>
        /// /// <param name="providerType"></param>
        /// <returns></returns>
        private void InsertMissingMarcRecordProviders(List<IMarcFile> marcFiles,
            Dictionary<int, int> marcRecordIdMarcProviderIdDicitonary, MarcRecordProviderType providerType)
        {
            if (!marcFiles.Any())
            {
                return;
            }

            var marcRecordDataTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecordProvider");

            foreach (var marcFile in marcFiles)
            {
                DataRow row = marcRecordDataTable.NewRow();


                row.SetField(0, marcFile.MarcRecordId);
                row.SetField(1, (int) providerType);
                row.SetField(2, marcFile.EncodingLevel);
                row.SetField(3, marcFile.ProcessedDate);
                row.SetField(4, marcFile.ProcessedDate);

                marcRecordDataTable.Rows.Add(row);
            }

            _sqlBulkCopyFactory.BulkInsertData("MarcRecordProvider", marcRecordDataTable);

            SetMarcRecordIdMarcRecordProviderIdDictionary(marcRecordIdMarcProviderIdDicitonary, providerType);
        }

        /// <summary>
        /// Used to get a marcRecordId/marcRecordProviderId dictionary based on the marcFiles. If no MarcRecordProvider Exists it will build one
        /// </summary>
        /// <param name="marcFiles"></param>
        /// <param name="providerTypeType"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetAndBuildMarcRecordIdMarcProviderIdDicitonary(List<IMarcFile> marcFiles,
            MarcRecordProviderType providerTypeType)
        {
            var marcRecordIds = marcFiles.Select(x => x.MarcRecordId.GetValueOrDefault(0)).ToList();
            if (marcRecordIds.Contains(0))
            {
                throw new Exception("GetAndBuildMarcRecordIdMarcProviderIdDicitonary found Marc Record Id that == 0. THIS SHOULD NEVER HAPPEN");
            }


            Dictionary<int, int> marcRecordIdMarcProviderIdDicitonary =
                marcFiles.ToDictionary(
                    x => x.MarcRecordId.GetValueOrDefault(),
                    y => y.MarcRecordProviderId.GetValueOrDefault(0)
                );

            var missingMarcRecordIdProviderIds = marcRecordIdMarcProviderIdDicitonary.Where(x=> x.Value == 0).ToDictionary(x => x.Key, y => y.Value);

            var missingMarcRecordProviders = marcFiles.Where(x => missingMarcRecordIdProviderIds.ContainsKey(x.MarcRecordId.GetValueOrDefault())).ToList();

            InsertMissingMarcRecordProviders(missingMarcRecordProviders, marcRecordIdMarcProviderIdDicitonary, providerTypeType);
            return marcRecordIdMarcProviderIdDicitonary;
        }

        #endregion

        #region Handles MarcRecordFiles Set/Insert/Update

        private List<int> GetFileIdsAndProviderIdsDictionary(List<int> providerIds)
        {
            List<int> missingProviderIds = new List<int>();

            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var providerIdsInClause = string.Join(",", providerIds.Select(x => $"{x}"));

            string sql = $@"
select marcRecordProviderId
from MarcRecordFile
where marcRecordProviderId in ({providerIdsInClause})
group by marcRecordProviderId";

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    missingProviderIds.Add(GetInt32Value(reader, "marcRecordProviderId", 0));
                }

                return missingProviderIds;
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


        private void InsertMissingMarcRecordFiles(List<IMarcFile> marcFiles)
        {
            var marcRecordFileDataTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecordFile");
            foreach (var marcFile in marcFiles)
            {
                DataRow row1 = marcRecordFileDataTable.NewRow();
                row1.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row1.SetField(1, 1);
                row1.SetField(2, marcFile.MrcFileText);
                marcRecordFileDataTable.Rows.Add(row1);

                DataRow row2 = marcRecordFileDataTable.NewRow();
                row2.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row2.SetField(1, 2);
                row2.SetField(2, marcFile.MrkFileText);
                marcRecordFileDataTable.Rows.Add(row2);

                DataRow row3 = marcRecordFileDataTable.NewRow();
                row3.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row3.SetField(1, 3);
                row3.SetField(2, marcFile.XmlFileText);
                marcRecordFileDataTable.Rows.Add(row3);
            }

            _sqlBulkCopyFactory.BulkInsertData("MarcRecordFile", marcRecordFileDataTable);
        }

        private List<int> InsertMarcRecordFilesAndGetUpdates(List<IMarcFile> marcFiles)
        {
            var providerIds = marcFiles.Where(x => !string.IsNullOrWhiteSpace(x.MrcFileText))
                .Select(x => x.MarcRecordProviderId.GetValueOrDefault(0)).ToList();
            if (providerIds.Contains(0))
            {
                throw new Exception(
                    "InsertMarcRecordFilesAndGetUpdates found Marc Record Provider Id that == 0. THIS SHOULD NEVER HAPPEN");
            }

            if (!providerIds.Any())
            {
                return providerIds;
            }
            var missingProviderIds = GetFileIdsAndProviderIdsDictionary(providerIds); //These Need Updates

            var missingProviderIdsForFiles =
                providerIds.Where(x => !missingProviderIds.Contains(x)).ToList(); //Insert These
            if (missingProviderIdsForFiles.Any())
            {
                var missingMarcFiles = marcFiles.Where(x =>
                    missingProviderIdsForFiles.Contains(x.MarcRecordProviderId.GetValueOrDefault())).ToList();
                InsertMissingMarcRecordFiles(missingMarcFiles);
            }

            return missingProviderIds;
        }

        #endregion

        #region Handles Provider and File Updates

        private void InsertProviderUpdates(List<IMarcFile> marcFiles)
        {
            var dateTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecordProviderUpdate");

            foreach (var marcFile in marcFiles)
            {
                DataRow row = dateTable.NewRow();


                row.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row.SetField(1, marcFile.EncodingLevel);
                row.SetField(2, marcFile.ProcessedDate);

                dateTable.Rows.Add(row);
            }

            _sqlBulkCopyFactory.BulkInsertData("MarcRecordProviderUpdate", dateTable);
        }

        private void InsertFileUpdates(List<IMarcFile> marcFiles)
        {
            var dateTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecordFileUpdate");

            foreach (var marcFile in marcFiles)
            {
                DataRow row1 = dateTable.NewRow();
                row1.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row1.SetField(1, 1);
                row1.SetField(2, marcFile.MrcFileText);
                dateTable.Rows.Add(row1);

                DataRow row2 = dateTable.NewRow();
                row2.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row2.SetField(1, 2);
                row2.SetField(2, marcFile.MrkFileText);
                dateTable.Rows.Add(row2);

                DataRow row3 = dateTable.NewRow();
                row3.SetField(0, marcFile.MarcRecordProviderId.GetValueOrDefault());
                row3.SetField(1, 3);
                row3.SetField(2, marcFile.XmlFileText);
                dateTable.Rows.Add(row3);
            }

            _sqlBulkCopyFactory.BulkInsertData("MarcRecordFileUpdate", dateTable);
        }

        private int ProcessUpdates(List<IMarcFile> marcFiles)
        {
            var providerUpdates = marcFiles.Where(x => x.IsProviderUpdate).ToList();
            var fileUpdates = marcFiles.Where(x => x.IsFileUpdate).ToList();

            int rowsUpdate = 0;


            if (providerUpdates.Any())
            {
                ExecuteTrancateTable("MarcRecordProviderUpdate", Settings.Default.RittenhouseMarcDb);
                InsertProviderUpdates(providerUpdates);
                var sql = @"
Update MarcRecordProvider
set encodingLevel = u.encodingLevel, dateUpdated = u.dateUpdated
from MarcRecordProviderUpdate u
join MarcRecordProvider mrp on mrp.marcRecordProviderId = u.marcRecordProviderId";
                rowsUpdate += ExecuteUpdateStatement(sql, new List<ISqlCommandParameter>(), true);

                Log.Debug($"MarcRecordProvider rows updated: {rowsUpdate}");
            }



            if (fileUpdates.Any())
            {
                ExecuteTrancateTable("MarcRecordFileUpdate", Settings.Default.RittenhouseMarcDb);

                InsertFileUpdates(fileUpdates);
                var sql = @"
Update MarcRecordFile
set fileData = u.fileData
from MarcRecordFileUpdate u
join MarcRecordFile mrf on mrf.marcRecordProviderId = u.marcRecordProviderId and mrf.marcRecordFileTypeId = u.marcRecordFileTypeId";
                rowsUpdate += ExecuteUpdateStatement(sql, new List<ISqlCommandParameter>(), true);

                Log.Debug($"MarcRecordFile rows updated: {rowsUpdate}");
            }


            return rowsUpdate;
        }

        #endregion

        public int InsertUpdateMarcRecordFiles(List<IMarcFile> marcFiles, MarcRecordProviderType providerType)
        {
            var skuMarcRecordIdDictionary = GetAndBuildSkuMarcRecordIdDictionary(marcFiles);
            foreach (var marcFile in marcFiles)
            {
                if (!marcFile.MarcRecordId.HasValue)
                {
                    marcFile.MarcRecordId = skuMarcRecordIdDictionary[marcFile.Product.Sku];
                }
            }

            var marcRecordIdMarcRecordProviderIdDictionary = GetAndBuildMarcRecordIdMarcProviderIdDicitonary(marcFiles, providerType);
            foreach (var marcFile in marcFiles)
            {
                if (!marcFile.MarcRecordProviderId.HasValue)
                {
                    marcFile.MarcRecordProviderId =
                        marcRecordIdMarcRecordProviderIdDictionary[marcFile.MarcRecordId.GetValueOrDefault()];
                }
                else
                {
                    marcFile.IsProviderUpdate = true;
                }
            }

            var providerIdsForFilesToUpdate = InsertMarcRecordFilesAndGetUpdates(marcFiles);
            if (providerIdsForFilesToUpdate.Any())
            {
                foreach (var marcFile in marcFiles)
                {
                    if (providerIdsForFilesToUpdate.Contains(marcFile.MarcRecordProviderId.GetValueOrDefault()))
                    {
                        marcFile.IsFileUpdate = true;
                    }
                }
            }

            int updatesProcessed = ProcessUpdates(marcFiles);
            return updatesProcessed;
        }





        public List<IMarcFile> GetProductsWithoutMarcRecords(int batchSize, MarcRecordProviderType providerType)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            List<IMarcFile> marcFiles = new List<IMarcFile>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
                .Append(FromProductAndMarcRecords((int) providerType))
                .Append(" order by mrp.DateUpdated, p.productStatusId asc, p.orderByDate, p.productId asc ")
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
                    var marcRecordId = GetInt32Value(reader, "marcRecordId");
                    var marcRecordProviderId = GetInt32Value(reader, "marcRecordProviderId");

                    switch (providerType)
                    {
                        case MarcRecordProviderType.Lc:
                            marcFiles.Add(
                                new LcMarcFile(productEntity.GetProduct(), marcRecordId, marcRecordProviderId));
                            break;
                        case MarcRecordProviderType.Nlm:
                            marcFiles.Add(new NlmMarcFile(productEntity.GetProduct(), marcRecordId,
                                marcRecordProviderId));
                            break;
                        case MarcRecordProviderType.Rbd:
                            marcFiles.Add(new RittenhouseMarcFile(productEntity.GetProduct(), marcRecordId,
                                marcRecordProviderId));
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

        public int GetProductsWithoutMarcRecordsCount(MarcRecordProviderType providerType)
        {
            string sql = new StringBuilder()
                .Append("select count(*) ")
                .Append(FromProductAndMarcRecords((int)providerType))
                .ToString();
            return ExecuteBasicCountQuery(sql, new List<ISqlCommandParameter>(), false, Settings.Default.RittenhouseMarcDb);
        }


        public List<IMarcFile> GetRittenhouseOnlyMarcFiles(int batchSize, bool missingFilesOnly)
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            List<IMarcFile> marcFiles = new List<IMarcFile>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var sqlBuilder = new StringBuilder()
                .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
                .Append(" from RittenhouseWeb.dbo.Product p ")
                .Append(" left join RittenhouseWeb.dbo.Category cat on p.categoryId = cat.categoryId ")
                .Append(" left join RittenhouseWeb.dbo.Publisher pub on p.publisherId = pub.publisherId ")
                .Append(" left join MarcRecord mr on mr.sku = p.sku ")
                .Append(" left join MarcRecordProvider mrp on mrp.MarcRecordId = mr.MarcRecordId and mrp.MarcRecordProviderTypeId = 3 ")
                .Append(" where (p.copyright is not null or p.publicationDate is not null)  and p.sku not like '%R2P%' ")
                .Append($"{(missingFilesOnly ? " and (mrp.dateCreated is null) " : "")}")
                ;

            sqlBuilder.Append(" order by mrp.DateUpdated, p.productStatusId asc, p.orderByDate, p.productId asc ");



            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sqlBuilder.ToString();
                command.CommandTimeout = 15;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductEntity productEntity = new ProductEntity();
                    productEntity.Populate(reader);
                    var marcRecordId = GetInt32Value(reader, "marcRecordId");
                    var marcRecordProviderId = GetInt32Value(reader, "marcRecordProviderId");

                    marcFiles.Add(new RittenhouseMarcFile(productEntity.GetProduct(), marcRecordId,
                        marcRecordProviderId));
                }

                return marcFiles;
            }
            catch (Exception ex)
            {
                Log.Info($"sql: {sqlBuilder}");
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
        }

        public int GetRittenhouseOnlyMarcFilesCount(bool missingFilesOnly)
        {
            var sqlBuilder = new StringBuilder()
                .Append(" select count(*) ")
                .Append(" from RittenhouseWeb.dbo.Product p ")
                .Append(" left join RittenhouseWeb.dbo.Category cat on p.categoryId = cat.categoryId ")
                .Append(" left join RittenhouseWeb.dbo.Publisher pub on p.publisherId = pub.publisherId ")
                .Append(" left join MarcRecord mr on mr.sku = p.sku ")
                .Append(" left join MarcRecordProvider mrp on mrp.MarcRecordId = mr.MarcRecordId and mrp.MarcRecordProviderTypeId = 3 ")
                .Append(" where (p.copyright is not null or p.publicationDate is not null)  and p.sku not like '%R2P%' ")
                .Append($"{(missingFilesOnly ? " and (mrp.dateCreated is null) " : "")}");

            return ExecuteBasicCountQuery(sqlBuilder.ToString(), new List<ISqlCommandParameter>(), false, Settings.Default.RittenhouseMarcDb);

        }
    }
}
