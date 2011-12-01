using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarcRecordServiceSite.Controllers
{
    public class DefaultController : Controller
    {
		/// <summary>
		/// GET: /Default/
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
	}
}
