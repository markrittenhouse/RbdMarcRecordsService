﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MARCEngine5;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class R2LibraryMarcRecordsTask : TaskBase2
    {
        private readonly R2ProductFactory _r2ProductFactory;
        private readonly DateTime _currentDateTime = DateTime.Now;

        public R2LibraryMarcRecordsTask(R2ProductFactory r2ProductFactory)
            : base("Generate R2library MArC Records", "CreateR2libraryMarcRecords")
        {
            _r2ProductFactory = r2ProductFactory;
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "R2LibraryMarcRecordsTask.CreateR2libraryMarcRecords",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {

                var resourcesThatDoNotExistInProviders = _r2ProductFactory.GetR2ResourcesThatDoNotExist();
                var r2LibraryMarcFiles = (from resource in resourcesThatDoNotExistInProviders
                    let mrkFileString = GetR2LibraryMrkFileText(resource)
                    select new R2LibraryMarcFile
                    {
                        MrkText = mrkFileString,
                        ProviderSourceId = 4,
                        Isbn = resource.Isbn,
                        Isbn10 = resource.Isbn10,
                        Isbn13 = resource.Isbn13,
                        EIsbn = resource.EIsbn
                    }).ToList();

                List<R2LibraryMarcFile> nlmandLcR2LibraryMarcFiles = _r2ProductFactory.GetNlmAndLcMarcFiles();

                RemoveFieldsFromExternalMarcFiles(nlmandLcR2LibraryMarcFiles);

                r2LibraryMarcFiles.AddRange(nlmandLcR2LibraryMarcFiles);

                int recordsUpdated = 0;
                int recordsInserted;
                if (_r2ProductFactory.CreateAllR2LibraryMarcRecords())
                {
                    recordsInserted = _r2ProductFactory.InsertWebR2LibraryMarcRecords(r2LibraryMarcFiles);
                }
                else
                {
                    _r2ProductFactory.UpdateWebR2LibraryMarcRecords(r2LibraryMarcFiles, out recordsUpdated, out recordsInserted);
                }
                
                step.CompletedSuccessfully = true;
                step.Results.AppendFormat("R2Library Records Updated: {0} | Records Inserted: {1}", recordsUpdated, recordsInserted);
            }
            catch (Exception ex)
            {
                step.Results.Insert(0, string.Format("EXCEPTION: {0}", ex.Message));
                step.CompletedSuccessfully = false;
                throw;
            }
            finally
            {
                step.EndTime = DateTime.Now;
                TaskResultFactory.InsertTaskResultStep(step);
            }
        }

        private string GetR2LibraryMrkFileText(R2Resource resource)
        {
            try
            {
                StringBuilder mrkFileText = new StringBuilder();

                mrkFileText.AppendFormat("=LDR  {0}nam  22{1}2a 4500", GetNext5DigitRandomNumber(), GetNext5DigitRandomNumber()).AppendLine();

                mrkFileText.AppendLine(string.Format("=001  {0}", resource.Isbn));
                mrkFileText.AppendLine(string.Format("=005  {0:yyyyMMddhhmmss}.0", DateTime.Now));
                mrkFileText.AppendLine(string.Format("=008  {0:yyMMdd}s{1:0000}{2}eng{3}o", _currentDateTime, resource.PublicationYear, GetSpace(23), GetSpace(1)));

                var firstAuthorString = resource.AuthorList.Any()
                    ? resource.AuthorList.First(x => x.Order == 1).ToDisplayName()
                    : resource.FirstAuthor;

                if (!string.IsNullOrWhiteSpace(resource.Isbn10))
                {
                    mrkFileText.AppendFormat("=020  {0}$a{1}", GetSpace(2), resource.Isbn10 ).AppendLine();
                    
                }
                if (!string.IsNullOrWhiteSpace(resource.Isbn13))
                {
                    mrkFileText.AppendFormat("=020  {0}$a{1}", GetSpace(2), resource.Isbn13).AppendLine();
                    
                }
                if (!string.IsNullOrWhiteSpace(resource.EIsbn))
                {
                    mrkFileText.AppendFormat("=020  {0}$a{1}", GetSpace(2), resource.EIsbn).AppendLine();
                }

                mrkFileText.AppendFormat("=037  {0}$bRittenhouse Book Distributors, Inc", GetSpace(2)).AppendLine();
                mrkFileText.AppendFormat("=100  1{0}$a{1}", GetSpace(1), firstAuthorString).AppendLine();
                mrkFileText.AppendFormat("=245  10$a{0}", StripOffCarriageReturnAndLineFeed(resource.Title)).AppendLine();
                mrkFileText.AppendFormat("=260  {0}$b{1},$c{2}", GetSpace(2), resource.PublisherName, resource.PublicationYear).AppendLine();
                mrkFileText.AppendFormat("=300  {0}$a online resource", GetSpace(2)).AppendLine();
                mrkFileText.AppendFormat("=533  {0}$aeBook.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{1}", GetSpace(2), resource.PublicationYear).AppendLine();
                mrkFileText.AppendFormat("=538  {0}$a Mode of Access: World Wide Web", GetSpace(2)).AppendLine();
                
                WriteCategories(mrkFileText, resource);//650
                
                mrkFileText.AppendFormat("=655  {0}4$a Electronic books", GetSpace(1)).AppendLine();
                
                WriteAuthors(mrkFileText, resource);//700

                mrkFileText.AppendFormat("=856  4{0}$zConnect to this resource online$u{1}{2}", GetSpace(1), Settings.Default.R2WebSite, resource.Isbn).AppendLine();

                return mrkFileText.ToString();
            }
            catch (Exception ex)
            {
                Log.Info(resource != null ? resource.ToDebugString() : "Product is null!");
                Log.Error(ex.Message, ex);
                throw;
            }

        }

        private static void WriteCategories(StringBuilder mrkFileText, R2Resource resource)
        {
            if (resource.Categories != null || resource.SubCategories != null)
            {
                var categories = resource.Categories != null ? resource.Categories.ToList() : null;
                var subCategories = resource.SubCategories != null ? resource.SubCategories.ToList() : null;
                //If categories = subCategories
                if (categories != null && subCategories != null)
                {
                    if (categories.Count == subCategories.Count)
                    {
                        for (int i = 0; i < categories.Count; i++)
                        {
                            mrkFileText.AppendFormat("=650  {0}4$a{1}$x{2}", GetSpace(1), categories[i].Category, subCategories[i].SubCategory).AppendLine();
                        }

                    }
                    else if (categories.Count > subCategories.Count && subCategories.Count == 1)
                    {
                        if (subCategories.Count == 1)
                        {
                            foreach (var r2Category in categories)
                            {
                                mrkFileText.AppendFormat("=650  {0}4$a{1}$x{2}", GetSpace(1), r2Category.Category, subCategories[0].SubCategory).AppendLine();
                            }
                        }
                    }
                    else
                    {
                        foreach (var r2Category in categories)
                        {
                            foreach (var r2SubCategory in subCategories)
                            {
                                mrkFileText.AppendFormat("=650  {0}4$a{1}$x{2}", GetSpace(1), r2Category.Category,
                                    r2SubCategory.SubCategory).AppendLine();
                            }
                        }
                    }

                }
                else if (categories != null) //subCategories == null
                {
                    foreach (var r2Category in categories)
                    {
                        mrkFileText.AppendFormat("=650  {0}4$a{1}", GetSpace(1), r2Category.Category).AppendLine();
                    }
                }
                else if (subCategories != null)
                {
                    foreach (var r2SubCategories in subCategories)
                    {
                        mrkFileText.AppendFormat("=650  {0}4$a{1}", GetSpace(1), r2SubCategories.SubCategory).AppendLine();
                    }
                }
            }
        }

        private static void WriteAuthors(StringBuilder mrkFileText, R2Resource resource)
        {
            if (resource.AuthorList != null)
            {
                foreach (var r2Author in resource.AuthorList)
                {
                    mrkFileText.AppendFormat("=700  1{0}$a{1}", GetSpace(1), r2Author.ToDisplayName()).AppendLine();
                }
            }
            else
            {
                mrkFileText.AppendFormat("=700  1{0}$a{1}", GetSpace(1), resource.FirstAuthor).AppendLine();
            }
        }

        private static string GetSpace(int count)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append("\\");
            }
            return sb.ToString();
        }


        private void RemoveFieldsFromExternalMarcFiles(List<R2LibraryMarcFile> nlmandLcR2LibraryMarcFiles)
        {
            string workingDirectory = (Settings.Default.MarcFilesWorkingDirectory.EndsWith(@"\"))
                              ? Settings.Default.MarcFilesWorkingDirectory
                              : string.Format(@"{0}\", Settings.Default.MarcFilesWorkingDirectory);
            Log.InfoFormat("workingDirectory:\n{0}", workingDirectory);

            ClearWorkingDirectory(workingDirectory);

            string batchFileNameBase = string.Format("batch_{0:yyyyMMdd_HHmmssfff}", DateTime.Now);
            MARC21 marc21 = new MARC21();
            foreach (var nlmandLcR2LibraryMarcFile in nlmandLcR2LibraryMarcFiles)
            {
                Log.InfoFormat("Editing MrkFile for ISBN: {0}", nlmandLcR2LibraryMarcFile.Isbn);
                string mrkFilePath = string.Format(@"{0}{1}_{2}.mrk", workingDirectory, batchFileNameBase, nlmandLcR2LibraryMarcFile.Isbn);
                Log.DebugFormat("mrkFilePath: {0}", mrkFilePath);

                File.WriteAllText(mrkFilePath, nlmandLcR2LibraryMarcFile.MrkText);
                
                marc21.Delete_Field(mrkFilePath, FieldsToRemove);
                marc21.Delete_Field(mrkFilePath, "=300");
                marc21.Delete_Field(mrkFilePath, "=538");
                marc21.Delete_Field(mrkFilePath, "=655");
                marc21.Delete_Field(mrkFilePath, "=856");

                marc21.Add_Field(mrkFilePath, string.Format("=300  {0}$a online resource", GetSpace(2)));
                marc21.Add_Field(mrkFilePath, string.Format("=538  {0}$a Mode of Access: World Wide Web", GetSpace(2)));
                marc21.Add_Field(mrkFilePath, string.Format("=655  {0}4$a Electronic books", GetSpace(1)));

                var field856 = string.Format("=856  4{0}$zConnect to this resource online$u{1}{2}", GetSpace(1), Settings.Default.R2WebSite, nlmandLcR2LibraryMarcFile.Isbn);

                marc21.Add_Field(mrkFilePath, field856);

                var newMrkFile = File.ReadAllText(mrkFilePath);
                nlmandLcR2LibraryMarcFile.MrkText = newMrkFile;
            }
            
        }

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
    }
}
