using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;
using MarcRecordServiceSite.Infrastructure;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using MarcRecordServiceSite.Models;

namespace MarcRecordServiceSite.Controllers
{
    public class PingController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IQueryable<MarcRecordProvider> _marcRecordProviders;

        public PingController(IQueryable<MarcRecordProvider> marcRecordProviders )
        {
            _marcRecordProviders = marcRecordProviders;
        }

        //
        // GET: /Ping/

        public ActionResult Index()
        {
            var model = new PingData { ClientIpAddress = GetIpAddress(Request) };


            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                //Ping ping = _pings.Single(x => x.Id == 1);

                //model.DatabaseStatus = ping.StatusCode;

                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds > 250)
                {
                    Log.WarnFormat("Ping query took longer than 250 ms, query time: {0} ms", stopwatch.ElapsedMilliseconds);
                }
                else if (stopwatch.ElapsedMilliseconds > 2000)
                {
                    Log.ErrorFormat("SERVER PERFORMANCE ALERT - Ping query took longer than 2 seconds, query time: {0} ms", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                model.DatabaseStatus = "R2v2DbStatusException";
                Log.Error(ex.Message, ex);
            }

            model.MachineName = Environment.MachineName;
            Session.Abandon();
            Log.DebugFormat("DatabaseStatus: {0}, ClientIpAddress: {1}, Version: {2}, MachineName: {3}",
                model.DatabaseStatus, model.ClientIpAddress, model.Version, model.MachineName);
            return View(model);
        }

        private static string GetIpAddress(HttpRequestBase request)
        {
            return string.IsNullOrEmpty(request.UserHostAddress) ? IPAddress.Loopback.GetIPv4Address() : IPAddress.Parse(request.UserHostAddress).GetIPv4Address();
        }

    }
}
