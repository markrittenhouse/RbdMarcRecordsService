using System;
using System.Data.SqlClient;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;
using log4net;

namespace MarcRecordServiceApp.Core.DataAccess.Factories.Base
{
    public class DbCommandHelper : DbConnectionHelper
    {
        protected new static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected static SqlCommand GetSqlCommand(SqlConnection cnn, string commandText)
        {
            return GetSqlCommand(cnn, commandText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="commandText"></param>
        /// <param name="sqlCommandParameters"></param>
        /// <returns></returns>
        protected static SqlCommand GetSqlCommand(SqlConnection cnn, string commandText, ISqlCommandParameter[] sqlCommandParameters)
        {
            return GetSqlCommand(cnn, commandText, sqlCommandParameters,  Settings.Default.DatabaseCommandTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="commandText"></param>
        /// <param name="sqlCommandParameters"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected static SqlCommand GetSqlCommand(SqlConnection cnn, string commandText, ISqlCommandParameter[] sqlCommandParameters, int commandTimeout)
        {
            if (cnn == null)
            {
                Log.Warn("connection object is null");
                return null;
            }

            SqlCommand command = cnn.CreateCommand();
            command.CommandText = commandText;
            command.CommandTimeout = commandTimeout;

            if (sqlCommandParameters != null)
            {
                foreach (ISqlCommandParameter parameter in sqlCommandParameters)
                {
                    parameter.SetCommandParmater(command);
                }
            }

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, string parameterValue)
        {
            if (null == parameterValue)
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="useEmptyString"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, string parameterValue, bool useEmptyString)
        {
            if (null == parameterValue)
            {
                if (useEmptyString)
                {
                    command.Parameters.AddWithValue(string.Format("@{0}", parameterName), string.Empty);
                }
                else
                {
                    command.Parameters.AddWithValue(string.Format("@{0}", parameterName), DBNull.Value);
                }
            }
            else
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, bool parameterValue)
        {
            command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, int parameterValue)
        {
            command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, decimal parameterValue)
        {
            command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, DateTime parameterValue)
        {
            if (parameterValue > DateTime.MinValue)
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
            }
            else
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), DBNull.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, DateTime? parameterValue)
        {
            if (parameterValue != null)
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
            }
            else
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), DBNull.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected static void SetCommandParmater(SqlCommand command, string parameterName, byte[] parameterValue)
        {
            if (null == parameterValue)
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(string.Format("@{0}", parameterName), parameterValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        protected static void LogCommandInfo(SqlCommand command)
        {
            if (command == null)
            {
                Log.Info("command object is null");
                return;
            }

            try
            {
                Log.InfoFormat("CommandTimeout: {0}, CommandType: {1}", command.CommandTimeout, command.CommandType);
                Log.InfoFormat("CommandText: {0}", command.CommandText);
                foreach (SqlParameter parameter in command.Parameters)
                {
                    Log.InfoFormat("{0} = {1}", parameter.ParameterName, parameter.Value);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        protected static void LogCommandDebug(SqlCommand command)
        {
            if (command == null)
            {
                Log.Info("command object is null");
                return;
            }

            try
            {
                Log.DebugFormat("CommandText: {0}", command.CommandText);
                StringBuilder sb = new StringBuilder();
                foreach (SqlParameter parameter in command.Parameters)
                {
                    sb.AppendFormat("{0}{1} = {2}", (sb.Length > 0) ? ", " : string.Empty, parameter.ParameterName, parameter.Value);
                }
                if (sb.Length > 0)
                {
                    Log.Debug(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
    }
}
