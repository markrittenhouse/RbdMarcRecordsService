namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public interface IProductStatus
    {
        int Id { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        bool IsAvailableForSale { get; set; }

        string GetQuantityOnHandText(int quantityOnHandValue);
    }
}