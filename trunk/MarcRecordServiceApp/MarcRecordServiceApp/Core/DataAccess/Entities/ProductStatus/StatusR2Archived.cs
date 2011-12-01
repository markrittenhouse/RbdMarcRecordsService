namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusR2Archived : StatusBase
    {
        internal StatusR2Archived()
        {
            Id = 11;
            Code = "ARC";
            Description = "R2 Archived";
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


