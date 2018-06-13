using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class AdditionalField : FactoryBase, IDataEntity
    {
        public int MarcRecordId { get; set; }
        public string FieldNumberString{ get; set; }
        public int FieldNumber { get; set; }
        public string Value{ get; set; }
        public string Sku { get; set; }
        public void Populate(SqlDataReader reader)
        {
            MarcRecordId = GetInt32Value(reader, "marcRecordId", 0);
            FieldNumberString = GetStringValue(reader, "fieldNumber");
            Value = GetStringValue(reader, "marcValue");
            Sku = GetStringValue(reader, "sku");

            int.TryParse(FieldNumberString, out int test);

            FieldNumber = test;
        }
    }
}