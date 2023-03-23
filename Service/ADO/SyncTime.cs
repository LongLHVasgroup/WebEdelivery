using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADO
{
    public class SyncTime : DataProvider<SyncTime>
    {
        public class LastRunModel
        {
            private Guid id;
            private DateTime lastRun;
            private string name;
            public Guid Id { get => id; set => id = value; }
            public DateTime LastRun { get => lastRun; set => lastRun = value; }
            public string Name { get => name; set => name = value; }
        }

        public LastRunModel GetLastRun(SqlConnection connection)
        {
            var result = new LastRunModel();
            using (var command = new SqlCommand("SELECT [ID],[Name], [LastRun] FROM [dbo].[Z_TestTableModel] " +
                "WHERE [Name] = 'VehicleRegisterMobileModel' " +
                "ORDER BY [LastRun] DESC", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Id = GetDbReaderValue<Guid>(reader["Id"]);
                        result.Name = GetDbReaderValue<string>(reader["Name"]);
                        result.LastRun = GetDbReaderValue<DateTime>(reader["LastRun"]);
                    }
                }
                return result;
            }
        }

        public void UpdateLastRun(SqlConnection connection, LastRunModel item)
        {
            using (var command = new SqlCommand("UPDATE [Z_TestTableModel] " +
                " SET [LastRun] = GETDATE() " +
                " WHERE ID = @ID", connection))
            {
                AddSqlParameter(command, "@ID", item.Id, System.Data.SqlDbType.UniqueIdentifier);
                var reader = command.ExecuteNonQuery();
            }
        }
    }
}