namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusNotYetPublished : StatusBase
    {
        internal StatusNotYetPublished()
        {
            Id = 2;
            Code = "NYP";
            Description = "Not Yet Published";
            IsAvailableForSale = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //NYP	    - QTY OH = O 
            //          - Add Line Item: QTY on order from publisher = XXXX	
            return string.Format("{0}", quantityOnHandValue);
        }
    }
}


