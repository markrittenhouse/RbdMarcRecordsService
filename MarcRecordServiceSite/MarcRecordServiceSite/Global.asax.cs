using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using NHibernate;
using log4net;
using log4net.Config;

namespace MarcRecordServiceSite
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static ISessionFactory sessionFactory;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ISession CreateSession()
		{
			var session = sessionFactory.OpenSession();
			return session;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filters"></param>
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="routes"></param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("favicon.ico");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Default", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

            routes.MapRoute("Robots.txt",
                "robots.txt",
                new { controller = "Default", action = "Robots" });
		}

		/// <summary>
		/// 
		/// </summary>
		protected void Application_Start()
		{
			XmlConfigurator.Configure();

			Log.Info("*******************************");
			Log.Info("***** APPLICATION STARTED *****");
			Log.Info("*******************************");
			try
			{
				sessionFactory = CreateSessionFactory();
				//Log.Info("SessionFactory Created");

				AreaRegistration.RegisterAllAreas();
				Log.Info("RegisterAllAreas");

				RegisterGlobalFilters(GlobalFilters.Filters);
				Log.Info("RegisterGlobalFilters()");
				RegisterRoutes(RouteTable.Routes);
				Log.Info("RegisterRoutes()");
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
				throw;
			}
			Log.Info("Application_Start() <<<");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			Context.Items.Add("RequestTimestamp", DateTime.Now);

			StringBuilder requestInfo = new StringBuilder()
				.AppendFormat(">>>>>>>>>> {0}, IP: {1}", Request.RawUrl, Request.UserHostAddress);

			HttpCookie aspDotNetCookie = Request.Cookies["ASP.NET_SessionId"];
			if (aspDotNetCookie != null)
			{
				requestInfo.AppendFormat(", aspDotNetCookie: {0}", aspDotNetCookie.Value);
			}
			Log.Info(requestInfo);
			Context.Items.Add("TimedPageRequest", true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs
			Exception ex = Server.GetLastError();
			if (ex != null)
			{
				Log.Error(ex.Message, ex);
			}
			else
			{
				Log.Error("NULL EXCEPTION");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Session_End(object sender, EventArgs e)
		{
			Log.Info("Session End <<<<");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_End(object sender, EventArgs e)
		{
			Log.Info("Application End <<<<<< --");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			try
			{
				bool timedPageRequest = ((Context != null) && (Context.Items["TimedPageRequest"] != null)) ? (bool)Context.Items["TimedPageRequest"] : false;
				if (timedPageRequest)
				{
					DateTime requestStartTime = (DateTime)Context.Items["RequestTimestamp"];
					TimeSpan requestTimeSpan = DateTime.Now.Subtract(requestStartTime);

					StringBuilder requestInfo = new StringBuilder()
						.AppendFormat("<<<<<<<<<< {0:0.000}, {1}, IP: {2}", requestTimeSpan.TotalSeconds, Request.RawUrl, Request.UserHostAddress);

					HttpCookie aspDotNetCookie = Request.Cookies["ASP.NET_SessionId"];
					if (aspDotNetCookie != null)
					{
						requestInfo.AppendFormat(", aspDotNetCookie: {0}", aspDotNetCookie.Value);
					}

					if (requestTimeSpan.TotalSeconds < 5.0)
					{
						Log.Info(requestInfo);
					}
					else if (requestTimeSpan.TotalSeconds < 10.0)
					{
						Log.Warn(requestInfo);
					}
					else
					{
						Log.Warn(requestInfo);
						StringBuilder errorMsg = new StringBuilder()
							.AppendLine("The requested page took more than 10 seconds to render.")
							.AppendFormat("Page: {0}", Request.RawUrl).AppendLine()
							.AppendFormat("Page execution time: {0:0.000}", requestTimeSpan.TotalSeconds).AppendLine()
							.AppendLine()
							.AppendLine("This message does not indicate there was an error with the site. It indicates only that requested page took an unusually long time to be rendered. Please monitor to make sure the site is performing as expected.")
							.AppendLine()
							.AppendFormat("Addition debug information: {0}", requestInfo).AppendLine();
						Log.Error(errorMsg);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private static ISessionFactory CreateSessionFactory()
		{
			try
			{
				ConnectionStringSettings dbConnectionString = WebConfigurationManager.ConnectionStrings["MarcRecordsDbConnection"];

				return Fluently.Configure()
					.Database(MsSqlConfiguration.MsSql2008.ConnectionString(dbConnectionString.ConnectionString)
								.Cache(c => c.UseQueryCache()).ShowSql())
					.Mappings(m => m.FluentMappings.AddFromAssemblyOf<MarcRecord>())
					.Diagnostics(x => x.Enable())
					.BuildSessionFactory();
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
				throw;
			}
		}
	}
}