using Common;
using System.Data;
using System.Data.SqlClient;

namespace ApiTest.Common
{
    public class ConnectionFactory
    {
        private static SqlConnection cnn;

        /// <summary>
        /// Hàm tạo chuổi kết nối cơ sở dữ liệu SQL Server
        /// </summary>
        /// <returns>OdbcConnection trả về</returns>
        public SqlConnection GetConnection()
        {
            ConnectionString instance = ConnectionString.getInstance();
            string conn = instance.getConnectionString();
            cnn = new SqlConnection(conn);
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
            cnn.Open();
            return cnn;
        }
    }
}