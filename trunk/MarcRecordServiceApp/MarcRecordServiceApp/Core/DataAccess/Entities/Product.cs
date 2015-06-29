using System;
using MarcRecordServiceApp.Core.DataAccess.Entities.Base;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
	public class Product : EntityBase 
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Authors { get; set; }
        public string FirstAuthorLastName { get; set; }

		
        public int Copyright { get; set; }
        public string PublicationDate { get; set; }
        public string Format { get; set; }
        public string Edition { get; set; }

        public string CategoryName { get; set; }
        public string PublisherName { get; set; }		


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
        /// Determines if the product is an R2 product or not.
        /// </summary>
        public bool IsR2Product
        {
            get
            {
                return (Sku.StartsWith("R2P"));
            }
        }
    }
}


