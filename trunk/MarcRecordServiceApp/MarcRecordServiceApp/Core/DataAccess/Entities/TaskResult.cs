using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class TaskResult : FactoryBase, IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool CompletedSuccessfully { get; set; }
        public string RunComments { get; set; }

        public List<TaskResultStep> Steps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskResult()
        {
            Steps = new List<TaskResultStep>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        public void AddStep(TaskResultStep step)
        {
            if (step.Id <= 0)
            {
                StringBuilder info = new StringBuilder()
                    .AppendFormat(">>>> Step Started - {0} <<<<", step.Name);

                Log.Info(info.ToString());
            }
            Steps.Add(step);
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
            return (DateTime)EndTime - StartTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("TaskResult = [")
                .AppendFormat("Id = {0}", Id)
                .AppendFormat(", Name = {0}", Name)
                .AppendFormat(", StartTime = {0:u}", StartTime)
                .AppendFormat(", EndTime = {0:u}", EndTime)
                .AppendFormat(", CompletedSuccessfully = {0}", CompletedSuccessfully)
                .AppendFormat(", RunComments = {0}", RunComments)
                .Append("]");

            return sb.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void Populate(SqlDataReader reader)
        {
            Id = GetInt32Value(reader, "taskResultId", -1);
            Name = GetStringValue(reader, "taskName");
            StartTime = GetDateValue(reader, "taskStartTime");
            EndTime = GetDateValueOrNull(reader, "taskEndTime");
            CompletedSuccessfully = GetBoolValue(reader, "taskCompletedSuccessfully", false);
            RunComments = GetStringValue(reader, "taskRunComments");            
        }
    }
}
