using System;
using System.Data.SqlClient;
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
       // public string Affiliation { get; set; }
        public int Copyright { get; set; }
        public string PublicationDate { get; set; }
        public string Format { get; set; }
        public string CategoryName { get; set; }
        public string PublisherName { get; set; }

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

                Copyright = GetInt32Value(reader, "copyright", -1);
                PublicationDate = GetStringValue(reader, "publicationDate", "");
                Format = GetStringValue(reader, "format", "");
                PublisherName = GetStringValue(reader, "publisherName");
                CategoryName = GetStringValue(reader, "categoryName", "");
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
                    Copyright = Copyright,
                    PublicationDate = PublicationDate,
                    Format = Format,
                    CategoryName = CategoryName,
                    PublisherName = PublisherName                    
                };

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


