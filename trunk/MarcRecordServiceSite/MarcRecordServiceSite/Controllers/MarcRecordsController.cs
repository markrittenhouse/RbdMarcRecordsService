using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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

                Log.DebugFormat("Id: {0}, FileData: {1}", file.Id, file.FileData);

                string filePath = string.Format("{0}\\{1}_{2}.{3}", workingDirectory, item.Isbn13, file.Provider.Id,
                                                (file.MarcRecordFileTypeId == 1) ? "mrc" : (file.MarcRecordFileTypeId == 2) ? "mrk" : "xml");

                // Write to file so we can edit the file. 
                System.IO.File.WriteAllText(filePath, file.FileData);

                marc21.Delete_Field(filePath, FieldsToRemove);
#region Original Method
				//IEnumerable<MarcRecord> records = MarcRecordQueries.GetMarcRecords2(item.Isbn13);

				//MARC21 marc21 = new MARC21();

                //foreach (MarcRecord marcRecord in records)
                //{
                //    Log.DebugFormat("id: {0}, isbn10: {1}, isbn13: {2}, sku: {3}", marcRecord.Id, marcRecord.Isbn10, marcRecord.Isbn13, marcRecord.Sku);

                //    // For providers we only need 1 provider with the highest priority from MarcRecordProviderType if a marcfile exists for it
                //    foreach (MarcRecordProvider provider in marcRecord.Providers)
                //    {
                //        Log.DebugFormat("   Provider - id: {0}", provider.Id);

                //        //Only need the .MRK version of the file. The Marc21 delete field only works on mnemonic files (MRK version)                        
                //        foreach (MarcRecordFile file in provider.Files)
                //        {
                //            Log.DebugFormat("      Id: {0}, FileData: {1}", file.Id, file.FileData);

                //            string filePath = string.Format("{0}\\{1}_{2}.{3}", workingDirectory, item.Isbn13, provider.MarcRecordProviderTypeId,
                //                                            (file.MarcRecordFileTypeId == 1) ? "mrc" : (file.MarcRecordFileTypeId == 2) ? "mrk" : "xml");

                //            // Write to file so we can edit the file. 
                //            System.IO.File.WriteAllText(filePath, file.FileData);
                //            //System.IO.File.WriteAllText(filePath.Replace(item.Isbn13, string.Format("_{0}", item.Isbn13)), file.FileData);

                //            marc21.Delete_Field(filePath, FieldsToRemove);
						    
                //        }

                        
                //    }
                //}
