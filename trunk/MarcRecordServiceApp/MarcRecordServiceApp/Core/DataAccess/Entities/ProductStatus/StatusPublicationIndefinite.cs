namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusPublicationIndefinite : StatusBase
    {
        internal StatusPublicationIndefinite()
        {
            Id = 7;
            Code = "PI";
            Description = "Publication Indefinite";
            IsAvailableForSale = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //PI	    - QTY OH = XXX
            //          - If qty = less than 0, display 0	
            return string.Format("{0}", quantityOnHandValue);
        }
    }
}


