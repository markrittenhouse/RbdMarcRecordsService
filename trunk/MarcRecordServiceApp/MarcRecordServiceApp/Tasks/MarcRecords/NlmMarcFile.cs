using MarcRecordServiceApp.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class NlmMarcFile : MarcFileData2, IMarcFile
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
