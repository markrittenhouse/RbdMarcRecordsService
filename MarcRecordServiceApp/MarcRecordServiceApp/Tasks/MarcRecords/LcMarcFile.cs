using MarcRecordServiceApp.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class LcMarcFile : MarcFileData2, IMarcFile
    {
        private readonly MarcRecordProvider _marcRecordProvider;



        public LcMarcFile(Product product)
            : base(product)
        {
            _marcRecordProvider = MarcRecordProvider.Lc;
        }

        public MarcRecordProvider RecordProvider
        {
            get { return _marcRecordProvider; }
        }
    }
}
