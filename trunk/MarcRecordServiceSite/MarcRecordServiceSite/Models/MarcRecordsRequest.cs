using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarcRecordServiceSite.Models
{
	public class MarcRecordsRequest
	{
        [Required]
        [Display(Name = "Isbn or Sku")]
        public string ItemIdentifier { get; set; }
	}
}