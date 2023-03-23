using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public sealed class ConnectionString
    {
        public string server;
        public string db;
        public string username;
        public string password;
        public string connectionString;
        private static ConnectionString Instance = null;

        public static ConnectionString getInstance()
        {
            if (Instance == null)
            {
                Instance = new ConnectionString();
            }
            return Instance;
        }

        private ConnectionString()
        {
        }

        //public string getConnectionString()
        //{
        //    return string.Format("data source={0};" +
        //        "initial catalog={1};" +
        //        "user id={2};" +
        //        "password={3};" +
        //        "MultipleActiveResultSets=True;", server, db, username, password);
        //}
        public string getConnectionString()
        {
            return connectionString;
        }
    }
}