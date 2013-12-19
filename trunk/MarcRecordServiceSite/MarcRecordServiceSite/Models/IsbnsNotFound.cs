﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MarcRecordServiceSite.Models
{
    public class IsbnsNotFound
    {
        public List<string> Isbns { get; set; }

        public new string ToString()
        {
            var sb = new StringBuilder();
            foreach (var isbn in Isbns)
            {
                sb.AppendFormat("{0}||", isbn);
            }
            return sb.ToString();
        }
    }
}