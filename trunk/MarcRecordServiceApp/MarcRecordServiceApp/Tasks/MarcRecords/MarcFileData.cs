using System.Linq;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Entities.Base;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class MarcFileData : EntityBase
    {
        public const char GroupSeparator = '\x1d';
        public const char RecordSeparator = '\x1e';
        public const char UnitSeparator = '\x1f';

        public string Isbn10 { get; private set; }
        public string Isbn13 { get; private set; }
        public string MrcFileText { get; private set; }
        public string MrkFileText { get; set; }
        public Product Product { get; private set; }

        private string[] _records;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mrcFileText"></param>
        public MarcFileData(string mrcFileText)
        {
            if (!mrcFileText.EndsWith(GroupSeparator.ToString()))
            {
                MrcFileText = string.Format("{0}{1}", mrcFileText, GroupSeparator);
            }
            MrcFileText = mrcFileText;

            _records = MrcFileText.Split(RecordSeparator);

            if (_records.Length > 3)
            {
                Isbn10 = _records[2].Substring(3);
                Isbn13 = _records[3].Substring(3);
                Log.DebugFormat("isbn10: {0}, isbn13: {1}", Isbn10, Isbn13);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="products"></param>
        public void SetProduct(Product[] products)
        {
            foreach (Product product in products.Where(product => product.Isbn10 == Isbn10))
            {
                Product = product;
                return;
            }
        }
    }
}
