using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MARCEngine5;
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

			string workingDirectory = System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];
			Log.DebugFormat("workingDirectory: {0}", workingDirectory);

			if (!string.IsNullOrEmpty(item.Isbn13))
			{
				IEnumerable<MarcRecord> records = MarcRecordQueries.GetMarcRecords2(item.Isbn13);

				MARC21 marc21 = new MARC21();

				foreach (MarcRecord marcRecord in records)
				{
					Log.DebugFormat("id: {0}, isbn10: {1}, isbn13: {2}, sku: {3}", marcRecord.Id, marcRecord.Isbn10, marcRecord.Isbn13, marcRecord.Sku);

					foreach (MarcRecordProvider provider in marcRecord.Providers)
					{
						Log.DebugFormat("   Provider - id: {0}", provider.Id);

						foreach (MarcRecordFile file in provider.Files)
						{
							Log.DebugFormat("      Id: {0}, FileData: {1}", file.Id, file.FileData);

							string filePath = string.Format("{0}\\{1}_{2}.{3}", workingDirectory, item.Isbn13, provider.MarcRecordProviderTypeId,
							                                (file.MarcRecordFileTypeId == 1) ? "mrc" : (file.MarcRecordFileTypeId == 2) ? "mrk" : "xml");

							System.IO.File.WriteAllText(filePath, file.FileData);
							System.IO.File.WriteAllText(filePath.Replace(item.Isbn13, string.Format("_{0}", item.Isbn13)), file.FileData);

							marc21.Delete_Field(filePath, "999");
						}
					}
				}
			}

			return View(item);
		}

	}
}
