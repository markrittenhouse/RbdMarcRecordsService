using System;
using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class LcMarcRecordFile : FactoryBase, IDataEntity
    {
        public int Id { get; set; }
        public string XmlFileData { get; set; }
        public string LcCallNumber { get; set; }

        public bool UpdateCallNumber { get; set; }
        
        public void Populate(SqlDataReader reader)
        {
            try
            {
                Id = GetInt32Value(reader, "lcMarcRecordFileId", -1);
                XmlFileData = GetStringValue(reader, "xmlFileData");
                LcCallNumber = GetStringValue(reader, "lcCallNumber");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }
}
