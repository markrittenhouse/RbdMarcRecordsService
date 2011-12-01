namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusActive : StatusBase
    {
        internal StatusActive()
        {
            Id = 1;
            Code = "ACT";
            Description = "Active";
            IsAvailableForSale = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //ACT	    - QTY OH XXXX
            //          - If qty = less than 0, display 0
            //          - If QTY OH = 0 display new line item: QTY on order from publisher = XXXX	
            return string.Format("{0}", quantityOnHandValue);
        }
    }
}


