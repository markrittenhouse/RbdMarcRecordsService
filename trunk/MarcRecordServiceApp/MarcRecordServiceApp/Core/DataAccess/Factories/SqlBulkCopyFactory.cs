using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using log4net;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class SqlBulkCopyFactory : FactoryBase
    {
        protected new static readonly ILog Log = LogManager.GetLogger(typeof(SqlBulkCopyFactory));
        public DataTable GetBulkInsertDataTable(string tableName)
        {
            try
            {
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(Settings.Default.RittenhouseMarcDb))
                {
                    connection.Open();


                    var sb = new StringBuilder()
                        .Append("select ")
                        .Append("CASE WHEN DATA_TYPE = 'varchar' then 'System.String' ")
                        .Append("WHEN DATA_TYPE = 'nvarchar' then 'System.String' ")
                        .Append("WHEN DATA_TYPE = 'datetime' then 'System.DateTime' ")
                        .Append("WHEN DATA_TYPE = 'smalldatetime' then 'System.DateTime' ")
                        .Append("WHEN DATA_TYPE = 'bit' then 'System.Int32' ")
                        .Append("WHEN DATA_TYPE = 'int' then 'System.Int32' ")
                        .Append("when DATA_TYPE = 'tinyint' then 'System.Int32' ")
                        .Append("when DATA_TYPE = 'char' then 'System.String' ")
                        .Append("when DATA_TYPE = 'money' then 'System.Decimal' ")
                        .Append("when DATA_TYPE = 'float' then 'System.Decimal' ")
                        .Append("when DATA_TYPE = 'decimal' then 'System.Decimal' ")
                        .Append("when DATA_TYPE = 'smallint' then 'System.Int16' ")
                        .Append("when DATA_TYPE = 'bigint' then 'System.Int64' ")
                        .Append("when DATA_TYPE = 'varbinary' then 'System.Byte[]' ")
                        .Append("when DATA_TYPE = 'text' then 'System.String' ")
                        .Append("END, COLUMN_NAME, IS_NULLABLE, isnull(CHARACTER_MAXIMUM_LENGTH, 0) ")
                        .Append($", COLUMNPROPERTY (OBJECT_ID('{tableName.Replace("dbo.", "")}'),ic.COLUMN_NAME ,'IsIdentity')")
                        .Append($"from INFORMATION_SCHEMA.COLUMNS IC where TABLE_NAME = '{tableName.Replace("dbo.", "")}' ")
                        .ToString();

                    SqlCommand tableInformationCommand = new SqlCommand(sb, connection);

                    SqlDataReader reader = tableInformationCommand.ExecuteReader();
                    while (reader.Read())
                    {

                        var datatype = reader.GetString(0);
                        var columnName = reader.GetString(1);
                        var allowNull = reader.GetString(2);
                        var maxLength = reader.GetInt32(3);
                        var isIdentity = reader.GetInt32(4);
                        if (isIdentity > 0)
                        {
                            continue;
                        }
                        var column = new DataColumn
                        {
                            ColumnName = columnName,
                            DataType = Type.GetType(datatype),
                            AllowDBNull = allowNull.ToLower() == "yes",
                            DefaultValue = null
                        };

                        if (maxLength > 0)
                        {
                            column.MaxLength = maxLength;
                        }

                        if (maxLength == -1)
                        {
                            column.MaxLength = 100000;
                        }


                        dataTable.Columns.Add(column);
                    }
                    reader.Close();
                }
                Log.Debug("GetBulkInsertDataTable was successfull");
                return dataTable;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }

        public void BulkInsertData(string tableName, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.RittenhouseMarcDb))
            {
                connection.Open();

                using (System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy(connection))
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.DestinationTableName = tableName;

                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
