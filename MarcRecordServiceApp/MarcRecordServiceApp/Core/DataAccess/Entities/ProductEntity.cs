using System;
using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;
using log4net;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class ProductEntity : FactoryBase, IDataEntity
    {
        protected new static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        //p.productId, p.sku, p.isbn10, p.isbn13, p.title, p.subTitle, p.authors, p.firstAuthorLastName, p.productStatusId 
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Authors { get; set; }
        public string FirstAuthorLastName { get; set; }
        public int ProductStatusId { get; set; }

        //p.affiliation, p.copyright, p.publicationDate, p.format, p.edition, p.pages, p.weight, p.length 
        public string Affiliation { get; set; }
        public int Copyright { get; set; }
        public string PublicationDate { get; set; }
        public string Format { get; set; }
        public string Edition { get; set; }
        public string Pages { get; set; }
        public decimal Weight { get; set; }
        public int Length { get; set; }

        //p.width, p.thickness, p.cartonQuantity, p.serialName, p.lcCallNumber, p.nlm, p.announcedEdition 
        public int Width { get; set; }
        public int Thickness { get; set; }
        public int CartonQuantity { get; set; }
        public string SeriesName { get; set; }
        public string LCCallNumber { get; set; }
        public string NLM { get; set; }
        public string AnnouncedEdition { get; set; }

        //p.announcedDate, p.newEditionIsbn, p.previousEditionIsbn, p.alternateIsbn, p.brandonHillCodes 
        public int AnnouncedDate { get; set; }
        public string NewEditionIsbn { get; set; }
        public string PreviousEditionIsbn { get; set; }
        public string AlternateIsbn { get; set; }
        public string BrandonHillCodes { get; set; }

        //p.priceGroup, p.venderNumber, p.venderInfo, p.isReturnable, p.isGsa, p.productType 
        public string PriceGroup { get; set; }
        public string VendorNumber { get; set; }
        public string VendorInfo { get; set; }
        public bool IsReturnable { get; set; }
        public bool IsGsa { get; set; }
        public string ProductType { get; set; }

        //p.audiancePrimary, p.audianceSecondary, p.imprint, p.doodyRating, p.publisherPrelude, p.orderBydate 
        public string AudiencePrimary { get; set; }
        public string AudienceSecondary { get; set; }
        public string Imprint { get; set; }
        public short DoodyRating { get; set; }
        public string PublisherPrelude { get; set; }
        public DateTime OrderByDate { get; set; }

        //p.brandonHillOrderBy, p.r2ResourceId, p.countryCode, p.eIsbn, p.isAvailableForSale, p.availabilityCode, p.tocFilename 
        public short BrandonHillOrderBy { get; set; }
        public int R2ResourceId { get; set; }
        public string CountryCode { get; set; }
        public string EIsbn { get; set; }
        public bool IsAvailableForSale { get; set; }
        public string AvailabilityCode { get; set; }
        public string TocFilename { get; set; }

        //i.quantityOnHand, i.quantityOnOrder, i.inStockOverride 
        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public bool InStockOverride { get; set; }

        //pp.priceList, pp.priceNet, pp.price2, pp.price3, pp.price4, pp.priceFuture 
        public decimal PriceList { get; set; }
        public decimal PriceNet { get; set; }
        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }
        public decimal Price4 { get; set; }
        public decimal PriceFuture { get; set; }

        //cat.categoryId, cat.categoryName, cat.categoryCode 
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }

        //pub.publisherId, pub.publisherName 
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }

        //pci.fileName 
        public string ImageFileName { get; set; }

        ////ps.synopsis
        //public string Synopsis { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void Populate(SqlDataReader reader)
        {
            try
            {
                //p.productId, p.sku, p.isbn10, p.isbn13, p.title, p.subTitle, p.authors, p.firstAuthorLastName, p.productStatusId 
                Id = GetInt32Value(reader, "productId", -1);
                Sku = GetStringValue(reader, "sku");
                Isbn10 = GetStringValue(reader, "isbn10");
                Isbn13 = GetStringValue(reader, "isbn13");
                Title = RemoveControlCharacters(GetStringValue(reader, "title"));
                SubTitle = RemoveControlCharacters(GetStringValue(reader, "subTitle"));
                Authors = RemoveControlCharacters(GetStringValue(reader, "authors"));
                FirstAuthorLastName = GetStringValue(reader, "firstAuthorLastName");
                ProductStatusId = GetInt32Value(reader, "productStatusId", -1);

                //p.affiliation, p.copyright, p.publicationDate, p.format, p.edition, p.pages, p.weight, p.length 
                Affiliation = GetStringValue(reader, "affiliation", "");
                Copyright = GetInt32Value(reader, "copyright", -1);
                PublicationDate = GetStringValue(reader, "publicationDate", "");
                Format = GetStringValue(reader, "format", "");
                Edition = GetStringValue(reader, "edition", "");
                Pages = GetStringValue(reader, "pages", "");
                Weight = GetDecimalValue(reader, "weight", 1);
                Length = GetInt32Value(reader, "length", 0);

                //p.width, p.thickness, p.cartonQuantity, p.serialName, p.lcCallNumber, p.nlm, p.announcedEdition 
                Width = GetInt32Value(reader, "width", 0);
                Thickness = GetInt32Value(reader, "thickness", 0);
                CartonQuantity = GetInt32Value(reader, "cartonQuantity", 0);
                SeriesName = GetStringValue(reader, "serialName", "");
                LCCallNumber = GetStringValue(reader, "lcCallNumber");
                NLM = GetStringValue(reader, "nlm");
                AnnouncedEdition = GetStringValue(reader, "announcedEdition");

                //p.announcedDate, p.newEditionIsbn, p.previousEditionIsbn, p.alternateIsbn, p.brandonHillCodes 
                AnnouncedDate = GetInt32Value(reader, "announcedDate", -1);
                NewEditionIsbn = GetStringValue(reader, "newEditionIsbn");
                PreviousEditionIsbn = GetStringValue(reader, "previousEditionIsbn");
                AlternateIsbn = GetStringValue(reader, "alternateIsbn");
                BrandonHillCodes = GetStringValue(reader, "brandonHillCodes");

                //p.priceGroup, p.venderNumber, p.venderInfo, p.isReturnable, p.isGsa, p.productType 
                PriceGroup = GetStringValue(reader, "priceGroup");
                VendorNumber = GetStringValue(reader, "venderNumber", "");
                VendorInfo = GetStringValue(reader, "venderInfo", "");
                IsReturnable = GetBoolValue(reader, "isReturnable", false);
                IsGsa = GetBoolValue(reader, "isGsa", false);
                ProductType = GetStringValue(reader, "productType", "");

                //p.audiancePrimary, p.audianceSecondary, p.imprint, p.doodyRating, p.publisherPrelude, p.orderBydate 
                AudiencePrimary = GetStringValue(reader, "audiancePrimary", "");
                AudienceSecondary = GetStringValue(reader, "audianceSecondary", "");
                Imprint = GetStringValue(reader, "imprint", "");
                DoodyRating = GetInt16Value(reader, "doodyRating", -1);
                PublisherPrelude = GetStringValue(reader, "publisherPrelude", "");
                OrderByDate = GetDateValue(reader, "orderByDate");

                //p.brandonHillOrderBy, p.r2ResourceId, p.countryCode, p.eIsbn, p.isAvailableForSale, p.availabilityCode, p.tocFilename 
                BrandonHillOrderBy = GetInt16Value(reader, "brandonHillOrderBy", -1);
                R2ResourceId = GetInt32Value(reader, "r2ResourceId", -1);
                CountryCode = GetStringValue(reader, "countryCode", "");
                EIsbn = GetStringValue(reader, "eIsbn", "");
                IsAvailableForSale = GetBoolValue(reader, "isAvailableForSale", false);
                AvailabilityCode = GetStringValue(reader, "availabilityCode", "N");
                TocFilename = GetStringValue(reader, "tocFilename");

                //i.quantityOnHand, i.quantityOnOrder, i.inStockOverride 
                QuantityOnHand = GetInt32Value(reader, "quantityOnHand", -1);
                QuantityOnOrder = GetInt32Value(reader, "quantityOnOrder", -1);
                InStockOverride = GetBoolValue(reader, "inStockOverride", false);

                //pp.priceList, pp.priceNet, pp.price2, pp.price3, pp.price4, pp.priceFuture 
                PriceList = GetDecimalValue(reader, "priceList", 0);
                PriceNet = GetDecimalValue(reader, "priceNet", 0);
                Price2 = GetDecimalValue(reader, "price2", 0);
                Price3 = GetDecimalValue(reader, "price3", 0);
                Price4 = GetDecimalValue(reader, "price4", 0);
                PriceFuture = GetDecimalValue(reader, "priceFuture", 0);

                //cat.categoryId, cat.categoryName, cat.categoryCode 
                CategoryId = GetInt32Value(reader, "categoryId", -1);
                CategoryName = GetStringValue(reader, "categoryName", "");
                CategoryCode = GetStringValue(reader, "categoryCode", "");

                //pub.publisherId, pub.publisherName 
                PublisherId = GetInt32Value(reader, "publisherId", -1);
                PublisherName = GetStringValue(reader, "publisherName");

                //pci.fileName 
                ImageFileName = GetStringValue(reader, "fileName", "");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Product GetProduct()
        {
            try
            {
                Product product = new Product
                {
                    Id = Id,
                    Sku = Sku,
                    Isbn10 = Isbn10,
                    Isbn13 = Isbn13,
                    Title = Title,
                    SubTitle = SubTitle,
                    Authors = Authors,
                    FirstAuthorLastName = FirstAuthorLastName,
                    Affiliation = Affiliation,
                    Copyright = Copyright,
                    PublicationDate = PublicationDate,
                    Format = Format,
                    Edition = Edition,
                    Pages = Pages,
                    Weight = Weight,
                    Length = Length,
                    Width = Width,
                    Thickness = Thickness,
                    CartonQuantity = CartonQuantity,
                    SeriesName = SeriesName,
                    LCCallNumber = LCCallNumber,
                    NLM = NLM,
                    AnnouncedEdition = AnnouncedEdition,
                    AnnouncedDate = AnnouncedDate,
                    NewEditionIsbn = NewEditionIsbn,
                    PreviousEditionIsbn = PreviousEditionIsbn,
                    AlternateIsbn = AlternateIsbn,
                    BrandonHillCodes = BrandonHillCodes,
                    PriceGroup = PriceGroup,
                    VendorNumber = VendorNumber,
                    VendorInfo = VendorNumber,
                    IsReturnable = IsReturnable,
                    IsGsa = IsGsa,
                    ProductType = ProductType,
                    AudiencePrimary = AudiencePrimary,
                    AudienceSecondary = AudienceSecondary,
                    Imprint = Imprint,
                    DoodyRating = DoodyRating,
                    R2ResourceId = R2ResourceId,
                    CountryCode = CountryCode,
                    EIsbn = EIsbn,
                    IsAvailableForSale = IsAvailableForSale,
                    AvailabilityCode = AvailabilityCode,
                    TocFilename = TocFilename,
                    QuantityOnHand = QuantityOnHand,
                    QuantityOnOrder = QuantityOnOrder,
                    InStockOverride = InStockOverride
                };

                int productStatusId = ProductStatusId;
                product.Status = ProductStatusFactory.GetById(productStatusId);


                //  pp.priceList, pp.priceNet, pp.price2, pp.price3, pp.price4, pp.priceFuture
                product.Prices[0] = PriceList;
                product.Prices[1] = PriceNet;
                product.Prices[2] = Price2;
                product.Prices[3] = Price3;
                product.Prices[4] = Price4;
                product.Prices[5] = PriceFuture;

                product.Category = new Category { Id = CategoryId, Name = CategoryName, Code = CategoryCode };

                // pub.publisherId, pub.publisherName
                product.Publisher = new Publisher { Id = PublisherId, Name = PublisherName };
                product.PublisherPrelude = PublisherPrelude;

                product.Image = ImageFileName;

                return product;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }
}


