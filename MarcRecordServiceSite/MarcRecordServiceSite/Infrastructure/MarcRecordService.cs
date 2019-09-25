using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MARCEngine5;
using MarcRecordServiceSite.Infrastructure.NHibernate.Entities;
using MarcRecordServiceSite.Models;
using log4net;

namespace MarcRecordServiceSite.Infrastructure
{
    public class MarcRecordService
    {
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
        private readonly ILog _log;

        public MarcRecordService(ILog log)
        {
            _log = log;
        }

        public string GetMergedMarcRecordsFilePath(JsonMarcRecordRequest marcRecordRequest, List<string> marcRecordPaths)
        {
            _log.Debug("Begin MArC Record Files Merge");
            string filePath = GetFilePath(marcRecordRequest.AccountNumber, "MarcRecords");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string marcRecordPath in marcRecordPaths.Where(File.Exists))
                    {
                        streamWriter.WriteLine(File.ReadAllText(marcRecordPath));
                    }
                }
                fileStream.Close();
                fileStream.Dispose();
            }

            foreach (var marcRecordPath in marcRecordPaths)
            {
                File.Delete(marcRecordPath);
            }

            _log.Debug("End MArC Record Files Merge");

            if (marcRecordRequest.Format != "mrk")
            {
                string newFilePath = string.Format(@"{0}.mrc", filePath.Replace(".mrk", ""));
                _log.Debug("Changing MRK file to MRC");
                MARC21 marc21 = new MARC21();
                marc21.MMaker(filePath, newFilePath);
                var fileToDelete = filePath;
                filePath = newFilePath;

                if (marcRecordRequest.Format == "xml")
                {
                    _log.Debug("Changing MRC file to XML");
                    newFilePath = string.Format(@"{0}.xml", filePath.Replace(".mrk", ""));
                    marc21.MARC2MARC21XML(filePath, newFilePath, false);
                    filePath = newFilePath;
                }

                File.Delete(fileToDelete);
            }
            return filePath;
        }

        public string GetMergedMarcRecordsFilePath(JsonMarcRecordRequest marcRecordRequest, List<string> marcRecordPaths, bool isMarc8)
        {
            _log.Debug("Begin MArC Record Files Merge");
            string filePath = GetFilePath(marcRecordRequest.AccountNumber, "MarcRecords");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string marcRecordPath in marcRecordPaths.Where(File.Exists))
                    {
                        streamWriter.WriteLine(File.ReadAllText(marcRecordPath));
                    }
                }
                fileStream.Close();
                fileStream.Dispose();
            }

            foreach (var marcRecordPath in marcRecordPaths)
            {
                File.Delete(marcRecordPath);
            }

            _log.Debug("End MArC Record Files Merge");

            if (marcRecordRequest.Format != "mrk")
            {
                string newFilePath = string.Format(@"{0}.mrc", filePath.Replace(".mrk", ""));
                _log.Debug("Changing MRK file to MRC");
                MARC21 marc21 = new MARC21();
                if (isMarc8)
                {
                    marc21.MMakerEx(filePath, newFilePath, MARC21.DEFAULT_SET);
                }
                else
                {
                    marc21.MMaker(filePath, newFilePath);
                }

                var fileToDelete = filePath;
                filePath = newFilePath;

                if (marcRecordRequest.Format == "xml")
                {
                    _log.Debug("Changing MRC file to XML");
                    newFilePath = string.Format(@"{0}.xml", filePath.Replace(".mrk", ""));
                    marc21.MARC2MARC21XML(filePath, newFilePath, false);
                    filePath = newFilePath;
                }

                File.Delete(fileToDelete);
            }
            return filePath;
        }

        public List<string> WriteMarcRecordFiles(JsonMarcRecordRequest marcRecordRequest, List<PrintMarcRecordFile> files)
        {
            string lastIsbn = "";
            List<string> marcRecordPaths = new List<string>();

            foreach (PrintMarcRecordFile file in files)
            {
                if (lastIsbn == file.Isbn13 || lastIsbn == file.Isbn10 || lastIsbn == file.Sku) continue;

                foreach (var jsonIsbnAndCustomerField in marcRecordRequest.IsbnAndCustomerFields)
                {
                    if (jsonIsbnAndCustomerField.IsbnOrSku != file.Isbn13 &&
                        jsonIsbnAndCustomerField.IsbnOrSku != file.Isbn10 &&
                        jsonIsbnAndCustomerField.IsbnOrSku != file.Sku)
                    {
                        continue;
                    }
                    var marcRecord = WriteIndividualMarcFile(file, jsonIsbnAndCustomerField,
                                                             marcRecordRequest.AccountNumber, out lastIsbn,
                                                             marcRecordRequest,
                                                             new JsonIsbnAndCustomerField
                                                             {
                                                                 CustomMarcFields =
                                                                     marcRecordRequest.CustomMarcFields,
                                                                 IsbnOrSku = jsonIsbnAndCustomerField.IsbnOrSku
                                                             }
                        );
                    if (!string.IsNullOrWhiteSpace(marcRecord))
                    {
                        marcRecordPaths.Add(marcRecord);
                    }

                    break;
                }

            }
            return marcRecordPaths;
        }

        public List<string> WriteDigitalMarcRecordFiles(List<DigitalMarcRecordFile> files, string accountNumber)
        {
            return WriteDigitalMarcRecordFiles(files, accountNumber, null, null);
        }
        public List<string> WriteDigitalMarcRecordFiles(List<DigitalMarcRecordFile> files, string accountNumber, string urlPrefix, string urlSuffix)
        {
            List<string> marcRecordPaths = new List<string>();

            foreach (DigitalMarcRecordFile file in files)
            {
                var marcRecord = WriteDigitalMarcRecordFile(file, accountNumber, urlPrefix, urlSuffix);
                if (!string.IsNullOrWhiteSpace(marcRecord))
                {
                    marcRecordPaths.Add(marcRecord);
                }

            }
            return marcRecordPaths;
        }

        public string WriteDigitalMarcRecordFile(DigitalMarcRecordFile marcRecordFile, string accountNumber, string urlPrefix, string urlSuffix)
        {
            string filePath = GetFilePath(accountNumber, marcRecordFile.Isbn13);
            try
            {
                _log.Info("Writing File");
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        var marcFile = marcRecordFile.MarcFile;
                        if (!string.IsNullOrWhiteSpace(urlPrefix))
                        {
                            marcFile = marcFile.Insert((marcFile.IndexOf("online$u", StringComparison.Ordinal) + 8), urlPrefix);
                        }

                        marcFile = $"{marcFile}{urlSuffix}";
                        streamWriter.Write(marcFile);
                    }
                    fileStream.Close();
                    fileStream.Dispose();
                }

                _log.Info("File is Complete");
            }

            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
            }
            return filePath;
        }

        public string WriteIndividualMarcFile(PrintMarcRecordFile marcRecordFile, JsonIsbnAndCustomerField jsonIsbnAndCustomerField, string accountNumber, out string lastIsbn, JsonMarcRecordRequest marcRecordRequest, JsonIsbnAndCustomerField commonJsonFields = null)
        {

            string filePath = GetFilePath(accountNumber, marcRecordFile.Isbn13);
            lastIsbn = marcRecordFile.Isbn13;
            try
            {


                _log.Info("Writing File");
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Position = 0;
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(marcRecordFile.MarcFile);
                    }
                    fileStream.Close();
                    fileStream.Dispose();
                }

                _log.Info("Deleting Extra Fields from file");
                MARC21 marc21 = new MARC21();
                marc21.Delete_Field(filePath, FieldsToRemove);

                if (marcRecordRequest.IsR2Request || marcRecordRequest.IsRittenhouseRequest)
                {
                    PopulateUrlInMarcRecord(filePath, marcRecordRequest.IsR2Request, marcRecordFile);
                    if (marcRecordRequest.IsR2Request)
                    {
                        PopulateStaticR2OnlyFields(filePath);
                    }
                }
                _log.Info("Adding common fields to file");
                if (commonJsonFields != null && commonJsonFields.CustomMarcFields != null &&
                    commonJsonFields.CustomMarcFields.Count > 0)
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
                    _log.InfoFormat("No MarcFields to add");
                }

                _log.Info("Adding custom fields to file");
                if (jsonIsbnAndCustomerField.CustomMarcFields != null &&
                    jsonIsbnAndCustomerField.CustomMarcFields.Count > 0)
                {
                    foreach (JsonCustomMarcField jsonCustomMarcField in jsonIsbnAndCustomerField.CustomMarcFields)
                    {
                        //TODO: Can remove once Marc Records has been updated on Rittenhouse and R2library
                        if (jsonCustomMarcField.FieldNumber == 856)
                        {
                            _log.InfoFormat("Deleting Field 856 because a new one was passed in.");
                            marc21.Delete_Field(filePath, "=856");
                        }
                        var marcFieldString = BuildCustomMarcField(jsonCustomMarcField);

                        if (!string.IsNullOrWhiteSpace(marcFieldString))
                        {
                            _log.InfoFormat("-------Custom Marc Field: {0}", marcFieldString);
                            marc21.Add_Field(filePath, marcFieldString);
                        }
                        else
                        {
                            _log.Info("-------Custom Marc Field: WAS NULL!!!");
                        }
                    }
                }
                else
                {
                    _log.InfoFormat("No customer specific MarcFields to add");
                }
                _log.Info("File is Complete");
            }

            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
            }
            return filePath;
        }

        


        public List<PrintMarcRecordFile> CreateDeleteMarcRecordFiles(List<PrintMarcRecordFile> files)
        {
            foreach (PrintMarcRecordFile marcRecordFile in files)
            {
                marcRecordFile.MarcFile = marcRecordFile.MarcFile.Remove(11, 1).Insert(11, "d");
            }
            return files;
        }

        public List<DigitalMarcRecordFile> CreateDeleteMarcRecordFiles(List<DigitalMarcRecordFile> files)
        {
            foreach (DigitalMarcRecordFile marcRecordFile in files)
            {
                marcRecordFile.MarcFile = marcRecordFile.MarcFile.Remove(11, 1).Insert(11, "d");
            }
            return files;
        }

        public string GetFilePath(string accountNumber, string fileNameStart)
        {
            string workingDirectory = System.Configuration.ConfigurationManager.AppSettings["MarcRecordsWorkingDirectory"];

            string testPath = Path.Combine(workingDirectory, accountNumber);

            if (!Directory.Exists(testPath))
            {
                Directory.CreateDirectory(testPath);
            }

            string fileName = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}.mrk", fileNameStart, DateTime.Now.Year,
                                            DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute,
                                            DateTime.Now.Second, DateTime.Now.Millisecond);

            return Path.Combine(testPath, fileName);
        }

        private void PopulateUrlInMarcRecord(string filePath, bool isR2Library, PrintMarcRecordFile marcRecordFile)
        {
            string r2Url = System.Configuration.ConfigurationManager.AppSettings["R2LibraryUrl"];
            string rittenhouseUrl = System.Configuration.ConfigurationManager.AppSettings["RittenhouseUrl"];

            string bookUrl = isR2Library ? r2Url : rittenhouseUrl;

            MARC21 marc21 = new MARC21();
            marc21.Delete_Field(filePath, "=856");

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("=856  4\\$zConnect to this resource online$u");
            sb.AppendFormat("{0}{1}", bookUrl, marcRecordFile.Isbn13 ?? marcRecordFile.Isbn10);
            marc21.Add_Field(filePath, sb.ToString());
        }

        private void PopulateStaticR2OnlyFields(string filePath)
        {
            MARC21 marc21 = new MARC21();
            
            marc21.Delete_Field(filePath, "=300");
            marc21.Delete_Field(filePath, "=538");
            marc21.Delete_Field(filePath, "=655");

            marc21.Add_Field(filePath, "=300  \\\\$a online resource");
            marc21.Add_Field(filePath, "=538  \\\\$a Mode of Access: World Wide Web");
            marc21.Add_Field(filePath, "=655  \\4$a Electronic books".Replace(@"\\",@"\"));
        }

        private static string BuildCustomMarcField(JsonCustomMarcField jsonCustomMarcField)
        {
            StringBuilder sb = new StringBuilder();
            if (jsonCustomMarcField.FieldNumber != 0)
            {
                sb.AppendFormat("={0:000}  {1}{2}", jsonCustomMarcField.FieldNumber, jsonCustomMarcField.FieldIndicator1,
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


    }
}