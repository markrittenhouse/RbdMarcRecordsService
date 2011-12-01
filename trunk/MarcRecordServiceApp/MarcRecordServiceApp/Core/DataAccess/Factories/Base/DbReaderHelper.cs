using System;
using System.Data;
using log4net;

namespace MarcRecordServiceApp.Core.DataAccess.Factories.Base
{
    public class DbReaderHelper : DbCommandHelper
    {
        protected new static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected static string GetStringValue(IDataReader reader, string fieldName)
        {
            try
            {
                return GetStringValue(reader, fieldName, null);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}", fieldName);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static string GetStringValue(IDataReader reader, int fieldIndex)
        {
            try
            {
                return GetStringValue(reader, fieldIndex, null);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}", fieldIndex);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static string GetStringValue(IDataReader reader, string fieldName, string defaultValue)
        {
            try
            {
                return GetStringValue(reader, reader.GetOrdinal(fieldName), defaultValue);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static string GetStringValue(IDataReader reader, int fieldIndex, string defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetString(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <param name="fieldName"></param>
        ///// <param name="defaultValue"></param>
        ///// <returns></returns>
        //protected static char GetCharValue(IDataReader reader, string fieldName, char defaultValue)
        //{
        //    try
        //    {
        //        return GetCharValue(reader, reader.GetOrdinal(fieldName), defaultValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <param name="fieldIndex"></param>
        ///// <param name="defaultValue"></param>
        ///// <returns></returns>
        //protected static char GetCharValue(IDataReader reader, int fieldIndex, char defaultValue)
        //{
        //    try
        //    {
        //        //return reader.IsDBNull(fieldIndex) ? defaultValue : reader.GetChar(fieldIndex);
        //        return reader.GetChar(fieldIndex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected static DateTime GetDateValue(IDataReader reader, string fieldName)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetDateValue(reader, fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}", fieldName);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static DateTime GetDateValue(IDataReader reader, int fieldIndex)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return DateTime.MinValue;
                }
                return reader.GetDateTime(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}", fieldIndex);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static DateTime GetDateValue(IDataReader reader, string fieldName, DateTime defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetDateValue(reader, fieldIndex, defaultValue);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}", fieldName);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static DateTime GetDateValue(IDataReader reader, int fieldIndex, DateTime defaultValue)
        {
            try
            {
                return reader.IsDBNull(fieldIndex) ? defaultValue : reader.GetDateTime(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}", fieldIndex);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected static DateTime? GetDateValueOrNull(IDataReader reader, string fieldName)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetDateValueOrNull(reader, fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}", fieldName);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static DateTime? GetDateValueOrNull(IDataReader reader, int fieldIndex)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return null;
                }
                return reader.GetDateTime(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}", fieldIndex);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static bool GetBoolValue(IDataReader reader, string fieldName, bool defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetBoolValue(reader, fieldIndex, defaultValue);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static bool GetBoolValue(IDataReader reader, int fieldIndex, bool defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetBoolean(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static int GetInt32Value(IDataReader reader, string fieldName, int defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetInt32Value(reader, fieldIndex, defaultValue);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static int GetInt32Value(IDataReader reader, int fieldIndex, int defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetInt32(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected static int? GetInt32Value(IDataReader reader, string fieldName)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetInt32Value(reader, fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}", fieldName);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static int? GetInt32Value(IDataReader reader, int fieldIndex)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return null;
                }
                return reader.GetInt32(fieldIndex);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}", fieldIndex);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static short GetInt16Value(IDataReader reader, string fieldName, short defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetInt16Value(reader, fieldIndex, defaultValue);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static short GetInt16Value(IDataReader reader, int fieldIndex, short defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetInt16(fieldIndex);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static long GetInt64Value(IDataReader reader, string fieldName, long defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetInt64Value(reader, fieldIndex, defaultValue);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static long GetInt64Value(IDataReader reader, int fieldIndex, long defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return Int64.Parse(reader[fieldIndex].ToString());

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static double GetDoubleValue(IDataReader reader, string fieldName, double defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetDoubleValue(reader, fieldIndex, defaultValue);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static double GetDoubleValue(IDataReader reader, int fieldIndex, double defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetDouble(fieldIndex);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Decimal GetDecimalValue(IDataReader reader, string fieldName, Decimal defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetDecimalValue(reader, fieldIndex, defaultValue);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Decimal GetDecimalValue(IDataReader reader, int fieldIndex, Decimal defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetDecimal(fieldIndex);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static float GetFloatValue(IDataReader reader, string fieldName, float defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                Log.DebugFormat("fieldName: {0}, fieldIndex: {1}", fieldName, fieldIndex);
                return GetFloatValue(reader, fieldIndex, defaultValue);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static float GetFloatValue(IDataReader reader, int fieldIndex, float defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetFloat(fieldIndex);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Guid GetGuidValue(IDataReader reader, string fieldName, Guid defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetGuidValue(reader, fieldIndex, defaultValue);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Guid GetGuidValue(IDataReader reader, int fieldIndex, Guid defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetGuid(fieldIndex);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static byte GetByteValue(IDataReader reader, string fieldName, byte defaultValue)
        {
            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetByteValue(reader, fieldIndex, defaultValue);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldName: {0}, defaultValue: {1}", fieldName, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static byte GetByteValue(IDataReader reader, int fieldIndex, byte defaultValue)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return defaultValue;
                }
                return reader.GetByte(fieldIndex);

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}, defaultValue: {1}", fieldIndex, defaultValue);
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static byte[] GetByteArrayValue(IDataReader reader, int fieldIndex)
        {
            try
            {
                if (reader.IsDBNull(fieldIndex))
                {
                    return null;
                }

                reader.Read();
                long size = reader.GetBytes(0, 0, null, 0, 0);  //get the length of data
                byte[] values = new byte[size];

                const int bufferSize = 8;
                long bytesRead = 0;
                int curPos = 0;

                while (bytesRead < size)
                {
                    bytesRead += reader.GetBytes(0, curPos, values, curPos, bufferSize);
                    curPos += bufferSize;
                }

                return values;

            }
            catch (Exception ex)
            {
                Log.WarnFormat("fieldIndex: {0}", fieldIndex);
                Log.Error(ex.Message, ex);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static bool GetBoolValue(DataRow dataRow, string fieldName, bool defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;

            return GetBoolValue(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static bool GetBoolValue(DataRow dataRow, int fieldIndex, bool defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (bool)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static byte[] GetByteArrayValue(DataRow dataRow, int fieldIndex)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return null;
            }

            return (byte[])dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static byte GetByteValue(DataRow dataRow, string fieldName, byte defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;

            return GetByteValue(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static byte GetByteValue(DataRow dataRow, int fieldIndex, byte defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (byte)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected static DateTime GetDateValue(DataRow dataRow, string fieldName)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetDateValue(dataRow, fieldIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static DateTime GetDateValue(DataRow dataRow, int fieldIndex)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return DateTime.MinValue;
            }
            return (DateTime)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Decimal GetDecimalValue(DataRow dataRow, string fieldName, Decimal defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetDecimalValue(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Decimal GetDecimalValue(DataRow dataRow, int fieldIndex, Decimal defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (Decimal)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static double GetDoubleValue(DataRow dataRow, string fieldName, double defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetDoubleValue(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static double GetDoubleValue(DataRow dataRow, int fieldIndex, double defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (double)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Guid GetGuidValue(DataRow dataRow, string fieldName, Guid defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetGuidValue(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static Guid GetGuidValue(DataRow dataRow, int fieldIndex, Guid defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (Guid)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static int GetInt16Value(DataRow dataRow, string fieldName, short defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetInt16Value(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static short GetInt16Value(DataRow dataRow, int fieldIndex, short defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return Int16.Parse(dataRow[fieldIndex].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static int GetInt32Value(DataRow dataRow, string fieldName, int defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetInt32Value(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static int GetInt32Value(DataRow dataRow, int fieldIndex, int defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return Int32.Parse(dataRow[fieldIndex].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static long GetInt64Value(DataRow dataRow, string fieldName, long defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetInt64Value(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static long GetInt64Value(DataRow dataRow, int fieldIndex, long defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return Int64.Parse(dataRow[fieldIndex].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected static string GetStringValue(DataRow dataRow, string fieldName)
        {
            return GetStringValue(dataRow, fieldName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        protected static string GetStringValue(DataRow dataRow, int fieldIndex)
        {
            return GetStringValue(dataRow, fieldIndex, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static string GetStringValue(DataRow dataRow, string fieldName, string defaultValue)
        {
            DataColumn c = dataRow.Table.Columns[fieldName];
            int fieldIndex = c.Ordinal;
            return GetStringValue(dataRow, fieldIndex, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static string GetStringValue(DataRow dataRow, int fieldIndex, string defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (string)dataRow[fieldIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected static float GetFloatValue(DataRow dataRow, int fieldIndex, float defaultValue)
        {
            if (dataRow[fieldIndex] == DBNull.Value)
            {
                return defaultValue;
            }
            return (float)dataRow[fieldIndex];
        }

    }
}
