namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusPrintOnDemand : StatusBase
    {
        internal StatusPrintOnDemand()
        {
            Id = 8;
            Code = "POD";
            Description = "Print on Demand";
            IsAvailableForSale = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //POD	    - QTY OH = XXX
            //          - If qty = less than 0, display 0	
            return string.Format("{0}", quantityOnHandValue);
        }    
    }
}


