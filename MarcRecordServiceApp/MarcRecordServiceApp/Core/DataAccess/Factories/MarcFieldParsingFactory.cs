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
Delete x
from MarcRecordDataSubField x
join MarcRecordDataField mf on x.marcRecordDataFieldId = mf.marcRecordDataFieldId
join MarcRecordProvider mrp on mf.marcRecordId = mrp.marcRecordId and mf.marcRecordProviderTypeId = mrp.marcRecordProviderTypeId
where mf.dateCreated < isnull(mrp.dateUpdated, mrp.dateCreated);

Delete mf
from MarcRecordDataField mf
join MarcRecordProvider mrp on mf.marcRecordId = mrp.marcRecordId and mf.marcRecordProviderTypeId = mrp.marcRecordProviderTypeId
where mf.dateCreated < isnull(mrp.dateUpdated, mrp.dateCreated);
";
            ExecuteStatement(deleteSql, false, Settings.Default.RittenhouseMarcDb);
        }

        public void ClearMarcRecordDataFieldTables()
        {
            ExecuteTruncateTable("MarcRecordDataSubField", Settings.Default.RittenhouseMarcDb);

            var sql = @"
    ALTER TABLE MarcRecordDataSubField DROP CONSTRAINT FK_MarcRecordDataField_MarcRecordDataFieldId;
";
            ExecuteStatement(sql, false, Settings.Default.RittenhouseMarcDb);

            ExecuteTruncateTable("MarcRecordDataField", Settings.Default.RittenhouseMarcDb);

            sql = @"
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




    }
}
