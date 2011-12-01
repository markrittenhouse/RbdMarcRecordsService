using System;

namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusPublicationCancelled : StatusBase
    {
        internal StatusPublicationCancelled()
        {
            Id = 6;
            Code = "PC";
            Description = "Publication Cancelled";
            IsAvailableForSale = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //PC	    - QTY OH = XXX
            //          - If qty = less than 0, display 0	
            return string.Format("{0}", quantityOnHandValue);
        }
    }
}


