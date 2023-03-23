using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADO
{
    public class ScaleTicketDAO : DataProvider<ScaleTicketDAO>
    {
        public List<VehicleRegisterMobileModel> GetVehicleRegisList(SqlConnection connection, DateTime lastRun)
        {
            var result = new List<VehicleRegisterMobileModel>();
            using (var command = new SqlCommand("SELECT [VehicleRegisterMobileId]" +
                ",[RegisterTime]" +
                ",[ThoiGianToiDuKien]" +
                ",[ThoiGianToiThucTe]" +
                ",[DVVCCode]" +
                ",[DVVC]" +
                ",[SoDonHang]" +
                ",[GiaoNhan]" +
                ",[VehicleNumber]" +
                ",[DriverName]" +
                ",[DriverIdCard]" +
                ",[TrongLuongGiaoDuKien]" +
                ",[TrongLuongGiaoThucTe]" +
                ",[TapChat]" +
                ",[CungDuongCode]" +
                ",[CungDuongName]" +
                ",[Assets]" +
                ",[Note]" +
                ",[ModifyTime]" +
                ",[ScaleTicketCode]" +
                ",[AllowEdit]" +
                " FROM [dbo].[VehicleRegisterMobileModel] " +
                " WHERE [ModifyTime] > @lastrun ", connection))
            {
                AddSqlParameter(command, "@lastrun", lastRun, System.Data.SqlDbType.DateTime);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new VehicleRegisterMobileModel
                        {
                            VehicleRegisterMobileId = GetDbReaderValue<Guid>(reader["VehicleRegisterMobileId"]),
                            RegisterTime = GetDbReaderValue<DateTime>(reader["RegisterTime"]),
                            ThoiGianToiDuKien = GetDbReaderValue<DateTime>(reader["ThoiGianToiDuKien"]),
                            ThoiGianToiThucTe = GetDbReaderValue<DateTime>(reader["ThoiGianToiThucTe"]),
                            DVVCCode = GetDbReaderValue<string>(reader["DVVCCode"]),
                            DVVC = GetDbReaderValue<string>(reader["DVVC"]),
                            SoDonHang = GetDbReaderValue<string>(reader["SoDonHang"]),
                            GiaoNhan = GetDbReaderValue<string>(reader["GiaoNhan"]),
                            VehicleNumber = GetDbReaderValue<string>(reader["VehicleNumber"]),
                            DriverName = GetDbReaderValue<string>(reader["DriverName"]),
                            DriverIdCard = GetDbReaderValue<string>(reader["DriverIdCard"]),
                            TrongLuongGiaoDuKien = GetDbReaderValue<decimal>(reader["TrongLuongGiaoDuKien"]),
                            TrongLuongGiaoThucTe = GetDbReaderValue<decimal>(reader["TrongLuongGiaoThucTe"]),
                            TapChat = GetDbReaderValue<decimal>(reader["TapChat"]),
                            CungDuongCode = GetDbReaderValue<int>(reader["CungDuongCode"]),
                            CungDuongName = GetDbReaderValue<string>(reader["CungDuongName"]),
                            Assets = GetDbReaderValue<string>(reader["Assets"]),
                            Note = GetDbReaderValue<string>(reader["Note"]),
                            ModifyTime = GetDbReaderValue<DateTime>(reader["ModifyTime"]),
                            ScaleTicketCode = GetDbReaderValue<string>(reader["ScaleTicketCode"]),
                            AllowEdit = GetDbReaderValue<bool>(reader["AllowEdit"])
                        };
                        result.Add(item);
                    }
                }
                return result;
            }
        }
    }
}