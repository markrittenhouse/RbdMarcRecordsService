using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using MarcRecordServiceSite.Infrastructure.NHibernate.Queries;
using MarcRecordServiceSite.Models;
using log4net;

namespace MarcRecordServiceSite.Controllers
{
    public class MarcRecordsController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		//
        // GET: /MarcRecords/

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public ActionResult Get()
		//{
		//    return View();
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult Get(MarcRecordRequestItem item)
		{
			Log.DebugFormat("isbn10: {0}, isbn13: {1}, sku: {2}", item.Isbn10, item.Isbn13, item.Sku);

			if (!string.IsNullOrEmpty(item.Isbn13))
			{
				IEnumerable<MarcRecord> records = MarcRecordQueries.GetMarcRecords(item.Isbn13);

				foreach (MarcRecord marcRecord in records)
				{
					Log.DebugFormat("id: {0}, isbn10: {1}, isbn13: {2}, sku: {3}", marcRecord.Id, marcRecord.Isbn10, marcRecord.Isbn13, marcRecord.Sku);

					foreach (MarcRecordProvider provider in marcRecord.Providers)
					{
						Log.DebugFormat("   Provider - id: {0}", provider.Id);
					}
				}
			}

			return View(item);
		}

	}
}
