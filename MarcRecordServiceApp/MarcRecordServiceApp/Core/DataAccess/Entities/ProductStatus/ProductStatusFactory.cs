using System.Linq;

namespace MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus
{
    public class ProductStatusFactory
    {
        public static readonly IProductStatus None = new StatusNone();
        public static readonly IProductStatus Active = new StatusActive();
        public static readonly IProductStatus NotYetPublished = new StatusNotYetPublished();
        public static readonly IProductStatus OutOfPrint = new StatusOutOfPrint();
        public static readonly IProductStatus OutOfStockAtPublisher = new StatusOutOfStockAtPublisher();
        public static readonly IProductStatus OutOfStockIndefinitely = new StatusOutOfStockIndefinitely();
        public static readonly IProductStatus PublicationCancelled = new StatusPublicationCancelled();
        public static readonly IProductStatus PublicationIndefinite = new StatusPublicationIndefinite();
        public static readonly IProductStatus PrintOnDemand = new StatusPrintOnDemand();
        public static readonly IProductStatus R2Active = new StatusR2Active();
        public static readonly IProductStatus R2Forthcoming = new StatusR2Forthcoming();
        public static readonly IProductStatus R2Archived = new StatusR2Archived();
        public static readonly IProductStatus R2Inactive = new StatusR2Inactive();
        public static readonly IProductStatus R2Cancelled = new StatusR2Cancelled();

        private static readonly IProductStatus[] ProductStatuses = {
                                                                       Active,
                                                                       NotYetPublished,
                                                                       OutOfPrint,
                                                                       OutOfStockAtPublisher,
                                                                       OutOfStockIndefinitely,
                                                                       PublicationCancelled,
                                                                       PublicationIndefinite,
                                                                       PrintOnDemand,
                                                                       R2Active,
                                                                       R2Forthcoming,
                                                                       R2Archived,
                                                                       R2Inactive,
                                                                       R2Cancelled
                                                                   };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IProductStatus GetByCode(string code)
        {
            foreach (IProductStatus productStatus in ProductStatuses.Where(productStatus => code == productStatus.Code))
            {
                return productStatus;
            }
            return None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IProductStatus GetById(int id)
        {
            foreach (IProductStatus productStatus in ProductStatuses.Where(productStatus => id == productStatus.Id))
            {
                return productStatus;
            }
            return None;
        }

    }
}
