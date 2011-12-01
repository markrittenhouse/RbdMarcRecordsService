using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;
using MarcRecordServiceApp.Tasks.MarcRecords;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class MarcRecordsProductFactory : FactoryBase
    {
		/// <summary>
		/// This string contains all product related fields except synopsis.  Use PopulateProduct() to populate
		/// </summary>
		public static readonly string ProductSelectFields = new StringBuilder()
			.Append("p.productId, p.sku, p.isbn10, p.isbn13, p.title, p.subTitle, p.authors, p.firstAuthorLastName, p.productStatusId ")
			.Append(", p.affiliation, p.copyright, p.publicationDate, p.format, p.edition, p.pages, p.weight, p.length ")
			.Append(", p.width, p.thickness, p.cartonQuantity, p.serialName, p.lcCallNumber, p.nlm, p.announcedEdition ")
			.Append(", p.announcedDate, p.newEditionIsbn, p.previousEditionIsbn, p.alternateIsbn, p.brandonHillCodes ")
			.Append(", p.priceGroup, p.venderNumber, p.venderInfo, p.isReturnable, p.isGsa, p.productType ")
			.Append(", p.audiancePrimary, p.audianceSecondary, p.imprint, p.doodyRating, p.publisherPrelude, p.orderBydate ")
			.Append(", p.brandonHillOrderBy, p.r2ResourceId, p.countryCode, p.eIsbn, p.isAvailableForSale, p.availabilityCode, p.tocFilename  ")
			.Append(", i.quantityOnHand, i.quantityOnOrder, i.inStockOverride ")
			.Append(", pp.priceList, pp.priceNet, pp.price2, pp.price3, pp.price4, pp.priceFuture ")
			.Append(", cat.categoryId, cat.categoryName, cat.categoryCode ")
			.Append(", pub.publisherId, pub.publisherName ")
			.Append(", pci.fileName ")
			.ToString();

        public static readonly string SelectMarcRecordandProviderFields = new StringBuilder()
            .Append("  , mr.MarcRecordId, mr.Isbn10, mr.Isbn13, mr.sku, mrp.MarcRecordProviderId, mrp.MarcRecordId,  ")
            .Append(" mrp.MarcRecordProviderTypeId, mrp.EncodingLevel, mrp.DateCreated, mrp.DateUpdated  ")
            .ToString();

        public static readonly string FromProductAndMarcRecords = new StringBuilder()
            .Append("from   RittenhouseWeb.dbo.Product p ")
            .Append(" join  RittenhouseWeb.dbo.Inventory i on i.productId = p.productId ")
            .Append(" join  RittenhouseWeb.dbo.ProductPrice pp on pp.productId = p.productId ")
            .Append(" left join  RittenhouseWeb.dbo.Category cat on p.categoryId = cat.categoryId ")
            .Append(" left join  RittenhouseWeb.dbo.Publisher pub on p.publisherId = pub.publisherId ")
            .Append(" left join  RittenhouseWeb.dbo.ProductCoverImage pci on p.productId = pci.productId ")
            .Append(" left join dbo.MarcRecord mr on mr.sku = p.sku ")
            .Append(" left join dbo.MarcRecordProvider mrp on mrp.MarcRecordId = mr.MarcRecordId ")
            .Append(" and mrp.MarcRecordProviderTypeId = {0} ")
            .Append(" where p.isAvailableForSale = 1 ")
            .Append(" and  p.productStatusId in (1, 2, 4, 5, 8) ")
            .Append(" and  (p.copyright is not null or p.publicationDate is not null) ")
            .Append(" and  (mrp.DateUpdated is null or mrp.DateUpdated < getdate() - 7) ")          
            .ToString();


            
        ///// <summary>
        ///// Old Marc Record Count
        ///// </summary>
        ///// <param name="batchSize"></param>
        ///// <returns></returns>
        //public static Product[] GetProductsWithoutMarcRecords(int batchSize)
        //{
        //    SqlConnection cnn = null;
        //    SqlCommand command = null;
        //    SqlDataReader reader = null;

        //    List<Product> products = new List<Product>();

        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    string sql = new StringBuilder()
        //    .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
        //    .Append("from   dbo.Product p ")
        //    .Append(" join  dbo.Inventory i on i.productId = p.productId ")
        //    .Append(" join  dbo.ProductPrice pp on pp.productId = p.productId ")
        //    .Append(" left join  dbo.Category cat on p.categoryId = cat.categoryId ")
        //    .Append(" left join  dbo.Publisher pub on p.publisherId = pub.publisherId ")
        //    .Append(" left join  dbo.ProductCoverImage pci on p.productId = pci.productId ")
        //    .Append("where  p.productId not in (select pmr.productId from ProductMarcRecord pmr) ")
        //    .Append("  and  p.isAvailableForSale = 1 ")             // only process titles available for sale
        //    .Append("  and  p.productStatusId not in (3, 6, 7) ")   // don't process OP, PC or PI
        //    .Append("  and  (p.copyright is not null or p.publicationDate is not null) ")       // only process if a copyright or publication date is available
        //    .Append("order by orderByDate ")    // process oldest records first
        //    .ToString();

        //    try
        //    {
        //        cnn = GetRittenhouseConnection();

        //        command = cnn.CreateCommand();
        //        command.CommandText = sql;
        //        command.CommandTimeout = 15;

        //        reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            //Product product = PopulateProduct(reader, false);
        //            //products.Add(product);
        //            ProductEntity productEntity = new ProductEntity();
        //            productEntity.Populate(reader);
        //            products.Add(productEntity.GetProduct());
        //        }
        //        return products.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InfoFormat("sql: {0}", sql);
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        DisposeConnections(cnn, command, reader);
        //    }
        //}

        ///// <summary>
        ///// Old Marc Record Count
        ///// </summary>
        ///// <returns></returns>
        //public static int GetProductsWithoutMarcRecordsCount()
        //{
        //    SqlConnection cnn = null;
        //    SqlCommand command = null;
        //    SqlDataReader reader = null;

        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    string sql = new StringBuilder()
        //    .Append("select count(*) ")
        //    .Append("from   dbo.Product p ")
        //    .Append(" join  dbo.Inventory i on i.productId = p.productId ")
        //    .Append(" join  dbo.ProductPrice pp on pp.productId = p.productId ")
        //    .Append(" left join  dbo.Category cat on p.categoryId = cat.categoryId ")
        //    .Append(" left join  dbo.Publisher pub on p.publisherId = pub.publisherId ")
        //    .Append(" left join  dbo.ProductCoverImage pci on p.productId = pci.productId ")
        //    .Append("where  p.isbn13 not in (select isbn13 from MarcRecords.dbo.MarcRecord) ")
        //    .Append("  and  p.isAvailableForSale = 1 ")             // only process titles available for sale
        //    .Append("  and  p.productStatusId not in (3, 6, 7) ")   // don't process OP, PC or PI
        //    .Append("  and  (p.copyright is not null or p.publicationDate is not null) ")       // only process if a copyright or publication date is available
        //    .ToString();

        //    try
        //    {
        //        cnn = GetRittenhouseConnection();

        //        command = cnn.CreateCommand();
        //        command.CommandText = sql;
        //        command.CommandTimeout = 15;

        //        reader = command.ExecuteReader();

        //        int count = -2;
        //        if (reader.Read())
        //        {
        //            count = GetInt32Value(reader, 0, -1);
        //        }
        //        return count;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InfoFormat("sql: {0}", sql);
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        DisposeConnections(cnn, command, reader);
        //    }
        //}

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
            .AppendFormat(FromProductAndMarcRecords, (int)providerType)
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
                .AppendFormat(FromProductAndMarcRecords, (int)providerType)
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="batchSize"></param>
        ///// <returns></returns>
        //public static Product[] GetProductsWithMarcRecordsForUpdate(int batchSize)
        //{
        //    SqlConnection cnn = null;
        //    SqlCommand command = null;
        //    SqlDataReader reader = null;

        //    List<Product> products = new List<Product>();

        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    string sql = new StringBuilder()
        //    .AppendFormat("select top {0} ", batchSize).Append(ProductSelectFields)
        //    .Append("from   dbo.Product p ")
        //    .Append(" join  dbo.Inventory i on i.productId = p.productId ")
        //    //.Append(" join  dbo.ProductStatus status on p.productStatusId = status.productStatusId ")
        //    .Append(" join  dbo.ProductPrice pp on pp.productId = p.productId ")
        //    .Append(" left join dbo.Category cat on p.categoryId = cat.categoryId ")
        //    .Append(" left join dbo.Publisher pub on p.publisherId = pub.publisherId ")
        //    .Append(" left join dbo.ProductCoverImage pci on p.productId = pci.productId ")
        //    .Append(" join  dbo.ProductMarcRecord pmr on p.productId = pmr.productId ")
        //    .Append("where  p.isAvailableForSale = 1 ")             // only process titles available for sale
        //    .Append("  and  p.productStatusId not in (3, 6, 7) ")   // don't process OP, PC or PI
        //    .Append("  and  (p.copyright is not null or p.publicationDate is not null) ")       // only process if a copyright or publication date is available
        //    .Append("  and  p.dateUpdated > pmr.dateCreated ")      // records that need to be updated.
        //    .Append("order by orderByDate ")    // process oldest records first
        //    .ToString();

        //    try
        //    {
        //        cnn = GetRittenhouseConnection();

        //        command = cnn.CreateCommand();
        //        command.CommandText = sql;
        //        command.CommandTimeout = 15;

        //        reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            //Product product = PopulateProduct(reader, false);
        //            ProductEntity productEntity = new ProductEntity();
        //            productEntity.Populate(reader);
        //            Product product = productEntity.GetProduct();

        //            products.Add(product);
        //        }
        //        return products.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InfoFormat("sql: {0}", sql);
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        DisposeConnections(cnn, command, reader);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static int GetProductsWithMarcRecordsCountForUpdate()
        //{
        //    SqlConnection cnn = null;
        //    SqlCommand command = null;
        //    SqlDataReader reader = null;

        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    string sql = new StringBuilder()
        //    .Append("select count(*) ")
        //    .Append("from   dbo.Product p ")
        //    .Append(" join  dbo.Inventory i on i.productId = p.productId ")
        //    .Append(" join  dbo.ProductPrice pp on pp.productId = p.productId ")
        //    .Append(" left join dbo.Category cat on p.categoryId = cat.categoryId ")
        //    .Append(" left join dbo.Publisher pub on p.publisherId = pub.publisherId ")
        //    .Append(" left join dbo.ProductCoverImage pci on p.productId = pci.productId ")
        //    .Append(" join  dbo.ProductMarcRecord pmr on p.productId = pmr.productId ")
        //    .Append("where  p.isAvailableForSale = 1 ")             // only process titles available for sale
        //    .Append("  and  p.productStatusId not in (3, 6, 7) ")   // don't process OP, PC or PI
        //    .Append("  and  (p.copyright is not null or p.publicationDate is not null) ")       // only process if a copyright or publication date is available
        //    .Append("  and  p.dateUpdated > pmr.dateCreated ")      // records that need to be updated.
        //    .ToString();

        //    try
        //    {
        //        cnn = GetRittenhouseConnection();

        //        command = cnn.CreateCommand();
        //        command.CommandText = sql;
        //        command.CommandTimeout = 15;

        //        reader = command.ExecuteReader();

        //        int count = -2;
        //        if (reader.Read())
        //        {
        //            count = GetInt32Value(reader, 0, -1);
        //        }
        //        return count;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InfoFormat("sql: {0}", sql);
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        DisposeConnections(cnn, command, reader);
        //    }
        //}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="productId"></param>
		///// <param name="mrkFileText"></param>
		///// <param name="mrcFileText"></param>
		///// <returns></returns>
		//public static int InsertProductmarcRecord(int productId, string mrkFileText, string mrcFileText)
		//{
		//    StringBuilder sql = new StringBuilder()
		//        .Append("insert into dbo.ProductMarcRecord (productId, dateCreated, mrkFileText, mrcFileText) ")
		//        .Append("values (@ProductId, getdate(), @MrkFileText, @MrcFileText)");

		//    List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
		//                                                {
		//                                                    new Int32Parameter("ProductId", productId),
		//                                                    new StringParameter("MrkFileText", mrkFileText),
		//                                                    new StringParameter("MrcFileText", mrcFileText)
		//                                                };

		//    int rowCount = ExecuteUpdateStatement(sql.ToString(), parameters.ToArray(), false, Settings.Default.RittenhouseWebDb);
		//    return rowCount;
		//}

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
                .Append(" inner join MarcRecordFile mrf ON mrf.marcRecordProviderId = mrp.marcRecordProviderId ")
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
                    recordCount = UpdateMarcRecordFile(marcFile, marcRecordProvider);
                }
            }
            else
            {
                marcRecordProvider = InsertMarcRecordProvider(marcFile, marcRecordId);
                if (marcFile.MrcFileText != null)
                {
                    recordCount = InsertMarcRecordFile(marcFile, marcRecordProvider);
                }                  
            }
            return recordCount;
        }

        //public static int InsertProductMarcRecordFile(IMarcFile marcFile)
        //{
        //    int marcRecordId = GetProductMarcRecordId(marcFile);
        //    StringBuilder sql = new StringBuilder()

        //    .Append(" insert into MarcRecordProvider (MarcRecordId, MarcRecordProviderTypeId, EncodingLevel, DateCreated, dateUpdated) ")
        //    .Append(" VALUES (@MarcRecordId, @MarcRecordProviderTypeId, @EncodingLevel, @DateCreated, @DateUpdated) ")
        //    ;
        //    List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
        //                                                {
        //                                                    new Int32Parameter("MarcRecordId", marcRecordId),
        //                                                    new Int32Parameter("MarcRecordProviderTypeId", (int)marcFile.RecordProvider),
        //                                                    new DateTimeParameter("DateCreated",marcFile.ProcessedDate),
        //                                                    new DateTimeParameter("DateUpdated",marcFile.ProcessedDate),
        //                                                    new StringParameter("EncodingLevel", marcFile.EncodingLevel)
        //                                                };
        //    int marcRecordProviderId = ExecuteInsertStatementReturnIdentity(sql.ToString(), parameters.ToArray(), true, Properties.Settings.Default.RittenhouseMarcDb);
        //    int rowCount = 0;
        //    if (marcFile.MrcFileText != null)
        //    {
        //        sql = new StringBuilder()
        //            .Append(" insert into MarcRecordFile (MarcRecordProviderId, MarcRecordFileTypeId, FileData) ")
        //            .Append(" VALUES (@MarcRecordProviderId, @MarcRecordFileTypeId1, @FileData1); ")
        //            .Append(" insert into MarcRecordFile (MarcRecordProviderId, MarcRecordFileTypeId, FileData) ")
        //            .Append(" VALUES (@MarcRecordProviderId, @MarcRecordFileTypeId2, @FileData2); ")
        //            .Append(" insert into MarcRecordFile (MarcRecordProviderId, MarcRecordFileTypeId, FileData) ")
        //            .Append(" VALUES (@MarcRecordProviderId, @MarcRecordFileTypeId3, @FileData3); ");

        //        parameters = new List<ISqlCommandParameter>
        //                                                {
        //                                                    new Int32Parameter("MarcRecordProviderId", marcRecordProviderId),
        //                                                    new Int32Parameter("MarcRecordFileTypeId1", 1),
        //                                                    new Int32Parameter("MarcRecordFileTypeId2", 2),
        //                                                    new Int32Parameter("MarcRecordFileTypeId3", 3),
        //                                                    new StringParameter("FileData1", marcFile.MrcFileText),
        //                                                    new StringParameter("FileData2", marcFile.MrkFileText),
        //                                                    new StringParameter("FileData3", marcFile.XmlFileText)
        //                                                };

        //        rowCount = ExecuteUpdateStatement(sql.ToString(), parameters.ToArray(), false, Properties.Settings.Default.RittenhouseMarcDb);
        //    }
            

        //    return rowCount;
        //}
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
            int rowCount = 0;
            if (marcFile.MrcFileText != null)
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

                rowCount = ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
            }
            return rowCount;
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
       

		///// <summary>
		///// 
		///// </summary>
		///// <param name="productId"></param>
		///// <param name="mrkFileText"></param>
		///// <param name="mrcFileText"></param>
		///// <returns></returns>
		//public static int UpdateProductmarcRecord(int productId, string mrkFileText, string mrcFileText)
		//{
		//    StringBuilder sql = new StringBuilder()
		//        .Append("update dbo.ProductMarcRecord set dateCreated = getdate(), mrkFileText = @MrkFileText, mrcFileText = @MrcFileText ")
		//        .Append("where productId = @ProductId; ");

		//    List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
		//                                                {
		//                                                    new Int32Parameter("ProductId", productId),
		//                                                    new StringParameter("MrkFileText", mrkFileText),
		//                                                    new StringParameter("MrcFileText", mrcFileText)
		//                                                };

		//    int rowCount = ExecuteUpdateStatement(sql.ToString(), parameters.ToArray(), true, Settings.Default.RittenhouseWebDb);
		//    return rowCount;
		//}


    }
}
