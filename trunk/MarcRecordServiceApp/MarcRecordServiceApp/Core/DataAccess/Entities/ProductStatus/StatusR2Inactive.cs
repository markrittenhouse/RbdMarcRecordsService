namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusR2Inactive : StatusBase
    {
        internal StatusR2Inactive()
        {
            Id = 12;
            Code = "R2I";
            Description = "R2 Inactive (Recalled)";
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


