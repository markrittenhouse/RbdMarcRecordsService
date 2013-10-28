﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MARCEngine5;
using MarcRecordServiceSite.Infrastructure;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using MarcRecordServiceSite.Infrastructure.NHibernate.Queries;
using MarcRecordServiceSite.Models;
using log4net;

namespace MarcRecordServiceSite.Controllers
{
    public class MarcRecordsController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string FieldsToRemove = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                                      "=900\t=901\t=902\t=903\t=904\t=905\t=906\t=907\t=908\t=909",
                                      "\t=910\t=911\t=912\t=913\t=914\t=915\t=916\t=917\t=918\t=919",
                                      "\t=920\t=921\t=922\t=923\t=924\t=925\t=926\t=927\t=928\t=929",
                                      "\t=930\t=931\t=932\t=933\t=934\t=935\t=936\t=937\t=938\t=939",
                                      "\t=940\t=941\t=942\t=943\t=944\t=945\t=946\t=947\t=948\t=949",
                                      "\t=950\t=951\t=952\t=953\t=954\t=955\t=956\t=957\t=958\t=959",
                                      "\t=960\t=961\t=962\t=963\t=964\t=965\t=966\t=967\t=968\t=969",
                                      "\t=970\t=971\t=972\t=973\t=974\t=975\t=976\t=977\t=978\t=979",
                                      "\t=980\t=981\t=982\t=983\t=984\t=985\t=986\t=987\t=988\t=989",
                                      "\t=990\t=991\t=992\t=993\t=994\t=995\t=996\t=997\t=998\t=999");

        private readonly MarcRecordService _marcRecordService;

        public MarcRecordsController()
        {
            _marcRecordService = new MarcRecordService(Log);
        }

        /// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
		{
            return View();
        }

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
                MARC21 marc21 = new MARC21();

			    MarcRecordFile file = MarcRecordQueries.GetMnemonicMarcFileForEditing2(item.Isbn13);
                if (file == null && !string.IsNullOrWhiteSpace(item.Isbn10))
                {
                    file = MarcRecordQueries.GetMnemonicMarcFileForEditing2(item.Isbn10);
                }
                if (file == null && !string.IsNullOrWhiteSpace(item.Sku))
                {
                    file = MarcRecordQueries.GetMnemonicMarcFileForEditing2(item.Sku);
                }

                if (file != null)
			    {
                    Log.DebugFormat("Id: {0}, FileData: {1}", file.Id, file.FileData);
                    string filePath = string.Format("{0}\\{1}_{2}.{3}", workingDirectory, item.Isbn13, file.Provider.Id,
                                                    (file.MarcRecordFileTypeId == 1) ? "mrc" : (file.MarcRecordFileTypeId == 2) ? "mrk" : "xml");

                    // Write to file so we can edit the file. 
                    System.IO.File.WriteAllText(filePath, file.FileData);

                    marc21.Delete_Field(filePath, FieldsToRemove);

			        string fileName = string.Format("MarcRecords-{0}_{1}_{2}.mrk", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    //Streams back the file to the client
                    Response.Clear();
                    Response.AddHeader("Cache-control", "no-cache");
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                    Response.Charset = "";
                    Response.ContentType = "text/plain";
                    Response.Write(new StreamReader(filePath).ReadToEnd());
                    Response.End();
			    }                             
			}

			return View(item);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonMarcRecordRequestString"></param>
        /// <returns></returns>
        public ActionResult Download2(string jsonMarcRecordRequestString)
        {
            if (!string.IsNullOrWhiteSpace(jsonMarcRecordRequestString))
            {
                try
                {
                    JsonMarcRecordRequest marcRecordRequest = new JavaScriptSerializer().Deserialize<JsonMarcRecordRequest>(jsonMarcRecordRequestString);
                    return Download(marcRecordRequest);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat("Download2 error: {0}", ex.Message);
                }
                
            }
            return View("Error");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcRecordRequest"></param>
        /// <returns></returns>
        public ActionResult Download(JsonMarcRecordRequest marcRecordRequest)
        {
            if (marcRecordRequest != null && !string.IsNullOrWhiteSpace(marcRecordRequest.AccountNumber))
            {
                //var timeOut = Server.ScriptTimeout;
                //// 1 hour = 3600 seconds
                //// 30 minutes = 1800 seconds
                //// 15 minutes = 900 seconds
                Server.ScriptTimeout = 900;
            }
            try
            {
                Log.DebugFormat("New JsonMarcRecordRequest-- AccountNumber: {0} | File Format: {1}", marcRecordRequest.AccountNumber, marcRecordRequest.Format);
                List<string> isbnsToFind = marcRecordRequest.IsbnAndCustomerFields.Select(jsonIsbnAndCustomerFields => jsonIsbnAndCustomerFields.IsbnOrSku).ToList();
                Log.DebugFormat("Number of ISBN/Sku to find: {0}", isbnsToFind.Count);

                List<DailyMarcRecordFile> files = MarcRecordQueries.GetDailyMarcRecordFiles(isbnsToFind);

                Log.DebugFormat(">>>>>>>>>>>>Marc files found: {0}", files.Count);

                if (marcRecordRequest.IsDeleteFile)
                {
                    files = _marcRecordService.CreateDeleteMarcRecordFiles(files);
                }

                List<string> marcRecordPaths = _marcRecordService.WriteMarcRecordFiles(marcRecordRequest, files);
                
                if (marcRecordPaths.Count == 0)
                {
                    Log.Debug("Zero Files found");
                    return View("Error");
                }

                var filePath = _marcRecordService.GetMergedMarcRecordsFilePath(marcRecordRequest, marcRecordPaths);
                

                //var ftpCredientials = new MarcFtpCredientials
                //                          {
                //                              Host = "ftp://technoserv04.technotects.com/Kens_Test_Folder",
                //                              UserName = "kshaberle",
                //                              Password = "Techno2008"
                //                          };

                //marcRecordRequest.FtpCredientials = ftpCredientials;

                if (marcRecordRequest.FtpCredientials != null)
                {
                    FtpService ftpService = new FtpService(marcRecordRequest.FtpCredientials);

                    if (ftpService.IsEligibleForFtp)
                    {
                        ftpService.UploadFileToFtp(filePath);
                        return View("Ftp");
                    }
                }
                else
                {
                    string fileStream = filePath;
                    const string mimeType = "text/plain";
                    string fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.{3}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, marcRecordRequest.Format);
                    Log.DebugFormat("File to stream back: {0}", fileDownloadName);

                    return File(fileStream, mimeType, fileDownloadName);
                }               
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error in Download: {0}", ex);
            }
            return View("Error");
        }
	}
}

