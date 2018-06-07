using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class MarcFieldParsingFactory : FactoryBase
    {
        private readonly SqlBulkCopyFactory _sqlBulkCopyFactory;
        public MarcFieldParsingFactory()
        {
            _sqlBulkCopyFactory = new SqlBulkCopyFactory();
        }

        public void ClearOldMarcRecordData()
        {
            var deleteSql = @"
delete from MarcRecordDataSubField where marcRecordDataSubFieldsId in (
    select sub.marcRecordDataSubFieldsId
    from MarcRecordDataField mf
    join MarcRecordDataSubField sub on mf.marcRecordDataFieldId = sub.marcRecordDataFieldId
    join MarcRecordProvider mrp on mf.marcRecordId = mrp.marcRecordId
    where mf.dateCreated < mrp.dateCreated or mf.dateCreated < isnull(mrp.dateUpdated, GETDATE())
    group by sub.marcRecordDataSubFieldsId
)

delete from MarcRecordDataField where marcRecordId in (
select mf.marcRecordId
    from MarcRecordDataField mf
    join MarcRecordProvider mrp on mf.marcRecordId = mrp.marcRecordId
    where mf.dateCreated < mrp.dateCreated or mf.dateCreated < isnull(mrp.dateUpdated, GETDATE())
    group by mf.marcRecordId
)
";
            ExecuteStatement(deleteSql, false, Settings.Default.RittenhouseMarcDb);
        }

        public void ClearMarcRecordDataFieldTables()
        {
            ExecuteTrancateTable("MarcRecordDataSubField", Settings.Default.RittenhouseMarcDb);
            DropForeignKey();
            ExecuteTrancateTable("MarcRecordDataField", Settings.Default.RittenhouseMarcDb);
            AddForeignKey();
        }

        private void DropForeignKey()
        {
            var sql = @"
    ALTER TABLE MarcRecordDataSubField DROP CONSTRAINT FK_MarcRecordDataField_MarcRecordDataFieldId;
";
            ExecuteStatement(sql, false, Settings.Default.RittenhouseMarcDb);
        }

        private void AddForeignKey()
        {
            var sql = @"
            ALTER TABLE[dbo].[MarcRecordDataSubField] WITH CHECK ADD CONSTRAINT[FK_MarcRecordDataField_MarcRecordDataFieldId] FOREIGN KEY([marcRecordDataFieldId])
            REFERENCES[dbo].[MarcRecordDataField]
                ([marcRecordDataFieldId])


            ALTER TABLE[dbo].[MarcRecordDataSubField]
            CHECK CONSTRAINT[FK_MarcRecordDataField_MarcRecordDataFieldId]
";
            ExecuteStatement(sql, false, Settings.Default.RittenhouseMarcDb);
        }

        public int InsertMarcRecordDataFields(List<MarcRecordDataField> marcRecordDataFields)
        {
            int rowsInserted = 0;
            var parentTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecordDataField");
            var childTable = _sqlBulkCopyFactory.GetBulkInsertDataTable("MarcRecordDataSubField");

            SetMarcRecordDataFieldIds(marcRecordDataFields);

            foreach (var marcRecordDataField in marcRecordDataFields)
            {
                //[marcRecordDataFieldId] [int] NOT NULL,
                //[marcRecordId] [int] NOT NULL,
                //[marcRecordProviderTypeId] [int] NOT NULL,
                //[fieldNumber] [varchar] (10) NOT NULL,
                //[fieldIndicator] [varchar] (10) NOT NULL,
                //[marcValue] [varchar] (max) NOT NULL, [dateCreated] [datetime] found field that is 9313 characters long
                DataRow parentRow = parentTable.NewRow();
                parentRow.SetField(0, marcRecordDataField.Id);
                parentRow.SetField(1, marcRecordDataField.MarcRecordId);
                //
                parentRow.SetField(2, marcRecordDataField.ProviderId);
                parentRow.SetField(3, marcRecordDataField.FieldNumber);
                parentRow.SetField(4, marcRecordDataField.FieldIndicator);
                
                parentRow.SetField(5, marcRecordDataField.MarcValue);
                parentRow.SetField(6, DateTime.Now);

                parentTable.Rows.Add(parentRow);
                rowsInserted++;
                foreach (var marcRecordDataSubField in marcRecordDataField.MarcRecordDataSubFields)
                {
                    //[marcRecordDataSubFieldsId] [int] NOT NULL,
                    //[marcRecordDataFieldId] int not null,
                    //[subFieldIndicator] [varchar] (10) NOT NULL,
                    //[subFieldValue]  [varchar] (max) NOT NULL, [dateCreated] [datetime] found field that is 9313 characters long
                    DataRow childRow = childTable.NewRow();
                    childRow.SetField(0, marcRecordDataSubField.Id);
                    childRow.SetField(1, marcRecordDataField.Id);
                    childRow.SetField(2, marcRecordDataSubField.SubFieldIndicator);
                    childRow.SetField(3, marcRecordDataSubField.SubFieldValue);

                    childTable.Rows.Add(childRow);
                    rowsInserted++;
                }

            }

            _sqlBulkCopyFactory.BulkInsertData("MarcRecordDataField", parentTable);
            _sqlBulkCopyFactory.BulkInsertData("MarcRecordDataSubField", childTable);
            return rowsInserted;
        }


        private void SetMarcRecordDataFieldIds(List<MarcRecordDataField> marcRecordDataFields)
        {
            int parentTableId = GetLastId("MarcRecordDataField", "marcRecordDataFieldId");
            int childTableId = GetLastId("MarcRecordDataSubField", "marcRecordDataSubFieldsId");

            foreach (var marcRecordDataField in marcRecordDataFields)
            {
                parentTableId++;
                marcRecordDataField.Id = parentTableId;
                foreach (var marcRecordDataSubField in marcRecordDataField.MarcRecordDataSubFields)
                {
                    childTableId++;
                    marcRecordDataSubField.Id = childTableId;
                }
            }
        }

        private int GetLastId(string tableName, string columnName)
        {
            int lastColumnName = 0;
            using (SqlConnection connection = new SqlConnection(Settings.Default.RittenhouseMarcDb))
            {
                connection.Open();
                string sql = $"select isnull(MAX({columnName}), 0) from {tableName}";
                SqlCommand tableInformationCommand = new SqlCommand(sql, connection);
                SqlDataReader reader = tableInformationCommand.ExecuteReader();
                while (reader.Read())
                {
                    lastColumnName = reader.GetInt32(0);
                }

                reader.Close();
            }

            Log.Debug("GetLastId was successfull");
            return lastColumnName;
        }

//        public List<ParsedMarcField> GetParsedMarcFields(List<string> skus)
//        {
//            SqlConnection cnn = null;
//            SqlCommand command = null;
//            SqlDataReader reader = null;

//            Stopwatch stopWatch = new Stopwatch();
//            stopWatch.Start();

//            List<ParsedMarcField> parsedMarcFields = new List<ParsedMarcField>();
//            string lastSql = null;
//            try
//            {
//                //String.Join(", ", isbnsNotFound)
//                var skuString = String.Join(",", skus.Select(x=> $"'{x}'"));
//                var sqlOclcNumber = $@"
//select mf.marcValue, mr.sku
//from MarcRecordDataField mf
//join MarcRecordDataSubField sub on mf.marcRecordDataFieldId = sub.marcRecordDataFieldId
//join MarcRecord mr on mf.marcRecordId = mr.marcRecordId
//where mf.fieldNumber = '035' and sub.subFieldIndicator = '$a' and sub.subFieldvalue like '%ocolc%'
//and mr.sku in ({skuString})
//group by mf.marcValue, mr.sku
//order by mr.sku";

//                var sqlNlmNumber = $@"
//select mf.marcValue, mr.sku
//from MarcRecordDataField mf
//join MarcRecordDataSubField sub on mf.marcRecordDataFieldId = sub.marcRecordDataFieldId
//join MarcRecord mr on mf.marcRecordId = mr.marcRecordId
//where mf.fieldNumber = '060' and sub.subFieldIndicator = '$a'
//and mr.sku in ({skuString})
//group by mf.marcValue, mr.sku
//order by mr.sku";

//                var sqlLcNumber = $@"
//select mf.marcValue, mr.sku
//from MarcRecordDataField mf
//join MarcRecordDataSubField sub on mf.marcRecordDataFieldId = sub.marcRecordDataFieldId
//join MarcRecord mr on mf.marcRecordId = mr.marcRecordId
//where mf.fieldNumber = '090' and sub.subFieldIndicator = '$a'
//and mr.sku in ({skuString})
//group by mf.marcValue, mr.sku
//order by mr.sku";

//                var sqlNlmSubjects = $@"
//select mf.marcValue, mr.sku
//from MarcRecordDataField mf
//join MarcRecordDataSubField sub on mf.marcRecordDataFieldId = sub.marcRecordDataFieldId
//join MarcRecord mr on mf.marcRecordId = mr.marcRecordId
//where mf.fieldNumber = '650' and mf.fieldIndicator in ('12', '\2')
//and mr.sku in ({skuString})
//group by mf.marcValue, mr.sku
//order by mr.sku";

//                lastSql = sqlOclcNumber;
//                cnn = GetRittenhouseConnection();
//                command = cnn.CreateCommand();
//                command.CommandText = sqlOclcNumber;
//                command.CommandTimeout = 150;
//                reader = command.ExecuteReader();

//                while (reader.Read())
//                {
//                    ParsedMarcField marcRecordData = new ParsedMarcField();
//                    marcRecordData.Populate(reader, MarcFieldType.OclcNumber);
//                    parsedMarcFields.Add(marcRecordData);
//                }

//                lastSql = sqlNlmNumber;
//                cnn = GetRittenhouseConnection();
//                command = cnn.CreateCommand();
//                command.CommandText = sqlNlmNumber;
//                command.CommandTimeout = 150;
//                reader = command.ExecuteReader();

//                while (reader.Read())
//                {
//                    ParsedMarcField marcRecordData = new ParsedMarcField();
//                    marcRecordData.Populate(reader, MarcFieldType.NlmNumber);
//                    parsedMarcFields.Add(marcRecordData);
//                }

//                lastSql = sqlLcNumber;
//                cnn = GetRittenhouseConnection();
//                command = cnn.CreateCommand();
//                command.CommandText = sqlLcNumber;
//                command.CommandTimeout = 150;
//                reader = command.ExecuteReader();

//                while (reader.Read())
//                {
//                    ParsedMarcField marcRecordData = new ParsedMarcField();
//                    marcRecordData.Populate(reader, MarcFieldType.LcNumber);
//                    parsedMarcFields.Add(marcRecordData);
//                }

//                lastSql = sqlNlmSubjects;
//                cnn = GetRittenhouseConnection();
//                command = cnn.CreateCommand();
//                command.CommandText = sqlNlmSubjects;
//                command.CommandTimeout = 150;
//                reader = command.ExecuteReader();

//                while (reader.Read())
//                {
//                    ParsedMarcField marcRecordData = new ParsedMarcField();
//                    marcRecordData.Populate(reader, MarcFieldType.NlmSubject);
//                    parsedMarcFields.Add(marcRecordData);
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Info($"sql: {lastSql}");
//                Log.Error(ex.Message, ex);
//                throw;
//            }
//            finally
//            {
//                DisposeConnections(cnn, command, reader);
//            }

//            return parsedMarcFields;
//        }
    }


    //public class ParsedMarcField : FactoryBase, IDataEntity
    //{
    //    public MarcFieldType Type { get; set; }
    //    public string Value { get; set; }
    //    public string Sku { get; set; }

    //    public void Populate(SqlDataReader reader)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Populate(SqlDataReader reader, MarcFieldType type)
    //    {
    //        Type = type;
    //        Sku = GetStringValue(reader, "Sku");
    //        Value = GetStringValue(reader, "marcValue");
    //    }
    //}

    //public enum MarcFieldType
    //{
    //    OclcNumber = 035,
    //    NlmNumber = 060,
    //    LcNumber = 090,
    //    NlmSubject = 650
    //}
}
