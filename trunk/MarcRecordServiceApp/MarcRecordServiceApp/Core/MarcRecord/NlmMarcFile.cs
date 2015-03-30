using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Tasks.MarcRecords;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class NlmMarcFile : MarcFileData, IMarcFile
    {
        private readonly MarcRecordProvider _marcRecordProvider;

        public NlmMarcFile(Product product)
            : base(product)
        {
            _marcRecordProvider = MarcRecordProvider.Nlm;
        }

        public MarcRecordProvider RecordProvider
        {
            get { return _marcRecordProvider; }
        }
    }
}