#endregion

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
                JsonMarcRecordRequest marcRecordRequest = new JavaScriptSerializer().Deserialize<JsonMarcRecordRequest>(jsonMarcRecordRequestString);
                return Download(marcRecordRequest);
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
            try
            {
                Log.DebugFormat("New JsonMarcRecordRequest-- AccountNumber: {0} | File Format: {1}", marcRecordRequest.AccountNumber, marcRecordRequest.Format);
                List<string> isbnsToFind = marcRecordRequest.IsbnAndCustomerFields.Select(jsonIsbnAndCustomerFields => jsonIsbnAndCustomerFields.IsbnOrSku).ToList();
                Log.DebugFormat("Number of ISBN/Sku to find: {0}", isbnsToFind.Count);

                List<MarcRecordFile> files = MarcRecordQueries.GetMnemonicMarcFilesForEditing2(isbnsToFind);

                if (marcRecordRequest.IsDeleteFile)
                {
                    foreach (MarcRecordFile marcRecordFile in files)
                    {
                        var sb = new StringBuilder(marcRecordFile.FileData);
                        sb.Remove(11, 12);
                        sb.Insert(11, 'd');
                        marcRecordFile.FileData = sb.ToString();
                    }
                }

                string lastIsbn = "";
                List<string> marcRecordPaths = new List<string>();

                foreach (MarcRecordFile file in files)
                {
                    if (lastIsbn == file.Provider.MarcRecord.Isbn13 || lastIsbn == file.Provider.MarcRecord.Isbn10 || lastIsbn == file.Provider.MarcRecord.Sku) continue;

                    foreach (var jsonIsbnAndCustomerField in marcRecordRequest.IsbnAndCustomerFields)
                    {
                        if (jsonIsbnAndCustomerField.IsbnOrSku != file.Provider.MarcRecord.Isbn13 &&
                            jsonIsbnAndCustomerField.IsbnOrSku != file.Provider.MarcRecord.Isbn10 &&
                            jsonIsbnAndCustomerField.IsbnOrSku != file.Provider.MarcRecord.Sku)
                        {
                            continue;
                        }
                        
                        marcRecordPaths.Add(WriteIndividualMarcFile2(file, jsonIsbnAndCustomerField,
                                                                     marcRecordRequest.AccountNumber, out lastIsbn,
                                                                     new JsonIsbnAndCustomerField
                                                                         {
                                                                             CustomMarcFields = marcRecordRequest.CustomMarcFields,
                                                                             IsbnOrSku = jsonIsbnAndCustomerField.IsbnOrSku
                                                                         }
                                                ));
                        break;
                    }

                }

                if (marcRecordPaths.Count == 0)
                {
                    Log.Debug("Zero Files found");
                    return View("Error");
                }

                Log.Debug("Begin MArC Record Files Merge");
                string filePath = GetFilePath(marcRecordRequest.AccountNumber, "MarcRecords");

                FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(aFile);
                foreach (string marcRecordPath in marcRecordPaths.Where(System.IO.File.Exists))
                {
                    sw.WriteLine(System.IO.File.ReadAllText(marcRecordPath));
                }
                sw.Close();
                aFile.Close();

                Log.Debug("End MArC Record Files Merge");

                if (marcRecordRequest.Format != "mrk")
                {
                    string newFilePath = string.Format(@"{0}.mrc", filePath.Replace(".mrk", ""));
                    Log.Debug("Changing MRK file to MRC");
                    MARC21 marc21 = new MARC21();
                    marc21.MMaker(filePath, newFilePath);

                    filePath = newFilePath;

                    if (marcRecordRequest.Format == "xml")
                    {
                        Log.Debug("Changing MRC file to XML");
                        newFilePath = string.Format(@"{0}.xml", filePath.Replace(".mrk", ""));
                        marc21.MARC2MARC21XML(filePath, newFilePath, false);
                        filePath = newFilePath;
                    }
                }

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


        //public ActionResult Download()
        //{
        //    return View();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcRecordFile"></param>
        /// <param name="jsonIsbnAndCustomerField"></param>
        /// <param name="accountNumber"></param>
        /// <param name="lastIsbn"></param>
        /// <param name="commonJsonFields"> </param>
        /// <returns></returns>
        public string WriteIndividualMarcFile2(MarcRecordFile marcRecordFile, JsonIsbnAndCustomerField jsonIsbnAndCustomerField, string accountNumber, out string lastIsbn, JsonIsbnAndCustomerField commonJsonFields = null)
        {
            string filePath = GetFilePath(accountNumber, marcRecordFile.Provider.MarcRecord.Isbn13);

            FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(aFile);

            lastIsbn = marcRecordFile.Provider.MarcRecord.Isbn13;
            Log.DebugFormat("Id: {0} | FileData: {1}", marcRecordFile.Id, marcRecordFile.FileData);
            sw.Write(marcRecordFile.FileData);

            sw.Close();
            aFile.Close();

            MARC21 marc21 = new MARC21();
            marc21.Delete_Field(filePath, FieldsToRemove);

            bool isR2LibraryRequest = false;
            if (HttpContext.Request.UrlReferrer != null)
            {
                isR2LibraryRequest = HttpContext.Request.UrlReferrer.ToString().Contains("r2");
            }

            PopulateUrlInMarcRecord(filePath, isR2LibraryRequest, marcRecordFile);

            if (commonJsonFields != null && commonJsonFields.CustomMarcFields != null && commonJsonFields.CustomMarcFields.Count > 0)
            {
                foreach (JsonCustomMarcField jsonCustomMarcField in commonJsonFields.CustomMarcFields)
                {
                    var marcFieldString = BuildCustomMarcField(jsonCustomMarcField);

                    if (!string.IsNullOrWhiteSpace(marcFieldString))
                    {
                        marc21.Add_Field(filePath, marcFieldString);
                    }
                }
            }
            else
            {
                Log.DebugFormat("No MarcFields to add");
            }


            if (jsonIsbnAndCustomerField.CustomMarcFields != null && jsonIsbnAndCustomerField.CustomMarcFields.Count > 0)
            {
                foreach (JsonCustomMarcField jsonCustomMarcField in jsonIsbnAndCustomerField.CustomMarcFields)
                {
                    var marcFieldString = BuildCustomMarcField(jsonCustomMarcField);

                    if (!string.IsNullOrWhiteSpace(marcFieldString))
                    {
                        marc21.Add_Field(filePath, marcFieldString);
                    }
                }
            }
            else
            {
                Log.DebugFormat("No customer specific MarcFields to add");
            }
            return filePath;
        }

        private string BuildCustomMarcField(JsonCustomMarcField jsonCustomMarcField)
        {
            StringBuilder sb = new StringBuilder();
            if (jsonCustomMarcField.FieldNumber != 0 && jsonCustomMarcField.FieldNumber != 856)
            {
                sb.AppendFormat("={0:000} {1}{2}", jsonCustomMarcField.FieldNumber, jsonCustomMarcField.FieldIndicator1,
                                jsonCustomMarcField.FieldIndicator2);
                if (jsonCustomMarcField.MarcSubfields.Count > 0)
                {
                    foreach (JsonCustomMarcSubfield jsonCustomMarcSubfield in jsonCustomMarcField.MarcSubfields)
                    {
                        sb.AppendFormat("${0}{1}", jsonCustomMarcSubfield.Subfield, jsonCustomMarcSubfield.SubfieldValue);
                    }
                }
                else
                {
                    sb.Append(jsonCustomMarcField.FieldValue);
                }
                return sb.ToString();
            }
            return null;
        }

        public void PopulateUrlInMarcRecord(string filePath, bool isR2Library, MarcRecordFile marcRecordFile)
        {
            string r2Url = System.Configuration.ConfigurationManager.AppSettings["R2LibraryUrl"];
            string rittenhouseUrl = System.Configuration.ConfigurationManager.AppSettings["RittenhouseUrl"];

            string bookUrl = isR2Library ? r2Url : rittenhouseUrl;

            MARC21 marc21 = new MARC21();
            marc21.Delete_Field(filePath, "=856");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("=856  4\\$zConnect to this resource online$u");

            sb.AppendFormat("{0}{1}", bookUrl,
                            isR2Library
                                ? marcRecordFile.Provider.MarcRecord.Isbn13 ?? marcRecordFile.Provider.MarcRecord.Isbn10
                                : marcRecordFile.Provider.MarcRecord.Sku ?? marcRecordFile.Provider.MarcRecord.Isbn10);

            marc21.Add_Field(filePath, sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="fileNameStart"></param>
        /// <returns></returns>
        private string GetFilePath(string accountNumber, string fileNameStart)
        {
            string workingDirectory = System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];

            string testPath = Path.Combine(workingDirectory, accountNumber);

            if (!Directory.Exists(testPath))
            {
                Directory.CreateDirectory(testPath);
            }

            string fileName = string.Format("{0}_{1}_{2}_{3}_{4}.mrk", fileNameStart, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);

            return Path.Combine(testPath, fileName);
        }

	}

}

