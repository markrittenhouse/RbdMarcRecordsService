using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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

			    var file = MarcRecordQueries.GetMnemonicMarcFileForEditing(item.Isbn13);
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
#region "Test Area"
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="itemList"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Download5(string itemList)
        //{

        //    if (itemList != null)
        //    {
        //        string workingDirectory =
        //            System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];
        //        Log.DebugFormat("workingDirectory: {0}", workingDirectory);

        //        MARC21 marc21 = new MARC21();

        //        var files = MarcRecordQueries.GetMnemonicMarcFilesForEditing(new List<string>());
        //        //var files = MarcRecordQueries.GetMnemonicMarcFilesForEditing(itemList);
        //        Random random = new Random();
        //        string filePath = string.Format("{0}\\MarcRecords-{1}_{2}_{3}_{4}.mrk", workingDirectory,
        //                                        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //                                        random.Next(9999).ToString().Substring(1, 2));

        //        FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(aFile);

        //        string lastIsbn = "";
        //        foreach (MarcRecordFile file in files)
        //        {
        //            if (lastIsbn == "" || lastIsbn != file.Provider.MarcRecord.Isbn13)
        //            {
        //                lastIsbn = file.Provider.MarcRecord.Isbn13;
        //                Log.DebugFormat("      Id: {0}, FileData: {1}", file.Id, file.FileData);
        //                sw.Write(file.FileData);
        //            }

        //        }

        //        sw.Close();
        //        aFile.Close();

        //        marc21.Delete_Field(filePath, FieldsToRemove);

        //        var fileStream = filePath;
        //        var mimeType = "text/plain";
        //        var fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.mrk", DateTime.Now.Year,
        //                                             DateTime.Now.Month, DateTime.Now.Day);


        //        return File(fileStream, mimeType, fileDownloadName);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// This method Gets the Items to find from the TempData. 
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Download()
        //{
        //    MarcRecordRequestItems itemList = (MarcRecordRequestItems)TempData["MarcRecordRequestItems"];

        //    if (itemList != null)
        //    {
        //        string workingDirectory =
        //            System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];
        //        Log.DebugFormat("workingDirectory: {0}", workingDirectory);

        //        MARC21 marc21 = new MARC21();

        //        var files = MarcRecordQueries.GetMnemonicMarcFilesForEditing(itemList);
        //        Random random = new Random();
        //        string filePath = string.Format("{0}\\MarcRecords-{1}_{2}_{3}_{4}.mrk", workingDirectory,
        //                                        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //                                        random.Next(9999).ToString().Substring(1, 2));

        //        FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(aFile);

        //        string lastIsbn = "";
        //        foreach (MarcRecordFile file in files)
        //        {
        //            if (lastIsbn == "" || lastIsbn != file.Provider.MarcRecord.Isbn13)
        //            {
        //                lastIsbn = file.Provider.MarcRecord.Isbn13;
        //                Log.DebugFormat("      Id: {0}, FileData: {1}", file.Id, file.FileData);
        //                sw.Write(file.FileData);
        //            }

        //        }

        //        sw.Close();
        //        aFile.Close();

        //        marc21.Delete_Field(filePath, FieldsToRemove);

        //        //TODO this is how you add fields
        //        marc21.Add_Field(filePath, "=999 //$atest");
                

        //        var fileStream = filePath;
        //        var mimeType = "text/plain";
        //        var fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.mrk", DateTime.Now.Year,
        //                                             DateTime.Now.Month, DateTime.Now.Day);
        //        return File(fileStream, mimeType, fileDownloadName);
        //    }
        //    return null;
        //}
        //public ActionResult GetMarcRecords()
        //{
        //    return View();
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetMarcRecords(MarcRecordsRequest item)
        //{
        //    if (item != null)
        //    {
        //        if (item.ItemIdentifier != null)
        //        {
        //            string[] newItems = item.ItemIdentifier.Replace("\r\n", "").Split(',');
        //            MarcRecordRequestItems requestItems = new MarcRecordRequestItems {Items = new List<string>()};
        //            requestItems.Items.AddRange(newItems);

        //            TempData["MarcRecordRequestItems"] = requestItems;
        //            return RedirectToAction("Download");
        //        }
        //    }
        //    return View();
        //}
        //public ActionResult GetKenMarcRecords()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult GetKenMarcRecords(MarcRecordRequestItems2 item)
        //{
        //    if (item != null)
        //    {
        //        TempData["MarcRecordRequestItems2"] = item;
        //        return RedirectToAction("Download2");
        //    }
        //    return View();
        //}
        ///// <summary>
        ///// This method Gets the Items to find from the TempData. 
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Download2()
        //{
        //    MarcRecordRequestItems2 marcItemList = (MarcRecordRequestItems2)TempData["MarcRecordRequestItems2"];

        //    if (marcItemList != null)
        //    {
        //        List<string> isbnsToFind =
        //            marcItemList.MarcRecordItems.Select(
        //                marcRecordItem => marcRecordItem.Isbn10 ?? (marcRecordItem.Isbn13 ?? (marcRecordItem.Sku))).
        //                ToList();

        //        string workingDirectory =
        //            System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];

        //        IEnumerable<MarcRecordFile> files = MarcRecordQueries.GetMnemonicMarcFilesForEditing(isbnsToFind);

        //        string filePath = string.Format("{0}\\MarcRecords-{1}_{2}_{3}_{4}.mrk", workingDirectory,
        //                                        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //                                        new Random().Next(9999).ToString().Substring(1, 2));

        //        string lastIsbn = "";
        //        List<string> marcRecordPaths = new List<string>();


        //        foreach (MarcRecordFile file in files)
        //        {
        //            if (lastIsbn == file.Provider.MarcRecord.Isbn13 || lastIsbn == file.Provider.MarcRecord.Isbn10 ||
        //                lastIsbn == file.Provider.MarcRecord.Sku) continue;

        //            foreach (MarcRecordItem marcRecordItem in marcItemList.MarcRecordItems)
        //            {
        //                if (file.Provider.MarcRecord.Isbn13 == marcRecordItem.Isbn13)
        //                {
        //                    marcRecordPaths.Add(WriteIndividualMarcFile(file, marcRecordItem, out lastIsbn));
        //                }
        //                else if (file.Provider.MarcRecord.Isbn10 == marcRecordItem.Isbn10)
        //                {
        //                    marcRecordPaths.Add(WriteIndividualMarcFile(file, marcRecordItem, out lastIsbn));
        //                }
        //                else if (file.Provider.MarcRecord.Isbn10 == marcRecordItem.Isbn10)
        //                {
        //                    marcRecordPaths.Add(WriteIndividualMarcFile(file, marcRecordItem, out lastIsbn));
        //                }
        //            }
        //        }

        //        FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(aFile);
        //        foreach (string marcRecordPath in marcRecordPaths)
        //        {
        //            sw.WriteLine(string.Format(System.IO.File.ReadAllText(marcRecordPath)));
        //        }
        //        sw.Close();
        //        aFile.Close();

        //        string fileStream = filePath;
        //        string mimeType = "text/plain";
        //        string fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.mrk", DateTime.Now.Year,
        //                                             DateTime.Now.Month, DateTime.Now.Day);
        //        return File(fileStream, mimeType, fileDownloadName);
        //    }
        //    return null;
        //}        
        //[HttpPost]
        //public ActionResult Download3(MarcRecordRequestItems2 marcItemList)
        //{            
        //    if (marcItemList != null)
        //    {
        //        List<string> isbnsToFind =
        //            marcItemList.MarcRecordItems.Select(
        //                marcRecordItem => marcRecordItem.Isbn10 ?? (marcRecordItem.Isbn13 ?? (marcRecordItem.Sku))).
        //                ToList();

        //        string workingDirectory =
        //            System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];

        //        IEnumerable<MarcRecordFile> files = MarcRecordQueries.GetMnemonicMarcFilesForEditing(isbnsToFind);

        //        string lastIsbn = "";
        //        List<string> marcRecordPaths = new List<string>();


        //        foreach (MarcRecordFile file in files)
        //        {
        //            if (lastIsbn == file.Provider.MarcRecord.Isbn13 || lastIsbn == file.Provider.MarcRecord.Isbn10 ||
        //                lastIsbn == file.Provider.MarcRecord.Sku) continue;

        //            foreach (MarcRecordItem marcRecordItem in marcItemList.MarcRecordItems)
        //            {
        //                if (file.Provider.MarcRecord.Isbn13 == marcRecordItem.Isbn13)
        //                {
        //                    marcRecordPaths.Add(WriteIndividualMarcFile(file, marcRecordItem, out lastIsbn));
        //                }
        //                else if (file.Provider.MarcRecord.Isbn10 == marcRecordItem.Isbn10)
        //                {
        //                    marcRecordPaths.Add(WriteIndividualMarcFile(file, marcRecordItem, out lastIsbn));
        //                }
        //                else if (file.Provider.MarcRecord.Isbn10 == marcRecordItem.Isbn10)
        //                {
        //                    marcRecordPaths.Add(WriteIndividualMarcFile(file, marcRecordItem, out lastIsbn));
        //                }
        //            }
        //        }

        //        string filePath = string.Format("{0}\\MarcRecords-{1}_{2}_{3}_{4}.mrk", workingDirectory,
        //                        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //                        new Random().Next(9999).ToString().Substring(1, 2));

        //        FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(aFile);
        //        foreach (string marcRecordPath in marcRecordPaths)
        //        {
        //            sw.WriteLine(string.Format(System.IO.File.ReadAllText(marcRecordPath)));
        //        }
        //        sw.Close();
        //        aFile.Close();

        //        string fileStream = filePath;
        //        string mimeType = "text/plain";
        //        string fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.mrk", DateTime.Now.Year,
        //                                             DateTime.Now.Month, DateTime.Now.Day);
        //        return File(fileStream, mimeType, fileDownloadName);
        //    }
        //    return null;
        //}
        //public string WriteIndividualMarcFile(MarcRecordFile marcRecordFile, MarcRecordItem marcRecordItem, out string lastIsbn)
        //{
        //    string workingDirectory =
        //            System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];

        //    string filePath = string.Format("{0}\\MarcRecords-{1}_{2}_{3}_{4}_{5}.mrk", workingDirectory,
        //                                        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //                                        marcRecordFile.Provider.MarcRecord.Isbn13, new Random().Next(9999).ToString().Substring(1, 2));

        //    FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //    StreamWriter sw = new StreamWriter(aFile);

        //    lastIsbn = marcRecordFile.Provider.MarcRecord.Isbn13;
        //    Log.DebugFormat("      Id: {0}, FileData: {1}", marcRecordFile.Id, marcRecordFile.FileData);
        //    sw.Write(marcRecordFile.FileData);

        //    sw.Close();
        //    aFile.Close();

        //    MARC21 marc21 = new MARC21();
        //    marc21.Delete_Field(filePath, FieldsToRemove);

        //    foreach (MarcField marcField in marcRecordItem.MarcFields)
        //    {
        //        if (marcField.Field != "" && marcField.FieldValue != "")
        //        {
        //            marc21.Add_Field(filePath, string.Format("={0} //{1}", marcField.Field, marcField.FieldValue));
        //        }
        //    }

        //    return filePath;
        //}
