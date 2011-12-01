using System;

namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusR2Active : StatusBase
    {
        internal StatusR2Active()
        {
            Id = 9;
            Code = "R2A";
            Description = "R2 Active";
            IsAvailableForSale = true;
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


