using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class MarcRecordData : FactoryBase, IDataEntity
    {
        public int MarcRecordId { get; set; }
        public int ProviderId { get; set; }
        public string FileData { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                MarcRecordId = GetInt32Value(reader, "marcRecordId", -1);
                ProviderId = GetInt32Value(reader, "marcRecordProviderTypeId", 0);
                FileData = GetStringValue(reader, "fileData");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }

    public class MarcRecordDataField : FactoryBase, IDataEntity
    {
        public MarcRecordDataField()
        {
            MarcRecordDataSubFields = new List<MarcRecordDataSubField>();
        }
        public int Id { get; set; }
        public int MarcRecordId { get; set; }
        public int ProviderId { get; set; }
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
                ProviderId = GetInt32Value(reader, "marcRecordProviderTypeId", -1);
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
        public int MarcRecordDataFieldId { get; set; }
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