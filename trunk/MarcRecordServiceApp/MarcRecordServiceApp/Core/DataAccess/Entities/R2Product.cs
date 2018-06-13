using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.MarcRecord;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class R2Resource : FactoryBase, IDataEntity
    {
        public int ResourceId { get; set; }
        public string Isbn { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string EIsbn { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Authors { get; set; }
        public string FirstAuthor { get; set; }

        public int PublicationYear
        {
            get
            {
                return PublicationDate.Year;
            } 
        }

        public DateTime PublicationDate { get; set; }
        public string PublisherName { get; set; }

        public IEnumerable<R2Author> AuthorList { get; set; }
        public IEnumerable<R2Category> Categories { get; set; }
        public IEnumerable<R2SubCategory> SubCategories { get; set; }

        public List<AdditionalField> AdditionalFields { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                ResourceId = GetInt32Value(reader, "iResourceId", -1);
                Isbn = GetStringValue(reader, "vchResourceISBN");
                Isbn10 = GetStringValue(reader, "vchIsbn10");
                Isbn13 = GetStringValue(reader, "vchIsbn13");
                EIsbn = GetStringValue(reader, "vchEIsbn");
                Title = RemoveControlCharacters(GetStringValue(reader, "vchResourceTitle"));
                SubTitle = RemoveControlCharacters(GetStringValue(reader, "vchResourceSubTitle"));
                Authors = RemoveControlCharacters(GetStringValue(reader, "vchResourceAuthors"));
                PublicationDate = GetDateValue(reader, "dtResourcePublicationDate", DateTime.Now);
                PublisherName = GetStringValue(reader, "vchPublisherName");
                FirstAuthor = GetStringValue(reader, "vchResourceSortAuthor");

            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }

        public string ToDebugString()
        {
            return new StringBuilder()
            .Append("R2Resource = [")
                .AppendFormat("ResourceId: {0}", ResourceId)
                .AppendFormat("Isbn: {0}", Isbn)
                .AppendFormat("Isbn10: {0}", Isbn10)
                .AppendFormat("Isbn13: {0}", Isbn13)
                .AppendFormat("EIsbn: {0}", EIsbn)
                .AppendFormat("Title: {0}", Title)
                .AppendFormat("SubTitle: {0}", SubTitle)
                .AppendFormat("Authors: {0}", Authors)
                .AppendFormat("FirstAuthor: {0}", FirstAuthor)
                .AppendFormat("PublicationYear: {0}", PublicationYear)
                .AppendFormat("PublisherName: {0}", PublisherName)
                .AppendFormat("AuthorList.Count: {0}", AuthorList != null ? AuthorList.Count() : 0)
                .AppendFormat("Categories.Count: {0}", Categories != null ? Categories.Count() : 0)
                .AppendFormat("SubCategories.Count: {0}", SubCategories != null ? SubCategories.Count() : 0)
                .Append("]")
                .ToString();


        }
    }

    public class R2Author : FactoryBase, IDataEntity
    {
        public int ResourceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Degree { get; set; }
        public int Order { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                ResourceId = GetInt32Value(reader, "iResourceId", -1);
                FirstName = GetStringValue(reader, "vchFirstName");
                LastName = GetStringValue(reader, "vchLastName");
                MiddleName = GetStringValue(reader, "vchMiddleName");
                Degree = GetStringValue(reader, "vchDegree");
                Order = GetByteValue(reader, "tiAuthorOrder", 0);

            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }

        public string ToDisplayName()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                return LastName;
            }
            return string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
        }
    }

    public class R2Category : FactoryBase, IDataEntity
    {
        public int ResourceId { get; set; }
        //public int CategoryId { get; set; }
        public string Category { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                ResourceId = GetInt32Value(reader, "iResourceId", -1);
                //CategoryId = GetInt32Value(reader, "iPracticeAreaId", 0);
                Category = GetStringValue(reader, "vchPracticeAreaName");

            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }

    public class R2SubCategory : FactoryBase, IDataEntity
    {
        public int ResourceId { get; set; }
        //public int SubCategoryId { get; set; }
        public string SubCategory { get; set; }

        public void Populate(SqlDataReader reader)
        {
            try
            {
                ResourceId = GetInt32Value(reader, "iResourceId", -1);
               // SubCategoryId = GetInt32Value(reader, "iSpecialtyId", 0);
                SubCategory = GetStringValue(reader, "vchSpecialtyName");

            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
                throw;
            }
        }
    }
}