#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcRecordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download(JsonMarcRecordRequest marcRecordRequest)
        {
            if (marcRecordRequest != null)
            {
                List<string> isbnsToFind = marcRecordRequest.IsbnAndCustomerFields.Select(jsonIsbnAndCustomerFields => jsonIsbnAndCustomerFields.IsbnOrSku).ToList();

                IEnumerable<MarcRecordFile> files = MarcRecordQueries.GetMnemonicMarcFilesForEditing(isbnsToFind);

                string lastIsbn = "";
                List<string> marcRecordPaths = new List<string>();

                foreach (MarcRecordFile file in files)
                {
                    if (lastIsbn == file.Provider.MarcRecord.Isbn13 || lastIsbn == file.Provider.MarcRecord.Isbn10 || lastIsbn == file.Provider.MarcRecord.Sku) continue;

                    marcRecordPaths.AddRange(from jsonIsbnAndCustomerFields in marcRecordRequest.IsbnAndCustomerFields
                                             where
                                                 jsonIsbnAndCustomerFields.IsbnOrSku == file.Provider.MarcRecord.Isbn13 ||
                                                 jsonIsbnAndCustomerFields.IsbnOrSku == file.Provider.MarcRecord.Isbn10 ||
                                                 jsonIsbnAndCustomerFields.IsbnOrSku == file.Provider.MarcRecord.Sku
                                             select WriteIndividualMarcFile2(file, jsonIsbnAndCustomerFields, marcRecordRequest.AccountNumber, out lastIsbn));
                }

                string filePath = GetFilePath(marcRecordRequest.AccountNumber, "MarcRecords");

                FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(aFile);
                foreach (string marcRecordPath in marcRecordPaths)
                {
                    sw.WriteLine(string.Format(System.IO.File.ReadAllText(marcRecordPath)));
                }
                sw.Close();
                aFile.Close();

                
                
                if (marcRecordRequest.Format != "mrk")
                {
                    string newFilePath = string.Format(@"{0}.mrc", filePath.Replace(".mrk", ""));

                    MARC21 marc21 = new MARC21();
                    marc21.MMaker(filePath, newFilePath);

                    filePath = newFilePath;

                    if (marcRecordRequest.Format == "xml")
                    {
                        newFilePath = string.Format(@"{0}.xml", filePath.Replace(".mrk", ""));
                        marc21.MARC2MARC21XML(filePath, newFilePath, false);
                        filePath = newFilePath;
                    }
                }

                string fileStream = filePath;
                const string mimeType = "text/plain";
                string fileDownloadName = string.Format("MarcRecords-{0}_{1}_{2}.{3}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, marcRecordRequest.Format);
                return File(fileStream, mimeType, fileDownloadName);
            }
            return null;
        }


        public ActionResult Download()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcRecordFile"></param>
        /// <param name="jsonIsbnAndCustomerField"></param>
        /// <param name="accountNumber"></param>
        /// <param name="lastIsbn"></param>
        /// <returns></returns>
        public string WriteIndividualMarcFile2(MarcRecordFile marcRecordFile, JsonIsbnAndCustomerField jsonIsbnAndCustomerField, string accountNumber, out string lastIsbn)
        {
            string filePath = GetFilePath(accountNumber, marcRecordFile.Provider.MarcRecord.Isbn13);

            FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(aFile);

            lastIsbn = marcRecordFile.Provider.MarcRecord.Isbn13;
            Log.DebugFormat("      Id: {0}, FileData: {1}", marcRecordFile.Id, marcRecordFile.FileData);
            sw.Write(marcRecordFile.FileData);

            sw.Close();
            aFile.Close();

            MARC21 marc21 = new MARC21();
            marc21.Delete_Field(filePath, FieldsToRemove);

            if (jsonIsbnAndCustomerField.CustomMarcFields != null && jsonIsbnAndCustomerField.CustomMarcFields.Count > 0)
            {
                foreach (JsonCustomMarcField jsonCustomMarcField in jsonIsbnAndCustomerField.CustomMarcFields)
                {
                    StringBuilder sb = new StringBuilder();
                    if (jsonCustomMarcField.FieldNumber != 0)
                    {
                        sb.AppendFormat("={0} {1}{2}", jsonCustomMarcField.FieldNumber, jsonCustomMarcField.FieldIndicator1,
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
                        marc21.Add_Field(filePath, sb.ToString());
                    }
                }
            }
            return filePath;
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

