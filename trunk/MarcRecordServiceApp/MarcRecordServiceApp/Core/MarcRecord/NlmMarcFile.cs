using System.Collections.Generic;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Tasks.MarcRecords;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class NlmMarcFile : MarcFileData, IMarcFile
    {
        public NlmMarcFile(Product product)
            : base(product)
        {
            RecordProviderType = MarcRecordProviderType.Nlm;
        }

        public NlmMarcFile(Product product, int? marcRecordId, int? marcRecordProviderId)
            : base(product)
        {
            RecordProviderType = MarcRecordProviderType.Nlm;
            MarcRecordId = marcRecordId;
            MarcRecordProviderId = marcRecordProviderId;
        }

        public MarcRecordProviderType RecordProviderType { get; }

        public int? MarcRecordId { get; set; }
        public int? MarcRecordProviderId { get; set; }

        public bool IsProviderUpdate { get; set; }
        public bool IsFileUpdate { get; set; }
        public List<AdditionalField> AdditionalFields { get; set; }
    }
}
