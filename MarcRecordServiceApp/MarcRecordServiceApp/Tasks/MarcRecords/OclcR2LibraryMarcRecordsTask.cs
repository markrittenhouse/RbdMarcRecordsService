using System;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories;
using MarcRecordServiceApp.Core.MarcRecord;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class OclcR2LibraryMarcRecordsTask : TaskBase2
    {
        private readonly R2ProductFactory _r2ProductFactory;
        private readonly DateTime _currentDateTime = DateTime.Now;

        public OclcR2LibraryMarcRecordsTask(R2ProductFactory r2ProductFactory)
            : base("Generate OCLC R2library MArC Records", "CreateOclcR2libraryMarcRecords")
        {
            _r2ProductFactory = r2ProductFactory;
        }

        public override void Run()
        {
            Log.InfoFormat("Task - {0} - STARTED ...", TaskResult.Name);

            TaskResultStep step = new TaskResultStep
            {
                Name = "OclcR2LibraryMarcRecordsTask.CreateOclcR2libraryMarcRecords",
                TaskResultId = TaskResult.Id,
                StartTime = DateTime.Now
            };
            TaskResult.AddStep(step);

            try
            {

                var resourceThatNeedProcessing = _r2ProductFactory.GetOclcR2ResourcesThatDoNotExistOrNeedUpdated();

                var r2LibraryMarcFiles = (from resource in resourceThatNeedProcessing
                                          let mrkFileString = GetOclcR2LibraryMrkFileText(resource)
                                          select new R2LibraryMarcFile
                                          {
                                              MrkText = mrkFileString,
                                              ProviderSourceId = 5,
                                              Isbn = resource.Isbn,
                                              Isbn10 = resource.Isbn10,
                                              Isbn13 = resource.Isbn13,
                                              EIsbn = resource.EIsbn
                                          }).ToList();

                int recordsUpdated = 0;
                int recordsInserted;
                if (_r2ProductFactory.CreateAllOclcR2LibraryMarcRecords())
                {
                    recordsInserted = _r2ProductFactory.InsertOclcR2LibraryMarcRecords(r2LibraryMarcFiles);
                }
                else
                {
                    _r2ProductFactory.UpdateOclcR2LibraryMarcRecords(r2LibraryMarcFiles, out recordsUpdated, out recordsInserted);
                }

                step.CompletedSuccessfully = true;
                step.Results.AppendFormat("R2Library OCLC Records Updated: {0} | Records Inserted: {1}", recordsUpdated, recordsInserted);
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

        private string GetOclcR2LibraryMrkFileText(R2Resource resource)
        {
            try
            {
                StringBuilder mrkFileText = new StringBuilder();

                mrkFileText.AppendFormat("=LDR  {0}nam  22{1}2a 4500", GetNext5DigitRandomNumber(), GetNext5DigitRandomNumber()).AppendLine();

                mrkFileText.AppendFormat("=001  {0}", resource.Isbn).AppendLine();
                mrkFileText.AppendFormat("=005  {0:yyyyMMddhhmmss}.0", DateTime.Now).AppendLine();
                mrkFileText.AppendLine("=006  m|||||o||d||||||||");
                mrkFileText.AppendFormat("=007  cr{0}bn", GetSpace(1)).AppendLine();
                mrkFileText.AppendFormat("=008  {0:yyMMdd}s{1:0000}{2}xxua{3}o{4}000{5}0{5}eng{5}d", _currentDateTime, resource.PublicationYear, GetSpace(4), GetSpace(4), GetSpace(5), GetSpace(1)).AppendLine();

                var firstAuthorString = resource.AuthorList.Any()
                    ? resource.AuthorList.First(x => x.Order == 1).ToDisplayName()
                    : resource.FirstAuthor;

                if (!string.IsNullOrWhiteSpace(resource.EIsbn))
                {
                    mrkFileText.AppendFormat("=020  {0}$a{1}", GetSpace(2), resource.EIsbn).AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(resource.Isbn10))
                {
                    mrkFileText.AppendFormat("=020  {0}$z{1}", GetSpace(2), resource.Isbn10).AppendLine();
                }
                if (!string.IsNullOrWhiteSpace(resource.Isbn13))
                {
                    mrkFileText.AppendFormat("=020  {0}$z{1}", GetSpace(2), resource.Isbn13).AppendLine();
                }
                

                mrkFileText.AppendFormat("=037  {0}$bRittenhouse Book Distributors, Inc", GetSpace(2)).AppendLine();
                mrkFileText.AppendFormat("=100  1{0}$a{1}", GetSpace(1), firstAuthorString).AppendLine();

                mrkFileText.AppendFormat("=245  10$a{0}$h[electronic resource] /$c{1}", StripOffCarriageReturnAndLineFeed(resource.Title), firstAuthorString).AppendLine();

                mrkFileText.AppendFormat("=260  {0}$b{1},$c{2}", GetSpace(2), resource.PublisherName, resource.PublicationYear).AppendLine();
                mrkFileText.AppendFormat("=300  {0}$a online resource", GetSpace(2)).AppendLine();
                mrkFileText.AppendFormat("=533  {0}$aeBook.$bKing of Prussia, PA:$cRittenhouse Book Distributors, Inc,$d{1}", GetSpace(2), resource.PublicationYear).AppendLine();
                mrkFileText.AppendFormat("=530  {0}$aAlso available in print edition.", GetSpace(2)).AppendLine();
                mrkFileText.AppendFormat("=538  {0}$a Mode of Access: World Wide Web", GetSpace(2)).AppendLine();
                mrkFileText.AppendFormat("=588  {0}$aOnline resource.", GetSpace(2)).AppendLine();
                WriteCategories(mrkFileText, resource);//650

                mrkFileText.AppendFormat("=655  {0}4$a Electronic books", GetSpace(1)).AppendLine();

                WriteAuthors(mrkFileText, resource);//700

                mrkFileText.AppendFormat("=776  1{0}$z{1}", GetSpace(1), resource.Isbn).AppendLine();

                mrkFileText.AppendFormat("=856  40$zConnect to this resource online$u{0}{1}", Settings.Default.R2WebSite, resource.Isbn).AppendLine();

                return mrkFileText.ToString();
            }
            catch (Exception ex)
            {
                Log.Info(resource != null ? resource.ToDebugString() : "Product is null!");
                Log.Error(ex.Message, ex);
                throw;
            }

        }
    }
}
