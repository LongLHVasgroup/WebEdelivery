using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADO
{
    public class VehicleRegisterMobileDAO : DataProvider<VehicleRegisterMobileDAO>
    {
        public int InsertOne(SqlConnection connection, VehicleRegisterMobileModel user)
        {
            int result = 0;
            using (var command = new SqlCommand("INSERT INTO [VehicleRegisterMobileModel]" +
                "([VehicleRegisterMobileId]," +
                "[RegisterTime]," +
                "[ThoiGianToiDuKien]," +
                "[ThoiGianToiThucTe]," +
                "[DVVC]," +
                "[SoDonHang]," +
                "[GiaoNhan]," +
                "[VehicleNumber]," +
                "[DriverName]," +
                "[DriverIdCard]," +
                "[TrongLuongGiaoDuKien]," +
                "[TrongLuongGiaoThucTe]," +
                "[CungDuongCode]," +
                "[CungDuongName]," +
                "[Assets]," +
                "[Note]," +
                "[ModifyTime]," +
                "[AllowEdit])" +
                " VALUES (" +
                " @VehicleRegisterMobileId," +
                "@RegisterTime," +
                "@ThoiGianToiDuKien," +
                "@ThoiGianToiThucTe," +
                "@DVVC," +
                "@SoDonHang," +
                "@GiaoNhan," +
                "@VehicleNumber," +
                "@DriverName," +
                "@DriverIdCard," +
                "@TrongLuongGiaoDuKien," +
                "@TrongLuongGiaoThucTe," +
                "@CungDuongCode," +
                "@CungDuongName," +
                "@Assets," +
                "@Note," +
                "@ModifyTime," +
                "@AllowEdit)"
                 , connection))
            {
                AddSqlParameter(command, "@VehicleRegisterMobileId", user.VehicleRegisterMobileId, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@RegisterTime", user.RegisterTime, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@ThoiGianToiDuKien", user.ThoiGianToiDuKien, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@ThoiGianToiThucTe", user.ThoiGianToiThucTe, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@DVVC", user.DVVC, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@SoDonHang", user.SoDonHang, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@GiaoNhan", user.GiaoNhan, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@VehicleNumber", user.VehicleNumber, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@DriverName", user.DriverName, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@DriverIdCard", user.DriverIdCard, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@TrongLuongGiaoDuKien", user.TrongLuongGiaoDuKien, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@TrongLuongGiaoThucTe", user.TrongLuongGiaoThucTe, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@CungDuongCode", user.CungDuongCode, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@CungDuongName", user.CungDuongName, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@Assets", user.Assets, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@Note", user.Note, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@ModifyTime", user.Note, System.Data.SqlDbType.UniqueIdentifier);
                AddSqlParameter(command, "@AllowEdit", user.AllowEdit, System.Data.SqlDbType.UniqueIdentifier);

                WriteLogExecutingCommand(command);
                result = command.ExecuteNonQuery();
            }
            return result;
        }

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