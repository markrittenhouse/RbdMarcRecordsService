using System;
using System.Data.SqlClient;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class TaskResultStep : FactoryBase, IDataEntity
    {
        public int Id { get; set; }
        public int TaskResultId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool CompletedSuccessfully { get; set; }
        public StringBuilder Results { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime FileTimestamp { get; set; }

        public bool FileWasPreviouslyProcessed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskResultStep()
        {
            Results = new StringBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetRunTime()
        {
            if (EndTime == null)
            {
                return new TimeSpan(0, 0, 0, 0, 0);
            }
            return (DateTime) EndTime - StartTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("TaskResultStep = [")
                .AppendFormat("Id = {0}", Id)
                .AppendFormat(", TaskResultId = {0}", TaskResultId)
                .AppendFormat(", Name = {0}", Name)
                .AppendFormat(", StartTime = {0:u}", StartTime)
                .AppendFormat(", EndTime = {0:u}", EndTime)
                .AppendFormat(", CompletedSuccessfully = {0}", CompletedSuccessfully)
                .AppendFormat(", FileSize = {0}", FileSize)
                .AppendFormat(", FileTimestamp = {0:u}", FileTimestamp)
                .AppendFormat(", FilePath = {0}", FilePath)
                .AppendFormat(", FileWasPreviouslyProcessed = {0}", FileWasPreviouslyProcessed)
                .AppendFormat(", Results = {0}", Results)
                .Append("]");

            return sb.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void Populate(SqlDataReader reader)
        {
            Id = GetInt32Value(reader, "taskResultStepId", -1);
            TaskResultId = GetInt32Value(reader, "taskResultId", -1);
            Name = GetStringValue(reader, "stepName");
            StartTime = GetDateValue(reader, "stepStartTime");
            EndTime = GetDateValueOrNull(reader, "stepEndTime");
            CompletedSuccessfully = GetBoolValue(reader, "stepCompletedSuccessfully", false);
            Results = new StringBuilder(GetStringValue(reader, "stepResults"));
            FilePath = GetStringValue(reader, "filePath");
            FileTimestamp = GetDateValue(reader, "fileTimestamp");
            FileSize = GetInt64Value(reader, "fileSizeInBytes", 0);
        }
    }
}
