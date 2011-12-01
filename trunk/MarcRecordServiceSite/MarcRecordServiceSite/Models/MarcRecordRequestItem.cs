using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Models
{
	public class MarcRecordRequestItem
	{
		public string Isbn10 { get; set; }
		public string Isbn13 { get; set; }
		public string Sku { get; set; }
	}
}