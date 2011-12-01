using System;

namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusR2Cancelled : StatusBase
    {
        internal StatusR2Cancelled()
        {
            Id = 13;
            Code = "R2C";
            Description = "R2 Cancelled";
            IsAvailableForSale = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            return string.Empty;
        }
    }
}


