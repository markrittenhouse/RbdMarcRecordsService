using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;
using Rittenhouse.RBD.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Core.DataAccess.Factories.Base
{
    public class EntityFactory : FactoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectStatement"></param>
        /// <param name="sqlCommandParameters"></param>
        /// <param name="logSql"></param>
        /// <returns></returns>
        public static T GetFirstEntity<T>(string selectStatement, List<ISqlCommandParameter> sqlCommandParameters, bool logSql) where T : IDataEntity, new()
        {
			return GetFirstEntity<T>(selectStatement, sqlCommandParameters, logSql, Settings.Default.RittenhouseMarcDb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectStatement"></param>
        /// <param name="sqlCommandParameters"></param>
        /// <param name="logSql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static T GetFirstEntity<T>(string selectStatement, List<ISqlCommandParameter> sqlCommandParameters, bool logSql, string connectionString) where T : IDataEntity, new()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopwatch = new Stopwatch();

            T item = new T();

            try
            {
                cnn = GetConnection(connectionString);
                command = GetSqlCommand(cnn, selectStatement, sqlCommandParameters.ToArray());

                if (logSql)
                {
                    LogCommandDebug(command);
                    stopwatch.Start();
                }

                reader = command.ExecuteReader();

                //Log.DebugFormat("query time: {0}ms", stopwatch.ElapsedMilliseconds);
                if (reader.Read())
                {
                    //Log.DebugFormat("query time: {0}ms", stopwatch.ElapsedMilliseconds);
                    item.Populate(reader);
                    //Log.DebugFormat("query time: {0}ms", stopwatch.ElapsedMilliseconds);
                }

                if (logSql)
                {
                    stopwatch.Stop();
                    Log.DebugFormat("query time: {0}ms", stopwatch.ElapsedMilliseconds);
                }
                return item;
            }
            catch (Exception ex)
            {
                LogCommandInfo(command);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectStatement"></param>
        /// <param name="sqlCommandParameters"></param>
        /// <param name="logSql"></param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(string selectStatement, List<ISqlCommandParameter> sqlCommandParameters, bool logSql) where T : IDataEntity, new()
        {
            return GetEntityList<T>(selectStatement, sqlCommandParameters, logSql, Settings.Default.RittenhouseMarcDb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectStatement"></param>
        /// <param name="sqlCommandParameters"></param>
        /// <param name="logSql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(string selectStatement, List<ISqlCommandParameter> sqlCommandParameters, bool logSql, string connectionString) where T : IDataEntity, new()
        {
            SqlConnection cnn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            Stopwatch stopwatch = new Stopwatch();

            List<T> items = new List<T>();

            try
            {
                cnn = GetConnection(connectionString);

                command = sqlCommandParameters == null ? GetSqlCommand(cnn, selectStatement) : GetSqlCommand(cnn, selectStatement, sqlCommandParameters.ToArray());

                if (logSql)
                {
                    LogCommandDebug(command);
                    stopwatch.Start();
                }

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T item = new T();
                    item.Populate(reader);
                    items.Add(item);
                }

                if (logSql)
                {
                    stopwatch.Stop();
                    Log.DebugFormat("query time: {0}ms, count: {1}", stopwatch.ElapsedMilliseconds, items.Count);
                }

                return items;
            }
            catch (Exception ex)
            {
                LogCommandInfo(command);
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                DisposeConnections(cnn, command, reader);
            }
        }

    }
}
