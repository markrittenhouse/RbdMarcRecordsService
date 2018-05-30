using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Tasks.MarcRecords;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class LcMarcFile : MarcFileData, IMarcFile
    {
        public LcMarcFile(Product product)
            : base(product)
        {
            RecordProviderType = MarcRecordProviderType.Lc;
        }

        public LcMarcFile(Product product, int? marcRecordId, int? marcRecordProviderId)
            : base(product)
        {
            RecordProviderType = MarcRecordProviderType.Lc;
            MarcRecordId = marcRecordId;
            MarcRecordProviderId = marcRecordProviderId;
        }

        public MarcRecordProviderType RecordProviderType { get; }

        public int? MarcRecordId { get; set; }
        public int? MarcRecordProviderId { get; set; }

        public bool IsProviderUpdate { get; set; }
        public bool IsFileUpdate { get; set; }
    }
}
