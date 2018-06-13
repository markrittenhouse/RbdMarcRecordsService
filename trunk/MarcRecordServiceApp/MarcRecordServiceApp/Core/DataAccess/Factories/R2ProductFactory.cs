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
    public class R2ProductFactory : FactoryBase
    {
        private SqlBulkCopyFactory _sqlBulkCopyFactory;

        public R2ProductFactory()
        {
            _sqlBulkCopyFactory = new SqlBulkCopyFactory();
        }
        private string GetAllResources()
        {
            return new StringBuilder()
                .Append("select r.iResourceId, r.vchResourceISBN, r.vchIsbn10, r.vchIsbn13, r.vchEIsbn ")
                .Append(", r.vchResourceTitle, r.vchResourceSubTitle, r.vchResourceAuthors, r.vchResourceSortAuthor ")
                .Append(", r.dtResourcePublicationDate ")
                .Append(", isnull(pub.vchPublisherName, pub2.vchPublisherName) as vchPublisherName ")
                .AppendFormat("from {0}..tResource r ", Settings.Default.R2DatabaseName)
                .AppendFormat("join {0}..tPublisher pub2 on r.iPublisherId = pub2.iPublisherId ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tPublisher pub on pub2.iConsolidatedPublisherId = pub.iPublisherId ", Settings.Default.R2DatabaseName)
                .ToString();
        }
        private string GetWebR2LibraryMarcRecordsThatDoNotExist()
        {
            return new StringBuilder()
                .Append("select r.iResourceId, r.vchResourceISBN, r.vchIsbn10, r.vchIsbn13, r.vchEIsbn ")
                .Append(", r.vchResourceTitle, r.vchResourceSubTitle, r.vchResourceAuthors, r.vchResourceSortAuthor ")
                .Append(", r.dtResourcePublicationDate ")
                .Append(", isnull(pub.vchPublisherName, pub2.vchPublisherName) as vchPublisherName ")
                .AppendFormat("from {0}..tResource r ", Settings.Default.R2DatabaseName)
                .AppendFormat("join {0}..tPublisher pub2 on r.iPublisherId = pub2.iPublisherId ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tPublisher pub on pub2.iConsolidatedPublisherId = pub.iPublisherId ", Settings.Default.R2DatabaseName)
                .Append("where  r.tiRecordStatus = 1 and r.vchResourceISBN not in (select isbn from WebR2LibraryMarcRecords)")
                //.Append("select r.vchResourceISBN ")
                //.Append("from MarcRecordFile mrf ")
                //.Append("join MarcRecordProvider mrp on mrf.marcRecordProviderId = mrp.marcRecordProviderId ")
                //.Append("join MarcRecordProviderType mrpt on mrpt.marcRecordProviderTypeId = mrp.marcRecordProviderTypeId ")
                //.Append("and mrpt.marcRecordProviderTypeId in (1,2) ")
                //.Append("join MarcRecord mr on mr.marcRecordId = mrp.marcRecordId ")
                //.AppendFormat("join {0}..tResource r on mr.isbn10 = r.vchIsbn10 ", Settings.Default.R2DatabaseName)
                //.Append("where marcRecordFileTypeId = 2) ")
                .ToString();
        }

        private string Sql_GetOclcResourcesThatDoNotExistOrUpdated()
        {
            return new StringBuilder()
                .Append("select r.iResourceId, r.vchResourceISBN, r.vchIsbn10, r.vchIsbn13, r.vchEIsbn ")
                .Append(", r.vchResourceTitle, r.vchResourceSubTitle, r.vchResourceAuthors, r.vchResourceSortAuthor ")
                .Append(", r.dtResourcePublicationDate ")
                .Append(", isnull(pub.vchPublisherName, pub2.vchPublisherName) as vchPublisherName ")
                .AppendFormat("from {0}..tResource r ", Settings.Default.R2DatabaseName)
                .AppendFormat("join {0}..tPublisher pub2 on r.iPublisherId = pub2.iPublisherId ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tPublisher pub on pub2.iConsolidatedPublisherId = pub.iPublisherId ", Settings.Default.R2DatabaseName)
                .Append("where  r.tiRecordStatus = 1 and r.vchResourceISBN not in ( ")
                .Append("select ormr.isbn ")
                .Append("from OclcR2LibraryMarcRecords ormr ")
                .AppendFormat("where (ormr.updatedDate is null and ormr.createdDate > getdate()-{0}) or (ormr.updatedDate > getdate()-{0})) ", Settings.Default.OclcGetRecordsOlderThanDays)
                .ToString();
        }

        private string GetAllResourcesCategories()
        {
            var sql = new StringBuilder()
                .Append("select r.iResourceId, pa.vchPracticeAreaName ")
                .AppendFormat("from {0}..tResource r ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tResourcePracticeArea rpa on rpa.iResourceId = r.iResourceId and rpa.tiRecordStatus = 1 ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tPracticeArea pa on pa.iPracticeAreaId = rpa.iPracticeAreaId and pa.tiRecordStatus= 1 ", Settings.Default.R2DatabaseName)
                .ToString();
            return sql;
        }
        private string GetAllResourcesSubCategories()
        {
            var sql = new StringBuilder()
                .Append("select r.iResourceId, s.vchSpecialtyName ")
                .AppendFormat("from {0}..tResource r ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tResourceSpecialty rs on rs.iResourceId = r.iResourceId and rs.tiRecordStatus = 1 ", Settings.Default.R2DatabaseName)
                .AppendFormat("left join {0}..tSpecialty s on rs.iSpecialtyId = s.iSpecialtyId and s.tiRecordStatus = 1 ", Settings.Default.R2DatabaseName)
                .ToString();
            return sql;
        }
        private string GetAllResourcesAuthors()
        {
            var sql = new StringBuilder()
                .Append("select iResourceId, vchFirstName, vchLastName, vchMiddleName, vchDegree, tiAuthorOrder ")
                .AppendFormat("from {0}..tAuthor ", Settings.Default.R2DatabaseName)
                .ToString();
            return sql;
        }

        private readonly string _additionalDataFieldsQuery = $@"
select r.vchIsbn13 as sku, amf.marcValue, amf.fieldNumber, amf.marcRecordId
from {Settings.Default.R2DatabaseName}..tResource r
join MarcRecord mr on r.vchIsbn13 = mr.isbn13
join AdditionalMarcField amf on mr.marcRecordId = amf.marcRecordId

";

        private string GetNlmAndLcMarcRecords()
        {
            return new StringBuilder()

                .Append("select r.vchResourceISBN, r.vchisbn10, r.vchisbn13, r.vcheIsbn, mrf.fileData, mrpt.marcRecordProviderTypeId ")
                .Append("from MarcRecordFile mrf ")
                .Append("join MarcRecordProvider mrp on mrf.marcRecordProviderId = mrp.marcRecordProviderId ")
                .Append("join MarcRecordProviderType mrpt on mrpt.marcRecordProviderTypeId = mrp.marcRecordProviderTypeId and mrpt.marcRecordProviderTypeId in (2) ")
                .Append("join MarcRecord mr on mr.marcRecordId = mrp.marcRecordId ")
                .AppendFormat("join {0}..tResource r on mr.isbn10 = r.vchIsbn10 and r.tiRecordStatus = 1 ", Settings.Default.R2DatabaseName)
                .Append("left join ExcludedMarcRecord emr on mr.sku = emr.sku and mrpt.marcRecordProviderTypeId = emr.marcRecordProviderTypeId ")
                .Append("where marcRecordFileTypeId = 2 and emr.excludedMarcRecordId is null ")
                .Append("union ")
                .Append("select r.vchResourceISBN, r.vchisbn10, r.vchisbn13, r.vcheIsbn, mrf.fileData, mrpt.marcRecordProviderTypeId ")
                .Append("from MarcRecordFile mrf ")
                .Append("join MarcRecordProvider mrp on mrf.marcRecordProviderId = mrp.marcRecordProviderId ")
                .Append("join MarcRecordProviderType mrpt on mrpt.marcRecordProviderTypeId = mrp.marcRecordProviderTypeId and mrpt.marcRecordProviderTypeId in (1) ")
                .Append("join MarcRecord mr on mr.marcRecordId = mrp.marcRecordId ")
                .AppendFormat("join {0}..tResource r on mr.isbn10 = r.vchIsbn10 and r.tiRecordStatus = 1 ", Settings.Default.R2DatabaseName)
                .Append("left join ExcludedMarcRecord emr on mr.sku = emr.sku and mrpt.marcRecordProviderTypeId = emr.marcRecordProviderTypeId ")
                .Append("where marcRecordFileTypeId = 2 and emr.excludedMarcRecordId is null and r.vchResourceISBN not in ")
                .Append("(select r.vchResourceISBN ")
                .Append("from MarcRecordFile mrf ")
                .Append("join MarcRecordProvider mrp on mrf.marcRecordProviderId = mrp.marcRecordProviderId ")
                .Append("join MarcRecordProviderType mrpt on mrpt.marcRecordProviderTypeId = mrp.marcRecordProviderTypeId and mrpt.marcRecordProviderTypeId in (2) ")
                .Append("join MarcRecord mr on mr.marcRecordId = mrp.marcRecordId ")
                .AppendFormat("join {0}..tResource r on mr.isbn10 = r.vchIsbn10 and r.tiRecordStatus = 1 ", Settings.Default.R2DatabaseName)
                .Append("left join ExcludedMarcRecord emr on mr.sku = emr.sku and mrpt.marcRecordProviderTypeId = emr.marcRecordProviderTypeId ")
                .Append("where marcRecordFileTypeId = 2  and emr.excludedMarcRecordId is null) ")
                .ToString();
        }

        public List<R2Resource> GetAllR2Resource()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append(GetAllResources())
                .Append("; ")
                .Append(GetAllResourcesAuthors())
                .Append("; ")
                .Append(GetAllResourcesCategories())
                .Append("; ")
                .Append(GetAllResourcesSubCategories())
                .Append("; ")
                .Append(_additionalDataFieldsQuery)
                .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 60;

                List<R2Resource> r2Resources = new List<R2Resource>();
                List<R2Author> r2Authors = new List<R2Author>();
                List<R2Category> r2Categories = new List<R2Category>();
                List<R2SubCategory> r2SubCategories = new List<R2SubCategory>();
                List<AdditionalField> addionalFields = new List<AdditionalField>();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    R2Resource resource = new R2Resource();
                    resource.Populate(reader);
                    r2Resources.Add(resource);
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2Author r2Author = new R2Author();
                        r2Author.Populate(reader);
                        r2Authors.Add(r2Author);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2Category r2Category = new R2Category();
                        r2Category.Populate(reader);
                        r2Categories.Add(r2Category);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2SubCategory r2SubCategory = new R2SubCategory();
                        r2SubCategory.Populate(reader);
                        r2SubCategories.Add(r2SubCategory);
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        AdditionalField additionalField = new AdditionalField();
                        additionalField.Populate(reader);
                        addionalFields.Add(additionalField);
                    }
                }

                foreach (R2Resource r2Resource in r2Resources)
                {
                    int resourceId = r2Resource.ResourceId;
                    r2Resource.AuthorList = r2Authors.Where(x => x.ResourceId == resourceId);
                    r2Resource.Categories = r2Categories.Where(x => x.ResourceId == resourceId);
                    r2Resource.SubCategories = r2SubCategories.Where(x => x.ResourceId == resourceId);
                    r2Resource.AdditionalFields = addionalFields.Where(x => x.Sku == r2Resource.Isbn13).ToList();
                }

                return r2Resources;
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

        public List<R2Resource> GetR2ResourcesThatDoNotExist()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append(GetWebR2LibraryMarcRecordsThatDoNotExist())
                .Append("; ")
                .Append(GetAllResourcesAuthors())
                .Append("; ")
                .Append(GetAllResourcesCategories())
                .Append("; ")
                .Append(GetAllResourcesSubCategories())
                .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 60;

                List<R2Resource> r2Resources = new List<R2Resource>();
                List<R2Author> r2Authors = new List<R2Author>();
                List<R2Category> r2Categories = new List<R2Category>();
                List<R2SubCategory> r2SubCategories = new List<R2SubCategory>();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    R2Resource resource = new R2Resource();
                    resource.Populate(reader);
                    r2Resources.Add(resource);
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2Author r2Author = new R2Author();
                        r2Author.Populate(reader);
                        r2Authors.Add(r2Author);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2Category r2Category = new R2Category();
                        r2Category.Populate(reader);
                        r2Categories.Add(r2Category);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2SubCategory r2SubCategory = new R2SubCategory();
                        r2SubCategory.Populate(reader);
                        r2SubCategories.Add(r2SubCategory);
                    }
                }

                foreach (var r2Resource in r2Resources)
                {
                    int resourceId = r2Resource.ResourceId;
                    r2Resource.AuthorList = r2Authors.Where(x => x.ResourceId == resourceId);
                    r2Resource.Categories = r2Categories.Where(x => x.ResourceId == resourceId);
                    r2Resource.SubCategories = r2SubCategories.Where(x => x.ResourceId == resourceId);
                }

                return r2Resources;
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

        public List<R2Resource> GetOclcR2ResourcesThatDoNotExistOrNeedUpdated()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = new StringBuilder()
                .Append(Sql_GetOclcResourcesThatDoNotExistOrUpdated())
                .Append("; ")
                .Append(GetAllResourcesAuthors())
                .Append("; ")
                .Append(GetAllResourcesCategories())
                .Append("; ")
                .Append(GetAllResourcesSubCategories())
                .ToString();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 60;

                List<R2Resource> r2Resources = new List<R2Resource>();
                List<R2Author> r2Authors = new List<R2Author>();
                List<R2Category> r2Categories = new List<R2Category>();
                List<R2SubCategory> r2SubCategories = new List<R2SubCategory>();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    R2Resource resource = new R2Resource();
                    resource.Populate(reader);
                    r2Resources.Add(resource);
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2Author r2Author = new R2Author();
                        r2Author.Populate(reader);
                        r2Authors.Add(r2Author);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2Category r2Category = new R2Category();
                        r2Category.Populate(reader);
                        r2Categories.Add(r2Category);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        R2SubCategory r2SubCategory = new R2SubCategory();
                        r2SubCategory.Populate(reader);
                        r2SubCategories.Add(r2SubCategory);
                    }
                }

                foreach (var r2Resource in r2Resources)
                {
                    int resourceId = r2Resource.ResourceId;
                    r2Resource.AuthorList = r2Authors.Where(x => x.ResourceId == resourceId);
                    r2Resource.Categories = r2Categories.Where(x => x.ResourceId == resourceId);
                    r2Resource.SubCategories = r2SubCategories.Where(x => x.ResourceId == resourceId);
                }

                return r2Resources;
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

        public List<R2LibraryMarcFile> GetNlmAndLcMarcFiles()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string sql = GetNlmAndLcMarcRecords();

            try
            {
                cnn = GetRittenhouseConnection();

                command = cnn.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = 60;

                List<R2LibraryMarcFile> r2LibraryMarcFiles = new List<R2LibraryMarcFile>();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    R2LibraryMarcFile marcFile = new R2LibraryMarcFile();
                    marcFile.MrkText = GetStringValue(reader, "fileData");
                    marcFile.ProviderSourceId = GetInt32Value(reader, "marcRecordProviderTypeId", 0);
                    marcFile.Isbn = GetStringValue(reader, "vchResourceIsbn");
                    marcFile.Isbn10 = GetStringValue(reader, "vchIsbn10");
                    marcFile.Isbn13 = GetStringValue(reader, "vchIsbn13");
                    marcFile.EIsbn = GetStringValue(reader, "vchEIsbn");
                    r2LibraryMarcFiles.Add(marcFile);
                }

                return r2LibraryMarcFiles;
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

        public int TruncateWebR2LibraryMarcRecords()
        {
            int rowCount = ExecuteTruncateTable("WebR2LibraryMarcRecords", Settings.Default.RittenhouseMarcDb);
            return rowCount;
        }

        public int InsertWebR2LibraryMarcRecords2(List<R2LibraryMarcFile> r2LibraryMarcFiles)
        {
            int rowCount = 0;
            var dataTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("WebR2LibraryMarcRecords");

            foreach (var r2LibraryMarcFile in r2LibraryMarcFiles)
            {
                //isbn, isbn10, isbn13, eIsbn, marcRecordProviderTypeId, fileData, createdDate
                DataRow row = dataTable.NewRow();

                row.SetField(0, r2LibraryMarcFile.Isbn);
                row.SetField(1, r2LibraryMarcFile.Isbn10);
                row.SetField(2, r2LibraryMarcFile.Isbn13);
                row.SetField(3, r2LibraryMarcFile.EIsbn);
                row.SetField(4, r2LibraryMarcFile.ProviderSourceId);
                row.SetField(5, r2LibraryMarcFile.MrkText);
                row.SetField(6, r2LibraryMarcFile.CreatedDate);

                dataTable.Rows.Add(row);
                rowCount++;
            }

            _sqlBulkCopyFactory.BulkInsertData("WebR2LibraryMarcRecords", dataTable);
            return rowCount;
        }


        public int InsertWebR2LibraryMarcRecords(List<R2LibraryMarcFile> r2LibraryMarcFiles)
        {
            try
            {
                int totalInserted = 0;

                foreach (R2LibraryMarcFile r2LibraryMarcFile in r2LibraryMarcFiles)
                {
                    string sql = new StringBuilder()
                        .Append(" insert into WebR2LibraryMarcRecords (isbn, isbn10, isbn13, eIsbn, marcRecordProviderTypeId, fileData, createdDate) ")
                        .Append(" VALUES (@Isbn, @Isbn10, @Isbn13, @EIsbn, @ProviderSourceId, @MrkText, @CreatedDate); ")
                        .ToString()
                        ;
                    List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                                        {
                                                                            new StringParameter("Isbn", r2LibraryMarcFile.Isbn),
                                                                            new StringParameter("Isbn10", r2LibraryMarcFile.Isbn10),
                                                                            new StringParameter("Isbn13", r2LibraryMarcFile.Isbn13),
                                                                            new StringParameter("EIsbn", r2LibraryMarcFile.EIsbn),
                                                                            new Int32Parameter("ProviderSourceId", r2LibraryMarcFile.ProviderSourceId),
                                                                            new StringParameter("MrkText", r2LibraryMarcFile.MrkText),
                                                                            new DateTimeParameter("CreatedDate", r2LibraryMarcFile.CreatedDate)
                                                                        };
                    var rowCount = ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
                    totalInserted += rowCount;
                }
                return totalInserted;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        public int InsertOclcR2LibraryMarcRecords(List<R2LibraryMarcFile> r2LibraryMarcFiles)
        {
            try
            {
                int totalInserted = 0;

                foreach (R2LibraryMarcFile r2LibraryMarcFile in r2LibraryMarcFiles)
                {
                    string sql = new StringBuilder()
                        .Append(" insert into OclcR2LibraryMarcRecords (isbn, isbn10, isbn13, eIsbn, marcRecordProviderTypeId, fileData, createdDate) ")
                        .Append(" VALUES (@Isbn, @Isbn10, @Isbn13, @EIsbn, @ProviderSourceId, @MrkText, @CreatedDate); ")
                        .ToString()
                        ;
                    List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                                        {
                                                                            new StringParameter("Isbn", r2LibraryMarcFile.Isbn),
                                                                            new StringParameter("Isbn10", r2LibraryMarcFile.Isbn10),
                                                                            new StringParameter("Isbn13", r2LibraryMarcFile.Isbn13),
                                                                            new StringParameter("EIsbn", r2LibraryMarcFile.EIsbn),
                                                                            new Int32Parameter("ProviderSourceId", r2LibraryMarcFile.ProviderSourceId),
                                                                            new StringParameter("MrkText", r2LibraryMarcFile.MrkText),
                                                                            new DateTimeParameter("CreatedDate", r2LibraryMarcFile.CreatedDate)
                                                                        };
                    var rowCount = ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
                    totalInserted += rowCount;
                }
                return totalInserted;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        public bool CreateAllR2LibraryMarcRecords()
        {
            var sql = new StringBuilder()
                .Append(" select count(*) from WebR2LibraryMarcRecords ")
                .ToString();
            var rowCount = ExecuteBasicCountQuery(sql, null, false, Settings.Default.RittenhouseMarcDb);
            return rowCount == 0;
        }

        public bool CreateAllOclcR2LibraryMarcRecords()
        {
            var sql = new StringBuilder()
                .Append(" select count(*) from OclcR2LibraryMarcRecords ")
                .ToString();
            var rowCount = ExecuteBasicCountQuery(sql, null, false, Settings.Default.RittenhouseMarcDb);
            return rowCount == 0;
        }

        public void UpdateWebR2LibraryMarcRecords(List<R2LibraryMarcFile> r2LibraryMarcFiles, out int totalUpdated, out int totalInserted)
        {
            try
            {
                totalUpdated = 0;
                totalInserted = 0;

                List<R2LibraryMarcFile> recordsNotFound = new List<R2LibraryMarcFile>();

                foreach (R2LibraryMarcFile r2LibraryMarcFile in r2LibraryMarcFiles)
                {
                    var sql = new StringBuilder()
                        .Append(" update WebR2LibraryMarcRecords ")
                        .Append(" set isbn10 = @Isbn10 ")
                        .Append(" , isbn13 = @Isbn13 ")
                        .Append(" , eIsbn = @EIsbn ")
                        .Append(" , marcRecordProviderTypeId = @ProviderSourceId ")
                        .Append(" , fileData = @MrkText ")
                        .Append(" , updatedDate = @UpdatedDate ")
                        .Append(" where isbn = @Isbn ")
                        .ToString();

                    var parameters = new List<ISqlCommandParameter>
                    {
                        new StringParameter("Isbn", r2LibraryMarcFile.Isbn),
                        new StringParameter("Isbn10", r2LibraryMarcFile.Isbn10),
                        new StringParameter("Isbn13", r2LibraryMarcFile.Isbn13),
                        new StringParameter("EIsbn", r2LibraryMarcFile.EIsbn),
                        new Int32Parameter("ProviderSourceId", r2LibraryMarcFile.ProviderSourceId),
                        new StringParameter("MrkText", r2LibraryMarcFile.MrkText),
                        new DateTimeParameter("UpdatedDate", r2LibraryMarcFile.UpdatedDate)
                    };

                    var rowCount = ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
                    if (rowCount == 0)
                    {
                        recordsNotFound.Add(r2LibraryMarcFile);
                    }

                    totalUpdated += rowCount;
                }
                if (recordsNotFound.Any())
                {
                    totalInserted = InsertWebR2LibraryMarcRecords(recordsNotFound);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        public void UpdateOclcR2LibraryMarcRecords(List<R2LibraryMarcFile> r2LibraryMarcFiles, out int totalUpdated, out int totalInserted)
        {
            try
            {
                totalUpdated = 0;
                totalInserted = 0;

                List<R2LibraryMarcFile> recordsNotFound = new List<R2LibraryMarcFile>();

                foreach (R2LibraryMarcFile r2LibraryMarcFile in r2LibraryMarcFiles)
                {
                    var sql = new StringBuilder()
                        .Append(" update OclcR2LibraryMarcRecords ")
                        .Append(" set isbn10 = @Isbn10 ")
                        .Append(" , isbn13 = @Isbn13 ")
                        .Append(" , eIsbn = @EIsbn ")
                        .Append(" , marcRecordProviderTypeId = @ProviderSourceId ")
                        .Append(" , fileData = @MrkText ")
                        .Append(" , updatedDate = @UpdatedDate ")
                        .Append(" where isbn = @Isbn ")
                        .ToString();

                    var parameters = new List<ISqlCommandParameter>
                    {
                        new StringParameter("Isbn", r2LibraryMarcFile.Isbn),
                        new StringParameter("Isbn10", r2LibraryMarcFile.Isbn10),
                        new StringParameter("Isbn13", r2LibraryMarcFile.Isbn13),
                        new StringParameter("EIsbn", r2LibraryMarcFile.EIsbn),
                        new Int32Parameter("ProviderSourceId", r2LibraryMarcFile.ProviderSourceId),
                        new StringParameter("MrkText", r2LibraryMarcFile.MrkText),
                        new DateTimeParameter("UpdatedDate", r2LibraryMarcFile.UpdatedDate)
                    };

                    var rowCount = ExecuteUpdateStatement(sql, parameters.ToArray(), false, Settings.Default.RittenhouseMarcDb);
                    if (rowCount == 0)
                    {
                        recordsNotFound.Add(r2LibraryMarcFile);
                    }

                    totalUpdated += rowCount;
                }
                if (recordsNotFound.Any())
                {
                    totalInserted = InsertOclcR2LibraryMarcRecords(recordsNotFound);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
