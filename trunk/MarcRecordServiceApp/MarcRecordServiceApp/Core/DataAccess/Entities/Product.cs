using System;
using System.Collections.Generic;
using MarcRecordServiceApp.Core.DataAccess.Entities.Base;
using MarcRecordServiceApp.Core.DataAccess.Entities.ProductStatus;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
	public class Product : EntityBase 
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn10Formatted { get; set; }
        public string Isbn13 { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Authors { get; set; }
        public string FirstAuthorLastName { get; set; }

		public Category Category { get; set; }
		public Publisher Publisher { get; set; }
		public IProductStatus Status { get; set; }
        public string Affiliation { get; set; }
        public int Copyright { get; set; }
        public string PublicationDate { get; set; }
        public string Format { get; set; }
        public string Edition { get; set; }

        public string Pages { get; set; }
        public decimal Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Thickness { get; set; }
        public int CartonQuantity { get; set; }
        public string SeriesName { get; set; }
        public string LCCallNumber { get; set; }

        public string NLM { get; set; }
        public string AnnouncedEdition { get; set; }
        public int AnnouncedDate { get; set; }
        public string NewEditionIsbn { get; set; }
        public string PreviousEditionIsbn { get; set; }
        public string AlternateIsbn { get; set; }
        public string BrandonHillCodes { get; set; }
        public string PriceGroup { get; set; }

        public string VendorNumber { get; set; }
        public string VendorInfo { get; set; }
        public bool IsReturnable { get; set; }

        public bool IsGsa { get; set; }
        public string ProductType { get; set; }
        public string AudiencePrimary { get; set; }
        public string AudienceSecondary { get; set; }

        public string Imprint { get; set; }
        public short DoodyRating { get; set; }
        //public string BisacCode { get; set; }
        //public string BisacDescription { get; set; }
        public string Synopsis { get; set; }
        public string PublisherPrelude { get; set; }

        public decimal[] Prices { get; set; }

        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public bool InStockOverride { get; set; }

        public int R2ResourceId { get; set; }

        public string Image { get; set; }

        public string CountryCode { get; set; }
        public string EIsbn { get; set; }
        public bool IsAvailableForSale { get; set; }
        public string AvailabilityCode { get; set; }

        public string TocFilename { get; set; }

		//public DoodyCoreTitleScore[] DoodyCoreTitleScores { get; set; }

		//public BackOrderData BackOrder { get; set; }
        public int OrderHistoryCount { get; set; }

        /// <summary>
        /// Calculates publication year based on the value of the copyright field and the publication date field
        /// </summary>
        public int PublicationYear
        {
            get
            {
                if (Copyright > 1752)
                {
                    return Copyright;
                }
                if (PublicationDate != null)
                {
                    DateTime pubDate;
                    if (DateTime.TryParse(PublicationDate, out pubDate))
                    {
                        return pubDate.Year;
                    }
                    Log.DebugFormat("Copyright: {0}, PublicationDate: {1}", Copyright, PublicationDate);
                    return 0;
                }
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PublicationYearText
        {
            get
            {
                int pubDateYear = PublicationYear;
                return (pubDateYear > 1752) ? pubDateYear.ToString() : "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string TitleSubTitleSpacer
        {
            get
            {
                return string.IsNullOrEmpty(SubTitle) ? "" : ":";
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public string CategoryName
		{
			get
			{
				if ((null != Category) && (!string.IsNullOrEmpty(Category.Name)))
				{
					return Category.Name;
				}
				return "";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string ProductStatus
		{
			get
			{
				if ((null != Status) && (!string.IsNullOrEmpty(Status.Description)))
				{
					return Status.Description;
				}
				return "";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PublisherName
		{
			get
			{
				if ((null != Publisher) && (Publisher.Id > 0) && (string.IsNullOrEmpty(Publisher.Name)))
				{
					return Publisher.Name;
				}
				return PublisherPrelude;
			}
		}

		///// <summary>
		///// 
		///// </summary>
		//public string QuantityOnHandText
		//{
		//    get
		//    {
		//        //QTY OH Business Rule:
		//        //  If QTY OH = less than 0, display 0

		//        //Status	Display
		//        //ACT	    - QTY OH XXXX
		//        //          - If qty = less than 0, display 0
		//        //          - If QTY OH = 0 display new line item: QTY on order from publisher = XXXX	
		//        //NYP	    - QTY OH = O 
		//        //          - Add Line Item: QTY on order from publisher = XXXX	
		//        //OP        - QTY OH XXXX
		//        //          - If qty = less than 0, display 0	
		//        //OS	    - QTY OH = XXX
		//        //          - If qty = less than 0, display 0
		//        //          - Add Text: Reprint Pending 	
		//        //OSI	    - QTY OH = XXX
		//        //          - If qty = less than 0, display 0
		//        //          - Add Text: No Reprint Date	
		//        //PC	    - QTY OH = XXX
		//        //          - If qty = less than 0, display 0	
		//        //PI	    - QTY OH = XXX
		//        //          - If qty = less than 0, display 0	
		//        //POD	    - QTY OH = XXX
		//        //          - If qty = less than 0, display 0	

		//        //Exceptions
		//        //All designated as QTY OH = In Stock
		//        //  - LWW titles
		//        //  - LWW Packages
		//        //  - Drop Ship Only Titles
		//        //Log.DebugFormat("Status.Code: {0}, Status.Id: {1}, QuantityOnHand: {2}", Status.Code, Status.Id, QuantityOnHand);
		//        //Log.DebugFormat("InStockOverride: {0}, Format: {1}", InStockOverride, Format);
		//        int quantityOnHand = (QuantityOnHand < 0) ? 0 : QuantityOnHand;

		//        string returnValue = null;
		//        //if ((productStatusCodes != ProductStatusCodes.NYP) && ((InStockOverride) || (Format.Equals("LWW Text Package"))))
		//        //if (((InStockOverride) || (Format.Equals("LWW Text Package")) || AvailabilityCode.Equals("D")) && (quantityOnHand <= 0))
		//        if ((quantityOnHand <= 0) &&
		//            ((InStockOverride) || 
		//             ((null != Format) && (Format.Equals("LWW Text Package"))) ||
		//             ((null != AvailabilityCode) && (AvailabilityCode.Equals("D")))))
		//        {
		//            returnValue = "In Stock";
		//        }
		//        else
		//        {
		//            returnValue = Status.GetQuantityOnHandText(quantityOnHand);
		//        }
		//        //Log.DebugFormat("QuantityOnHandText: {0}", returnValue);
		//        return returnValue;
		//    }
		//}

		///// <summary>
		///// 
		///// </summary>
		//public string QuantityOnOrderText
		//{
		//    get
		//    {
		//        string returnValue = null;

		//        //Status	Display
		//        //ACT	    - QTY OH XXXX
		//        //          - If qty = less than 0, display 0
		//        //          - If QTY OH = 0 display new line item: QTY on order from publisher = XXXX	
		//        //NYP	    - QTY OH = O 
		//        //          - Add Line Item: QTY on order from publisher = XXXX	
		//        //Log.DebugFormat("Status.Code: {0}, Status.Id: {1}, QuantityOnHand: {2}", Status.Code, Status.Id, QuantityOnHand);
		//        //Log.DebugFormat("InStockOverride: {0}, Format: {1}", InStockOverride, Format);
		//        int quantityOnHand = (QuantityOnHand < 0) ? 0 : QuantityOnHand;
		//        int quantityOnOrder = (QuantityOnOrder < 0) ? 0 : QuantityOnOrder;

		//        //ProductStatusCodes productStatusCodes = (ProductStatusCodes)Status.Id;
		//        //Log.DebugFormat("quantityOnHand: {0}, productStatusCodes: {1}", quantityOnHand, productStatusCodes);

		//        if ((!InStockOverride) && (!Format.Equals("LWW Text Package")))
		//        {
		//            if (quantityOnHand == 0)
		//            {
		//                if ((Status == ProductStatusFactory.Active) || (Status == ProductStatusFactory.NotYetPublished))
		//                {
		//                    returnValue = string.Format("{0}", quantityOnOrder);
		//                }
		//            }
		//        }

		//        //Log.DebugFormat("QuantityOnOrderText: {0}", returnValue);
		//        return returnValue;
		//    }
		//}


        /// <summary>
        /// 
        /// </summary>
        public string Dimensions
        {
            get
            {
                if ((Width > 0) && (Width > 0) && (Thickness > 0))
                {
                    return string.Format("{0:0.0#} x {1:0.0#} x {2:0.0#} in", Length * 0.01, Thickness * 0.01, Width * 0.01);
                }
                return "";
            }
        }

        /// <summary>
        /// Determines if the product is an R2 product or not.
        /// </summary>
        public bool IsR2Product
        {
            get
            {
                return (Sku.StartsWith("R2P"));
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Product()
        {
            Prices = new decimal[6];
			//Status = new StatusNone();
			//Publisher = new Publisher();
			//Category = new Category();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetBrandonHillDescriptions()
        {
            List<string> descriptions = new List<string>();
            if (string.IsNullOrEmpty(BrandonHillCodes))
            {
                return descriptions.ToArray();
            }

            string[] codes = BrandonHillCodes.Split(',');
            foreach (string code in codes)
            {
                if (code == "M1")
                {
                    descriptions.Add("Medical Select Title");
                    continue;
                }

                if (code == "M2")
                {
                    descriptions.Add("Core Select Title");
                    continue;
                }

                if (code == "M3")
                {
                    descriptions.Add("Min Select Title");
                    continue;
                }

                if (code == "N1")
                {
                    descriptions.Add("Nursing Select Title");
                    continue;
                }

                if (code == "N2")
                {
                    descriptions.Add("Nursing Core Title");
                    continue;
                }

                if (code == "A1")
                {
                    descriptions.Add("Allied Health Select Title");
                    continue;
                }

                if (code == "A2")
                {
                    descriptions.Add("Allied Health Core Title");
                }
            }
            return descriptions.ToArray();
        }

    }
}


