using System;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Entities.Base;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
	public class MarcFileData2 : EntityBase
    {
        
        public string EncodingLevel { get; private set; }
        private string _mrcFileText;
        public string XmlFileText { get; set; }
        public string MrkFileText { get; set; }
        public string MrcFileText
        {
            get { return _mrcFileText; }
            set
            {
                _mrcFileText = value;
                EncodingLevel = value.Substring(17, 1);
            }
        }

        public Product Product { get; private set; }

        public DateTime ProcessedDate { get; private set; }

        /// <summary>
        /// Need to set product to insert null records when no record is found on external searchs
        /// </summary>
        /// <param name="product"></param>
        public MarcFileData2(Product product)
        {
            Product = product;
            ProcessedDate = DateTime.Now;
        }
    }
}
