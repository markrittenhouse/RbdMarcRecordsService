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
            //_request = request;
            _log = log;
        }

        public string GetMergedMarcRecordsFilePath(JsonMarcRecordRequest marcRecordRequest, List<string> marcRecordPaths)
        {
            _log.Debug("Begin MArC Record Files Merge");
            string filePath = GetFilePath(marcRecordRequest.AccountNumber, "MarcRecords");

            FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(aFile);
            foreach (string marcRecordPath in marcRecordPaths.Where(File.Exists))
            {
                sw.WriteLine(File.ReadAllText(marcRecordPath));
            }
            sw.Close();
            aFile.Close();

            _log.Debug("End MArC Record Files Merge");

            if (marcRecordRequest.Format != "mrk")
            {
                string newFilePath = string.Format(@"{0}.mrc", filePath.Replace(".mrk", ""));
                _log.Debug("Changing MRK file to MRC");
                MARC21 marc21 = new MARC21();
                marc21.MMaker(filePath, newFilePath);

                filePath = newFilePath;

                if (marcRecordRequest.Format == "xml")
                {
                    _log.Debug("Changing MRC file to XML");
                    newFilePath = string.Format(@"{0}.xml", filePath.Replace(".mrk", ""));
                    marc21.MARC2MARC21XML(filePath, newFilePath, false);
                    filePath = newFilePath;
                }
            }
            return filePath;
        }

        public List<string> WriteMarcRecordFiles(JsonMarcRecordRequest marcRecordRequest, List<DailyMarcRecordFile> files)
        {
            string lastIsbn = "";
            List<string> marcRecordPaths = new List<string>();

            foreach (DailyMarcRecordFile file in files)
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

                    marcRecordPaths.Add(WriteIndividualMarcFile(file, jsonIsbnAndCustomerField,
                                                                marcRecordRequest.AccountNumber, out lastIsbn,
                                                                marcRecordRequest,
                                                                new JsonIsbnAndCustomerField
                                                                    {
                                                                        CustomMarcFields =
                                                                            marcRecordRequest.CustomMarcFields,
                                                                        IsbnOrSku = jsonIsbnAndCustomerField.IsbnOrSku
                                                                    }
                                            ));
                    break;
                }

            }
            return marcRecordPaths;
        }

        public string WriteIndividualMarcFile(DailyMarcRecordFile marcRecordFile, JsonIsbnAndCustomerField jsonIsbnAndCustomerField, string accountNumber, out string lastIsbn, JsonMarcRecordRequest marcRecordRequest, JsonIsbnAndCustomerField commonJsonFields = null)
        {
            string filePath = GetFilePath(accountNumber, marcRecordFile.Isbn13);

            FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(aFile);

            lastIsbn = marcRecordFile.Isbn13;
            _log.DebugFormat("Id: {0} | FileData: {1}", marcRecordFile.Id, marcRecordFile.MarcFile);
            sw.Write(marcRecordFile.MarcFile);

            sw.Close();
            aFile.Close();

            MARC21 marc21 = new MARC21();
            marc21.Delete_Field(filePath, FieldsToRemove);
            
            if (marcRecordRequest.IsR2Request || marcRecordRequest.IsRittenhouseRequest)
            {
                PopulateUrlInMarcRecord(filePath, marcRecordRequest.IsR2Request, marcRecordFile);
            }            

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
                _log.DebugFormat("No MarcFields to add");
            }


            if (jsonIsbnAndCustomerField.CustomMarcFields != null && jsonIsbnAndCustomerField.CustomMarcFields.Count > 0)
            {
                foreach (JsonCustomMarcField jsonCustomMarcField in jsonIsbnAndCustomerField.CustomMarcFields)
                {
                    //TODO: Can remove once Marc Records has been updated on Rittenhouse and R2library
                    if (jsonCustomMarcField.FieldNumber == 856)
                    {
                        _log.DebugFormat("Deleting Field 856 because a new one was passed in.");
                        marc21.Delete_Field(filePath, "=856");
                    }
                    var marcFieldString = BuildCustomMarcField(jsonCustomMarcField);

                    if (!string.IsNullOrWhiteSpace(marcFieldString))
                    {
                        _log.DebugFormat("-------Custom Marc Field: {0}", marcFieldString);
                        marc21.Add_Field(filePath, marcFieldString);
                    }
                    else
                    {
                        _log.Debug("-------Custom Marc Field: WAS NULL!!!");
                    }
                }
            }
            else
            {
                _log.DebugFormat("No customer specific MarcFields to add");
            }
            return filePath;
        }

        public List<DailyMarcRecordFile> CreateDeleteMarcRecordFiles(List<DailyMarcRecordFile> files)
        {
            foreach (DailyMarcRecordFile marcRecordFile in files)
            {
                var sb = new StringBuilder(marcRecordFile.MarcFile);
                sb.Remove(11, 12);
                sb.Insert(11, 'd');
                marcRecordFile.MarcFile = sb.ToString();
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

            string fileName = string.Format("{0}_{1}_{2}_{3}_{4}.mrk", fileNameStart, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);

            return Path.Combine(testPath, fileName);
        }

        private void PopulateUrlInMarcRecord(string filePath, bool isR2Library, DailyMarcRecordFile marcRecordFile)
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
                                ? marcRecordFile.Isbn13 ?? marcRecordFile.Isbn10
                                : marcRecordFile.Sku ?? marcRecordFile.Isbn10);

            marc21.Add_Field(filePath, sb.ToString());
        }

        private string BuildCustomMarcField(JsonCustomMarcField jsonCustomMarcField)
        {
            StringBuilder sb = new StringBuilder();
            if (jsonCustomMarcField.FieldNumber != 0)
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


    }
}