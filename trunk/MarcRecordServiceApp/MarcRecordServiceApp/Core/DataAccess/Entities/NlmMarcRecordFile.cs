using System;
using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class NlmMarcRecordFile : FactoryBase, IDataEntity
    {
        public int Id { get; set; }
        public string XmlFileData { get; set; }
        public string NlmCallNumber { get; set; }

        //public bool UpdateCallNumber { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                Id = GetInt32Value(reader, "nlmMarcRecordFileId", -1);
                XmlFileData = GetStringValue(reader, "xmlFileData");
                NlmCallNumber = GetStringValue(reader, "nlmCallNumber");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }
}
