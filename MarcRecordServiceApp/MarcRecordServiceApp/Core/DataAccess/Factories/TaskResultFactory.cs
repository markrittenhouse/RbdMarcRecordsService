using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class TaskResultFactory : FactoryBase
    {
		private static readonly string DbConnectionString = Settings.Default.RittenhouseMarcDb;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskResult"></param>
        /// <returns></returns>
        public static int InsertTaskResult(TaskResult taskResult)
        {
            StringBuilder sql = new StringBuilder()
                .Append("insert into TaskResult (taskName, taskStartTime, taskEndTime, taskCompletedSuccessfully, taskRunComments) ")
                .Append("values (@TaskName, @TaskStartTime, @TaskEndTime, @TaskCompletedSuccessfully, @TaskRunComments)");

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new StringParameter("TaskName", taskResult.Name),
                                                            new DateTimeParameter("TaskStartTime", taskResult.StartTime),
                                                            new DateTimeNullParameter("TaskEndTime", taskResult.EndTime),
                                                            new BooleanParameter("TaskCompletedSuccessfully", taskResult.CompletedSuccessfully),
                                                            new StringParameter("TaskRunComments", taskResult.RunComments)
                                                        };

			int id = ExecuteInsertStatementReturnIdentity(sql.ToString(), parameters.ToArray(), true, DbConnectionString);
            taskResult.Id = id;
            return id;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskResult"></param>
        /// <returns></returns>
        public static int UpdateTaskResult(TaskResult taskResult)
        {
            //SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.RittenhouseWebDb);
            StringBuilder sql = new StringBuilder()
                .Append("update TaskResult set ")
                .Append("       taskEndTime = @TaskEndTime ")
                .Append("     , taskCompletedSuccessfully = @TaskCompletedSuccessfully ")
                .Append("     , taskRunComments = @TaskRunComments ")
                .Append("where  taskResultId = @TaskResultId");


            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new DateTimeNullParameter("TaskEndTime", taskResult.EndTime),
                                                            new BooleanParameter("TaskCompletedSuccessfully", taskResult.CompletedSuccessfully),
                                                            new StringParameter("TaskRunComments", taskResult.RunComments),
                                                            new Int32Parameter("TaskResultId", taskResult.Id)
                                                        };

			int rowCount = ExecuteUpdateStatement(sql.ToString(), parameters.ToArray(), true, DbConnectionString);
            return rowCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskResultStep"></param>
        /// <returns></returns>
        public static int InsertTaskResultStep(TaskResultStep taskResultStep)
        {
            StringBuilder sql = new StringBuilder()
                .Append("insert into TaskResultStep (taskResultId, stepName, stepStartTime, stepEndTime, stepCompletedSuccessfully, filePath, fileTimestamp, fileSizeInBytes, stepResults) ")
                .Append("values (@TaskResultId, @StepName, @StepStartTime, @StepEndTime, @StepCompletedSuccessfully, @FilePath, @FileTimestamp, @FileSizeInBytes, @StepResults)");

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new Int32Parameter("TaskResultId", taskResultStep.TaskResultId),
                                                            new StringParameter("StepName", taskResultStep.Name),
                                                            new DateTimeParameter("StepStartTime", taskResultStep.StartTime),
                                                            new DateTimeNullParameter("StepEndTime", taskResultStep.EndTime),
                                                            new BooleanParameter("StepCompletedSuccessfully", taskResultStep.CompletedSuccessfully),
                                                            new StringParameter("StepResults", taskResultStep.Results.ToString()),
                                                            new StringParameter("FilePath", taskResultStep.FilePath),
                                                            new DateTimeParameter("FileTimestamp", taskResultStep.FileTimestamp),
                                                            new Int64Parameter("FileSizeInBytes", taskResultStep.FileSize)
                                                        };

            int id = ExecuteInsertStatementReturnIdentity(sql.ToString(), parameters.ToArray(), true, DbConnectionString);
            taskResultStep.Id = id;

            StringBuilder info = new StringBuilder()
                .AppendLine(">>>> Step Complete")
                .AppendFormat("{0} - {1} - {2}", taskResultStep.Id, (taskResultStep.CompletedSuccessfully) ? "ok" : "ERROR", taskResultStep.Name).AppendLine()
                .AppendFormat("{0}", taskResultStep.Results).AppendLine("<<<<");
            Log.Info(info.ToString());

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <param name="taskResultId"></param>
        /// <returns></returns>
        public static List<TaskResult> GetTaskResultsFromDate(DateTime minDate, DateTime maxDate, int taskResultId)
        {
            StringBuilder sql = new StringBuilder()
                .Append("select taskResultId, taskName, taskStartTime, taskEndTime, taskCompletedSuccessfully, taskRunComments ")
                .Append("from   TaskResult ")
                .Append("where  taskStartTime >= @MinDate ")
                .Append("  and  taskStartTime < @MaxDate ")
                .Append("  and  taskResultId <> @TaskResultId ")
                .Append("order by taskResultId desc ");

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new DateTimeParameter("MinDate", minDate),
                                                            new DateTimeParameter("MaxDate", maxDate),
                                                            new Int32Parameter("TaskResultId", taskResultId)
                                                        };

			List<TaskResult> taskResultEntities = EntityFactory.GetEntityList<TaskResult>(sql.ToString(), parameters, true, DbConnectionString);

            sql = new StringBuilder()
                .Append("select trs.taskResultStepId, trs.taskResultId, trs.stepName, trs.stepStartTime, trs.stepEndTime, trs.stepCompletedSuccessfully, trs.filePath, trs.fileTimestamp, trs.fileSizeInBytes, trs.stepResults ")
                .Append("from   TaskResult tr join  dbo.TaskResultStep trs on trs.taskResultId = tr.taskResultId ")
                .Append("where  tr.taskStartTime >= @MinDate ")
                .Append("  and  tr.taskStartTime < @MaxDate ")
                .Append("  and  tr.taskResultId <> @TaskResultId ")
                .Append("order by trs.taskResultId, trs.taskResultStepId ");

			List<TaskResultStep> taskResultStepEntities = EntityFactory.GetEntityList<TaskResultStep>(sql.ToString(), parameters, true, DbConnectionString);

            foreach (TaskResult taskResult in taskResultEntities)
            {
                foreach (TaskResultStep step in taskResultStepEntities)
                {
                    if (step.TaskResultId == taskResult.Id)
                    {
                        taskResult.Steps.Add(step);
                    }
                }
            }

            taskResultEntities = taskResultEntities.OrderByDescending(x => x.StartTime).ToList();
            return taskResultEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public static TaskResult GetPreviousTaskResult(string taskName)
        {
            StringBuilder sql = new StringBuilder()
                .Append("select top 1 taskResultId, taskName, taskStartTime, taskEndTime, taskCompletedSuccessfully, taskRunComments ")
                .Append("from   dbo.TaskResult ")
                .Append("where  taskName = @TaskName ")
                .Append("order by taskResultId desc ");

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new StringParameter("TaskName", taskName)
                                                        };

            TaskResult taskResult = EntityFactory.GetFirstEntity<TaskResult>(sql.ToString(), parameters, true, DbConnectionString);

            sql = new StringBuilder()
                .Append("select trs.taskResultStepId, trs.taskResultId, trs.stepName, trs.stepStartTime, trs.stepEndTime, trs.stepCompletedSuccessfully, trs.filePath, trs.fileTimestamp, trs.fileSizeInBytes, trs.stepResults ")
                .Append("from   dbo.TaskResultStep trs  ")
                .Append("where  trs.taskResultId = @TaskResultId ")
                .Append("order by trs.taskResultStepId ");

            parameters = new List<ISqlCommandParameter>
                             {
                                 new Int32Parameter("TaskResultId", taskResult.Id)
                             };

            List<TaskResultStep> taskResultStepEntities = EntityFactory.GetEntityList<TaskResultStep>(sql.ToString(), parameters, true, DbConnectionString);

            foreach (TaskResultStep step in taskResultStepEntities)
            {
                taskResult.Steps.Add(step);
            }

            return taskResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public static TaskResultStep GetLatestTaskResultStepByName(string stepName)
        {
            string sql = new StringBuilder()
                .Append("select top 1 taskResultStepId, taskResultId, stepName, stepStartTime, stepEndTime, stepCompletedSuccessfully, filePath, fileTimestamp, fileSizeInBytes, stepResults ")
                .Append("from   TaskResultStep ")
                .Append("where  stepName = @StepName ")
                .Append("order by stepStartTime desc, taskResultStepId desc; ")
                .ToString();


            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new StringParameter("StepName", stepName)
                                                        };

			TaskResultStep step = EntityFactory.GetFirstEntity<TaskResultStep>(sql, parameters, true, DbConnectionString);

            return step;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int GetPreludeDataTableColumnCount(string tableName)
        {
            StringBuilder sql = new StringBuilder()
                .Append("select count(*) As ColumnCount ")
                .Append("from   sys.syscolumns ")
                .Append("where id = (select id from sys.sysobjects where name = @TableName) ");

            List<ISqlCommandParameter> parameters = new List<ISqlCommandParameter>
                                                        {
                                                            new StringParameter("TableName", tableName)
                                                        };


			return ExecuteBasicCountQuery(sql.ToString(), parameters, true, DbConnectionString);
        }
    }
}
