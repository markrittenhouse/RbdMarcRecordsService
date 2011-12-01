namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class StatusOutOfPrint : StatusBase
    {
        internal StatusOutOfPrint()
        {
            Id = 3;
            Code = "OP";
            Description = "Out of Print";
            IsAvailableForSale = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantityOnHandValue"></param>
        /// <returns></returns>
        public override string GetQuantityOnHandText(int quantityOnHandValue)
        {
            //OP        - QTY OH XXXX
            //          - If qty = less than 0, display 0	                        
            return string.Format("{0}", quantityOnHandValue);
        }
    }
}


