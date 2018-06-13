using System.Collections.Generic;
using MarcRecordServiceApp.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class RittenhouseMarcFile : MarcFileData, IMarcFile
    {
        public RittenhouseMarcFile(Product product)
            : base(product)
        {
            RecordProviderType = MarcRecordProviderType.Rbd;
        }

        public RittenhouseMarcFile(Product product, int? marcRecordId, int? marcRecordProviderId)
            : base(product)
        {
            RecordProviderType = MarcRecordProviderType.Rbd;
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

