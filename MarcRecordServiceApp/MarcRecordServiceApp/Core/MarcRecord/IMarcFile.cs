using System;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Tasks.MarcRecords;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public interface IMarcFile
    {

        string XmlFileText { get; set; }
        string MrkFileText { get; set; }
        string EncodingLevel { get;}
        string MrcFileText { get; set; }

        MarcRecordProvider RecordProvider { get; }
        Product Product { get; }
        DateTime ProcessedDate { get; }
    }
}
