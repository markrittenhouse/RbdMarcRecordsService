using System;
using System.Collections.Generic;
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

        MarcRecordProviderType RecordProviderType { get; }
        Product Product { get; }
        DateTime ProcessedDate { get; }

        int? MarcRecordId { get; set; }
        int? MarcRecordProviderId{ get; set; }

        bool IsProviderUpdate { get; set; }
        bool IsFileUpdate { get; set; }
        List<AdditionalField> AdditionalFields { get; set; }
    }
}
