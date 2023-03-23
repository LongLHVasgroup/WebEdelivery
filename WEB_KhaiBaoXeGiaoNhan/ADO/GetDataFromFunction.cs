using AdminPortal.DataLayer;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_KhaiBaoXeGiaoNhan.ADO
{
    public class GetDataFromFunction : DataProvider<GetDataFromFunction>
    {
        public decimal GetSLDaNhapTuPONumber(string poNumber = "", string productCode = "", string cnt = "")
        {
            using (var connection = new SqlConnection(cnt))
            {
                //Lê Hoàng Long
                //Config.getInstance().connPMC => cnt
                using (SqlConnection conn = new SqlConnection(cnt))
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.fnGetSLDaNhapTuPONumber(@ponumber,@productcode)", conn))
                {
                    AddSqlParameter(cmd, "@ponumber", poNumber, System.Data.SqlDbType.NVarChar);
                    AddSqlParameter(cmd, "@productcode", productCode, System.Data.SqlDbType.NVarChar);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    conn.Close();
                    return (decimal)result;
                }
            }
        }
    }
}