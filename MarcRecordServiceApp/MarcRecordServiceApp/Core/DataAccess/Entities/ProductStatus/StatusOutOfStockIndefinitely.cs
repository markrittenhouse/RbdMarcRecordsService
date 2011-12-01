namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusOutOfStockIndefinitely : StatusBase
    {
        internal StatusOutOfStockIndefinitely()
        {
            Id = 5;
            Code = "OSI";
            Description = "Out of Stock Indefinitely";
            IsAvailableForSale = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //OSI	    - QTY OH = XXX
            //          - If qty = less than 0, display 0
            //          - Add Text: No Reprint Date	
            return string.Format("{0}, No Reprint Date", quantityOnHandValue);
        }
    }
}


