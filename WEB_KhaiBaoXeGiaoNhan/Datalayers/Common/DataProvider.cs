using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace AdminPortal.DataLayer
{
    public abstract class DataProvider<T> where T : new()
    {
        /// <summary>
        /// 	Logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static T _instance = default(T);

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }

        /// <summary>
        /// Log Object
        /// </summary>
        /// <param name="obj"></param>
        public void DumpObject(object obj)
        {
            StringBuilder builder = new StringBuilder("");
            var dumbObj = ObjectDumper.Dump(obj, DumpStyle.CSharp);
            var type = obj.GetType().Name;
            builder.Append(string.Format("Preparing dumb {0}: ", type));
            builder.Append(dumbObj);
            _logger.Debug(builder.ToString());
        }

        ///// <summary>
        ///// Hàm thêm biến vào câu lệnh sql
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="parameterName"></param>
        ///// <param name="value"></param>
        ///// <param name="type"></param>
        //protected void AddSqlParameter(SqlCommand command, string parameterName, object value, SqlDbType type)
        //{
        //    SqlParameter parameter = new SqlParameter(parameterName, type);
        //    parameter.Value = GetDatabaseValue(value);
        //    command.Parameters.Add(parameter);
        //}
        /// <summary>
        /// Lấy dữ liệu từ reader
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected K GetDbReaderValue<K>(object value)
        {
            if (DBNull.Value.Equals(value) == false && value != null)
            {
                return (K)value;
            }

            return default(K);
        }

        /// <summary>
        /// Hàm này dùng để thực thi câu lệnh sql kiểu dự liệu trả về là DataSet
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strSql">Truyền vào câu lệnh SQL</param>
        /// <returns>Dữ liệu trả về DataSet</returns>
        protected DataSet ExcuteDataSet(SqlConnection connection, string strSql)
        {
            _logger.DebugFormat("Try to ExcuteDataSet from sql: {0}", strSql);
            DataSet ds = new DataSet();
            using (SqlDataAdapter odbcAdt = new SqlDataAdapter(strSql, connection))
            {
                odbcAdt.Fill(ds);
            }
            return ds;
        }

        /// <summary>
        /// Hàm này dùng để thực thi câu lệnh sql như Insert, Update, Delete
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strSQL">Truyền vào câu lệnh SQL</param>
        protected void ExecuteQuery(SqlConnection connection, string strSQL)
        {
            SqlTransaction trans = null;
            try
            {
                trans = connection.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand(strSQL, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    WriteLogExecutingCommand(cmd);

                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                if (trans != null) trans.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Hàm này dùng để thực thi câu lệnh sql kiểu dự liệu trả về là DataReader
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strSQL">Truyền vào câu lệnh SQL</param>
        /// <returns>Dữ liệu trả về DataReader</returns>
        protected SqlDataReader ExcuteDataReader(SqlConnection connection, string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.Text;

                WriteLogExecutingCommand(cmd);

                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                _logger.Error("Execute reader failed.", ex);
                throw;
            }
        }

        ///// <summary>
        ///// Đọc dữ liệu từ Reader
        ///// </summary>
        ///// <typeparam name="K"></typeparam>
        ///// <param name="value"></param>
        ///// <returns>Giá trị K</returns>
        //protected K GetDbReaderValue<K>(object value)
        //{
        //    if (DBNull.Value.Equals(value) == false && value != null)
        //    {
        //        return (K)value;
        //    }

        //    return default(K);
        //}

        protected object ExcuteScalar(SqlConnection connection, SqlCommand cmd)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = 3000;

            WriteLogExecutingCommand(cmd);

            return cmd.ExecuteScalar();
        }

        protected void ApplyValue(object dataObject, object value, string valueProperty)
        {
            if (DBNull.Value.Equals(value) == false)
            {
                PropertyInfo propertyInfo = dataObject.GetType().GetProperty(valueProperty);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(dataObject, value, null);
                }
            }
        }

        protected static void AddSqlParameter(SqlCommand command, string parameterName, object value, System.Data.SqlDbType type)
        {
            SqlParameter parameter = new SqlParameter(parameterName, type);
            parameter.Value = GetDatabaseValue(value);
            command.Parameters.Add(parameter);
        }

        protected static object GetDatabaseValue(object value)
        {
            if (value is string)
            {
                return string.IsNullOrEmpty((string)value) ? System.DBNull.Value : value;
            }

            return value == null ? System.DBNull.Value : value;
        }

        /// <summary>
        /// Thực thi Storedprocedure không tham số
        /// </summary>
        /// <returns>Datatable</returns>
        protected DataTable GetDataTable(SqlConnection connection, SqlCommand sqlCommand)
        {
            DataTable da = new DataTable();
            try
            {
                sqlCommand.Connection = connection;
                WriteLogExecutingCommand(sqlCommand);

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(da);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
            return da;
        }

        /// <summary>
        /// datatable goi store procedure khong truyen tham so
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strQuery"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        protected DataTable GetDataTable(SqlConnection connection, string strQuery, CommandType commandType)
        {
            DataTable da = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = commandType;
                sqlCommand.Connection = connection;

                WriteLogExecutingCommand(sqlCommand);

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(da);
                }
            }
            return da;
        }

        /// <summary>
        /// Gọi storedprocedure có truyền tham số
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strQuery"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="values"></param>
        /// <returns>Datatatble</returns>
        protected DataTable GetDataTable(SqlConnection connection, string strQuery, CommandType commandType, string[] parameters, string[] values)
        {
            //khoi tao doi tuong cho dataTable
            DataTable da = new DataTable();
            //khai bao khoi tao doi tuong SqlCommand
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                //khai bao thuoc tinh cho doi tuong SqlCommand
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = commandType;
                sqlCommand.Connection = connection;
                SqlParameter sqlParameter;
                for (int i = 0; i < parameters.Length; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = parameters[i];
                    sqlParameter.SqlValue = values[i];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                WriteLogExecutingCommand(sqlCommand);

                //khai bao va khoi tao doi tuong SqlDataAdapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    //khai bao goi phuong thuc Fill cua doi tuong SqlDataAdapter
                    sqlDataAdapter.Fill(da);
                }
            }
            return da;
        }

        /// <summary>
        /// Thực thi Storedprocedure
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strQuery"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="values"></param>
        /// <returns>True nếu thực thi thành công</returns>
        protected bool ExecuteSqlQuery(SqlConnection connection, string strQuery, CommandType commandType, string[] parameters, string[] values)
        {
            SqlTransaction trans = null;
            try
            {
                trans = connection.BeginTransaction();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = strQuery;
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = commandType;
                    SqlParameter sqlParameter;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = parameters[i];
                        sqlParameter.SqlValue = values[i];
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    sqlCommand.Transaction = trans;
                    sqlCommand.ExecuteNonQuery();
                    trans.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                if (trans != null)
                {
                    trans.Rollback();
                }
                throw;
            }
        }

        /// <summary>
        /// Thực thi Storedprocedure
        /// </summary>
        /// <returns>True nếu thực thi thành công</returns>
        protected bool ExecuteSqlQuery(SqlConnection connection, SqlCommand sqlCommand)
        {
            SqlTransaction trans = null;
            try
            {
                trans = connection.BeginTransaction();
                sqlCommand.Connection = connection;
                sqlCommand.Transaction = trans;
                WriteLogExecutingCommand(sqlCommand);

                sqlCommand.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                if (trans != null)
                {
                    trans.Rollback();
                }
                throw;
            }
        }

        /// <summary>
        /// Thực thi Storedprocedure
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="strQuery"></param>
        /// <param name="commandType"></param>
        /// <returns>True nếu thực thi thành công</returns>
        protected bool ExecuteSqlQuery(SqlConnection connection, string strQuery, CommandType commandType)
        {
            SqlTransaction trans = null;
            try
            {
                trans = connection.BeginTransaction();
                using (SqlCommand sqlCommand = new SqlCommand(strQuery, connection, trans))
                {
                    sqlCommand.CommandType = commandType;
                    WriteLogExecutingCommand(sqlCommand);
                    sqlCommand.ExecuteNonQuery();
                    trans.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                if (trans != null)
                {
                    trans.Rollback();
                }
                throw;
                //return false;
            }
        }

        protected static void WriteLogExecutingCommand(SqlCommand command)
        {
            StringBuilder builder = new StringBuilder("");
            builder.AppendLine("Preparing SQL command with parameters: ");
            foreach (SqlParameter param in command.Parameters)
            {
                builder.AppendLine(string.Format("declare {0} {1}; set {0}={2};",
                    param.ParameterName,
                    param.DbType.ToString(),
                    param.Value == DBNull.Value ? "null" : param.Value.ToString()));
            }
            builder.AppendLine(string.Format("SQL executing: {0} on host {1} and db {2}", command.CommandText,
                command.Connection != null ? command.Connection.DataSource : string.Empty,
                command.Connection != null ? command.Connection.Database : string.Empty));

            _logger.Debug(builder.ToString());
        }

        protected static void WriteLogErr(string command)
        {
            StringBuilder builder = new StringBuilder("");
            builder.AppendLine("Exception: ");
            builder.AppendLine(string.Format(command));

            _logger.Error(builder.ToString());
        }
    }
}