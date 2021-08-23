using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        private static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        private List<string> IsbnsToFind { get; set; }

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
        [DeleteFileAttribute]
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
        [DeleteFileAttribute]
        public ActionResult Download2(string jsonMarcRecordRequestString)
        {
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            return View("Error", new IsbnsNotFound { Isbns = IsbnsToFind });
        }
        [DeleteFileAttribute]
        public ActionResult EbookDownload(string jsonMarcRecordRequestString)
        {
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!string.IsNullOrWhiteSpace(jsonMarcRecordRequestString))
            {
                try
                {
                    JsonMarcRecordRequest marcRecordRequest = new JavaScriptSerializer().Deserialize<JsonMarcRecordRequest>(jsonMarcRecordRequestString);
                    return EBookDownload(marcRecordRequest);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat("Download2 error: {0}", ex.Message);
                }

            }
            return View("Error", new IsbnsNotFound { Isbns = IsbnsToFind });
        }
        [DeleteFileAttribute]
        public ActionResult OclcEBookDownload(string jsonMarcRecordRequestString)
        {
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!string.IsNullOrWhiteSpace(jsonMarcRecordRequestString))
            {
                try
                {
                    JsonMarcRecordRequest marcRecordRequest = new JavaScriptSerializer().Deserialize<JsonMarcRecordRequest>(jsonMarcRecordRequestString);
                    return OclcEBookDownload(marcRecordRequest);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat("Download2 error: {0}", ex.Message);
                }

            }
            return View("Error", new IsbnsNotFound { Isbns = IsbnsToFind });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcRecordRequest"></param>
        /// <returns></returns>
        [DeleteFileAttribute]
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
                Log.InfoFormat("New JsonMarcRecordRequest-- AccountNumber: {0} | File Format: {1}", marcRecordRequest.AccountNumber, marcRecordRequest.Format);
                IEnumerable<string> isbnsToFind = marcRecordRequest.IsbnAndCustomerFields != null
                                      ? marcRecordRequest.IsbnAndCustomerFields.Select(x => x.IsbnOrSku)
                                      : new List<string>();
                if (isbnsToFind.Any())
                {
                    IsbnsToFind = isbnsToFind.ToList();
                }
                else
                {
                    var isbnsNotFound = new IsbnsNotFound { Isbns = null };

                    Log.ErrorFormat("The following ISBNs could not be found : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }
                //IsbnsToFind = marcRecordRequest.IsbnAndCustomerFields.Select(x => x.IsbnOrSku).ToList();
                Log.InfoFormat("Number of ISBN/Sku to find: {0}", IsbnsToFind.Count);

                MarcRecordQueries marcRecordQueries = new MarcRecordQueries(Log);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<PrintMarcRecordFile> files = marcRecordQueries.GetDailyMarcRecordFiles(IsbnsToFind, marcRecordRequest.IsR2Request, marcRecordRequest.IsRittenhouseRequest);
                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>Marc files found: {0} || Time it took: {1}ms", files.Count, stopwatch.ElapsedMilliseconds);

                if (files.Count == 0)
                {
                    var isbnsNotFound = new IsbnsNotFound();

                    if (IsbnsToFind != null && IsbnsToFind.Count > 0)
                    {
                        isbnsNotFound.Isbns = IsbnsToFind;
                    }

                    Log.ErrorFormat("The following ISBNs could not be found in the database : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                if (marcRecordRequest.IsDeleteFile)
                {
                    files = _marcRecordService.CreateDeleteMarcRecordFiles(files);
                }

                stopwatch = new Stopwatch();
                stopwatch.Start();

                List<string> marcRecordPaths = _marcRecordService.WriteMarcRecordFiles(marcRecordRequest, files);

                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>WriteMarcRecordFiles Time it took: {0}ms", stopwatch.ElapsedMilliseconds);

                if (marcRecordPaths.Count == 0)
                {
                    var isbnsNotFound = new IsbnsNotFound { Isbns = IsbnsToFind };

                    Log.ErrorFormat("The following ISBNs could not be found : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                stopwatch = new Stopwatch();
                stopwatch.Start();

                var filePath = _marcRecordService.GetMergedMarcRecordsFilePath(marcRecordRequest, marcRecordPaths, false);

                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>GetMergedMarcRecordsFilePath Time it took: {0}ms", stopwatch.ElapsedMilliseconds);

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
                    Log.InfoFormat("File to stream back: {0}", fileDownloadName);

                    return File(fileStream, mimeType, fileDownloadName);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error in Download: {0}", ex);
            }
            return View("Error");
        }
        [DeleteFileAttribute]
        private ActionResult EBookDownload(JsonMarcRecordRequest marcRecordRequest)
        {
            if (marcRecordRequest != null && !string.IsNullOrWhiteSpace(marcRecordRequest.AccountNumber))
            {
                //var timeOut = Server.ScriptTimeout;
                //// 1 hour = 3600 seconds
                //// 30 minutes = 1800 seconds
                //// 15 minutes = 900 seconds
                Server.ScriptTimeout = 900;
            }
            else
            {
                return View("Error");
            }
            try
            {
                Log.InfoFormat("New JsonMarcRecordRequest-- AccountNumber: {0} | File Format: {1}", marcRecordRequest.AccountNumber, marcRecordRequest.Format);
                List<string> isbnsToFind = marcRecordRequest.IsbnAndCustomerFields != null
                                      ? marcRecordRequest.IsbnAndCustomerFields.Select(x => x.IsbnOrSku).ToList()
                                      : new List<string>();
                if (isbnsToFind.Any())
                {
                    IsbnsToFind = isbnsToFind.ToList();
                }
                else
                {
                    var isbnsNotFound = new IsbnsNotFound { Isbns = null };

                    Log.ErrorFormat("The following ISBNs could not be found : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                Log.InfoFormat("Number of ISBN/Sku to find: {0}", IsbnsToFind.Count);

                MarcRecordQueries marcRecordQueries = new MarcRecordQueries(Log);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<DigitalMarcRecordFile> files = marcRecordQueries.GetEBookMarcRecords(IsbnsToFind);
                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>Marc files found: {0} || Time it took: {1}ms", files.Count, stopwatch.ElapsedMilliseconds);

                if (files.Count == 0)
                {
                    var isbnsNotFound = new IsbnsNotFound();

                    if (IsbnsToFind != null && IsbnsToFind.Count > 0)
                    {
                        isbnsNotFound.Isbns = IsbnsToFind;
                    }

                    Log.ErrorFormat("The following ISBNs could not be found in the database : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                files = AlterDigitalMarcRecords(files);
                
                if (marcRecordRequest.IsDeleteFile)
                {
                    files = _marcRecordService.CreateDeleteMarcRecordFiles(files);
                }

                string urlPrefix = null;
                var customMarcFields = marcRecordRequest.CustomMarcFields;
                if (customMarcFields != null && customMarcFields.Any())
                {
                    foreach (var jsonCustomMarcField in customMarcFields.Where(x => x.FieldNumber == 856))
                    {
                        urlPrefix = jsonCustomMarcField.FieldValue;
                    }
                }

                string urlSuffix = "";
                //var customMarcFields = marcRecordRequest.CustomMarcFields;
                if (customMarcFields != null && customMarcFields.Any())
                {
                    foreach (var jsonCustomMarcField in customMarcFields.Where(x => x.FieldNumber == 8566))
                    {
                        urlSuffix = jsonCustomMarcField.FieldValue;
                    }
                }

                //urlSuffix
                stopwatch = new Stopwatch();
                stopwatch.Start();

                List<string> marcRecordPaths = _marcRecordService.WriteDigitalMarcRecordFiles(files, marcRecordRequest.AccountNumber, urlPrefix, urlSuffix);

                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>WriteMarcRecordFiles Time it took: {0}ms", stopwatch.ElapsedMilliseconds);

                if (marcRecordPaths.Count == 0)
                {
                    var isbnsNotFound = new IsbnsNotFound { Isbns = IsbnsToFind };

                    Log.ErrorFormat("The following ISBNs could not be found : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                stopwatch = new Stopwatch();
                stopwatch.Start();

                var filePath = _marcRecordService.GetMergedMarcRecordsFilePath(marcRecordRequest, marcRecordPaths, true);

                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>GetMergedMarcRecordsFilePath Time it took: {0}ms", stopwatch.ElapsedMilliseconds);

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
                        System.IO.File.Delete(filePath);
                        return View("Ftp");
                    }
                }
                else
                {
                    string fileStream = filePath;
                    const string mimeType = "text/plain";
                    string fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.{3}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, marcRecordRequest.Format);
                    Log.InfoFormat("File to stream back: {0}", fileDownloadName);

                    return File(fileStream, mimeType, fileDownloadName);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error in Download: {0}", ex);
            }
            return View("Error");
        }

        private List<DigitalMarcRecordFile> AlterDigitalMarcRecords(List<DigitalMarcRecordFile> files)
        {
            var alteredFiles = new List<DigitalMarcRecordFile>();

            if (files != null && files.Any())
            {
                foreach (var digitalMarcRecordFile in files)
                {
                    var marcRecord = digitalMarcRecordFile.MarcFile.Replace(@"\\\\\\\\\\\\\\\\\\\\\\\\eng\d", @"\\\\\\\\\\\\o\\\\\\\\\\\eng\d");
                    digitalMarcRecordFile.MarcFile = marcRecord;
                    alteredFiles.Add(digitalMarcRecordFile);
                }
            }

            return alteredFiles;
        }


        [DeleteFileAttribute]
        private ActionResult OclcEBookDownload(JsonMarcRecordRequest marcRecordRequest)
        {
            if (marcRecordRequest != null && !string.IsNullOrWhiteSpace(marcRecordRequest.AccountNumber))
            {
                //var timeOut = Server.ScriptTimeout;
                //// 1 hour = 3600 seconds
                //// 30 minutes = 1800 seconds
                //// 15 minutes = 900 seconds
                Server.ScriptTimeout = 900;
            }
            else
            {
                return View("Error");
            }
            try
            {
                Log.InfoFormat("New JsonMarcRecordRequest-- AccountNumber: {0} | File Format: {1}", marcRecordRequest.AccountNumber, marcRecordRequest.Format);
                List<string> isbnsToFind = marcRecordRequest.IsbnAndCustomerFields != null
                                      ? marcRecordRequest.IsbnAndCustomerFields.Select(x => x.IsbnOrSku).ToList()
                                      : new List<string>();
                if (isbnsToFind.Any())
                {
                    IsbnsToFind = isbnsToFind.ToList();
                }
                else
                {
                    var isbnsNotFound = new IsbnsNotFound { Isbns = null };

                    Log.ErrorFormat("The following ISBNs could not be found : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                Log.InfoFormat("Number of ISBN/Sku to find: {0}", IsbnsToFind.Count);

                MarcRecordQueries marcRecordQueries = new MarcRecordQueries(Log);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<DigitalMarcRecordFile> files = marcRecordQueries.GetOclcEBookMarcRecords(IsbnsToFind);
                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>Marc files found: {0} || Time it took: {1}ms", files.Count, stopwatch.ElapsedMilliseconds);

                files = AlterDigitalMarcRecords(files);

                if (files.Count == 0)
                {
                    var isbnsNotFound = new IsbnsNotFound();

                    if (IsbnsToFind != null && IsbnsToFind.Count > 0)
                    {
                        isbnsNotFound.Isbns = IsbnsToFind;
                    }

                    Log.ErrorFormat("The following ISBNs could not be found in the database : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                if (marcRecordRequest.IsDeleteFile)
                {
                    files = _marcRecordService.CreateDeleteMarcRecordFiles(files);
                }

                stopwatch = new Stopwatch();
                stopwatch.Start();

                List<string> marcRecordPaths = _marcRecordService.WriteDigitalMarcRecordFiles(files, marcRecordRequest.AccountNumber);

                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>WriteMarcRecordFiles Time it took: {0}ms", stopwatch.ElapsedMilliseconds);

                if (marcRecordPaths.Count == 0)
                {
                    var isbnsNotFound = new IsbnsNotFound { Isbns = IsbnsToFind };

                    Log.ErrorFormat("The following ISBNs could not be found : {0}", isbnsNotFound.ToString());

                    return View("Error", isbnsNotFound);
                }

                stopwatch = new Stopwatch();
                stopwatch.Start();

                var filePath = _marcRecordService.GetMergedMarcRecordsFilePath(marcRecordRequest, marcRecordPaths, false);

                stopwatch.Stop();
                Log.InfoFormat(">>>>>>>>>>>>GetMergedMarcRecordsFilePath Time it took: {0}ms", stopwatch.ElapsedMilliseconds);

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
                    Log.InfoFormat("File to stream back: {0}", fileDownloadName);

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

