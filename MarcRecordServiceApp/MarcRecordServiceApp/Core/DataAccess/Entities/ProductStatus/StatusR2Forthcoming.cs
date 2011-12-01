namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusR2Forthcoming : StatusBase
    {
        internal StatusR2Forthcoming()
        {
            Id = 10;
            Code = "R2F";
            Description = "R2 Forthcoming";
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


