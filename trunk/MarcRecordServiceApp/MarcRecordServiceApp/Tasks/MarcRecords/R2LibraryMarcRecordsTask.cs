using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;
using MARCEngine5;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class R2LibraryMarcRecordsTask : TaskBase
    {
        private readonly R2ProductFactory _r2ProductFactory;
        private readonly MarcRecordService _marcRecordService;
        private readonly DateTime _currentDateTime = DateTime.Now;

        public R2LibraryMarcRecordsTask()
            : base("Generate R2library MArC Records", "CreateR2libraryMarcRecords")
        {
            _r2ProductFactory = new R2ProductFactory();
            _marcRecordService = new MarcRecordService(MarcRecordProviderType.R2Library);
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
                List<R2Resource> r2Resources2 = _r2ProductFactory.GetAllR2Resource();

                List<R2LibraryMarcFile> r2LibraryMarcFiles = r2Resources2.Select(x => _marcRecordService.GetR2LibraryMarcFile(x)).ToList();

                int rowsDeleted = _r2ProductFactory.TruncateWebR2LibraryMarcRecords();
                var rowsInserted = _r2ProductFactory.InsertWebR2LibraryMarcRecords2(r2LibraryMarcFiles);


                //var resourcesThatDoNotExistInProviders = _r2ProductFactory.GetR2ResourcesThatDoNotExist();
                //var r2LibraryMarcFiles = (from resource in resourcesThatDoNotExistInProviders
                //    let mrkFileString = GetR2LibraryMrkFileText(resource)
                //    select new R2LibraryMarcFile
                //    {
                //        MrkText = mrkFileString,
                //        ProviderSourceId = 4,
                //        Isbn = resource.Isbn,
                //        Isbn10 = resource.Isbn10,
                //        Isbn13 = resource.Isbn13,
                //        EIsbn = resource.EIsbn
                //    }).ToList();

                //List<R2LibraryMarcFile> nlmandLcR2LibraryMarcFiles = _r2ProductFactory.GetNlmAndLcMarcFiles();

                //RemoveFieldsFromExternalMarcFiles(nlmandLcR2LibraryMarcFiles);

                //r2LibraryMarcFiles.AddRange(nlmandLcR2LibraryMarcFiles);

                //int recordsUpdated = 0;
                
                //if (_r2ProductFactory.CreateAllR2LibraryMarcRecords())
                //{
                //    recordsInserted = _r2ProductFactory.InsertWebR2LibraryMarcRecords(r2LibraryMarcFiles);
                //}
                //else
                //{
                //    //Updates and inserts
                //    _r2ProductFactory.UpdateWebR2LibraryMarcRecords(r2LibraryMarcFiles, out recordsUpdated, out recordsInserted);
                //}
                
                step.CompletedSuccessfully = true;
                step.Results.AppendFormat($"R2Library Records Deleted: {rowsDeleted} | Records Inserted: {rowsInserted}");
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

              


        private void RemoveFieldsFromExternalMarcFiles(IEnumerable<R2LibraryMarcFile> nlmandLcR2LibraryMarcFiles)
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
                File.Delete(mrkFilePath);
                nlmandLcR2LibraryMarcFile.MrkText = newMrkFile;
            }
            
        }


    }
}
