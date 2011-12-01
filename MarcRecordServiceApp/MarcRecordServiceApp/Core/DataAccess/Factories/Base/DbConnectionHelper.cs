using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace MarcRecordServiceApp.Core.DataAccess.Factories.Base
{
    public class DbConnectionHelper
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected static SqlConnection GetRittenhouseConnection()
        {
            try
            {
                SqlConnection cnn = new SqlConnection(Settings.Default.RittenhouseMarcDb);
                if (cnn.State != ConnectionState.Open)
                {
                    cnn.Open();
                }
                return cnn;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected static SqlConnection GetConnection(string connectionString)
        {
            try
            {
                //SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

                //Log.DebugFormat("Pooling: {0}, MaxPoolSize: {1}, MinPoolSize: {2}", connectionStringBuilder.Pooling, connectionStringBuilder.MaxPoolSize, connectionStringBuilder.MinPoolSize);

                SqlConnection cnn = new SqlConnection(connectionString);
                if (cnn.State != ConnectionState.Open)
                {
                    cnn.Open();
                }
                return cnn;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="command"></param>
        /// <param name="reader"></param>
        protected static void DisposeConnections(SqlConnection cnn, SqlCommand command, SqlDataReader reader)
        {
            if (null != reader)
            {
                reader.Dispose();
            }

            if (null != command)
            {
                command.Dispose();
            }

            if (null != cnn)
            {
                cnn.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="command"></param>
        protected static void DisposeConnections(SqlConnection cnn, SqlCommand command)
        {
            if (null != command)
            {
                command.Dispose();
            }

            if (null != cnn)
            {
                cnn.Dispose();
            }
        }

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//protected static string GetEktronDatabaseName()
		//{
		//    string ektronConnectionString = ConfigurationManager.ConnectionStrings["Ektron.DbConnection"].ToString();
		//    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(ektronConnectionString);

		//    Log.DebugFormat("database: {0}", sqlConnectionStringBuilder["database"]);
		//    return sqlConnectionStringBuilder["database"].ToString();
		//}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected static string GetDatabaseName(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            Log.DebugFormat("database: {0}", sqlConnectionStringBuilder["database"]);
            return sqlConnectionStringBuilder["database"].ToString();
        }
    }
}
