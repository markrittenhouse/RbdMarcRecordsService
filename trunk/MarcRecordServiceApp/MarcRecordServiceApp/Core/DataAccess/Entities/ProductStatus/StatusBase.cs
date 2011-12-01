using System;
using MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus;

namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public abstract class StatusBase : IProductStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsAvailableForSale { get; set; }

        public abstract string GetQuantityOnHandText(int quantityOnHandValue);

        public override string ToString()
        {
            return string.Format("IProductStatus = [Id: {0}, Code: {1}, Description: {2}, IsAvailableForSale: {3}]", Id, Code, Description, IsAvailableForSale);
        }
    }
}


