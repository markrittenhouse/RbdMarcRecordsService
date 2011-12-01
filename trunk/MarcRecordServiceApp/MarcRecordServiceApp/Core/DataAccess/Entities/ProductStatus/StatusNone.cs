namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusNone : StatusBase
    {
        internal StatusNone()
        {
            Id = 0;
            Code = "";
            Description = "";
            IsAvailableForSale = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            return string.Format("{0}", quantityOnHandValue);
        }
    }
}


