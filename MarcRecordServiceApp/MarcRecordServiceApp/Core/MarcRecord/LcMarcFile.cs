using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Tasks.MarcRecords;

namespace MarcRecordServiceApp.Core.MarcRecord
{
    public class LcMarcFile : MarcFileData, IMarcFile
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
