namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusOutOfStockAtPublisher : StatusBase
    {
        internal StatusOutOfStockAtPublisher()
        {
            Id = 4;
            Code = "OS";
            Description = "Out of Stock at the Publisher";
            IsAvailableForSale = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //OS	    - QTY OH = XXX
            //          - If qty = less than 0, display 0
            //          - Add Text: Reprint Pending 	
            return string.Format("{0}, Reprint Pending", quantityOnHandValue);
        }
    }
}


