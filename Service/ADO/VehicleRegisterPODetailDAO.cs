using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Service.ADO
{
    public class VehicleRegisterPODetailDAO : DataProvider<VehicleRegisterPODetailDAO>
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

        public List<VehicleRegisterPODetailModel> GetVehicleRegisDetailList(SqlConnection connection, Guid lastRunId)
        {
            var result = new List<VehicleRegisterPODetailModel>();
            using (var command = new SqlCommand("SELECT [VehicleRegisterPODetailId] " +
                ",[VehicleRegisterMobileId] " +
                ",[PONumber] " +
                ",[POLine] " +
                ",[ProductCode] " +
                ",[ProductName] " +
                ",[TiLe] " +
                ",[Unit] " +
                ",[TrongLuong] " +
                " FROM [VehicleRegisterPODetailModel] " +
                "WHERE [VehicleRegisterMobileId] = @lastrunid", connection))
            {
                AddSqlParameter(command, "@lastrunid", lastRunId, System.Data.SqlDbType.UniqueIdentifier);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new VehicleRegisterPODetailModel
                        {
                            VehicleRegisterPODetailId = GetDbReaderValue<Guid>(reader["VehicleRegisterPODetailId"]),
                            VehicleRegisterMobileId = GetDbReaderValue<Guid>(reader["VehicleRegisterMobileId"]),
                            POLine = GetDbReaderValue<string>(reader["POLine"]),
                            PONumber = GetDbReaderValue<string>(reader["PONumber"]),
                            ProductCode = GetDbReaderValue<string>(reader["ProductCode"]),
                            ProductName = GetDbReaderValue<string>(reader["ProductName"]),
                            TiLe = GetDbReaderValue<decimal>(reader["TiLe"]),
                            TrongLuong = GetDbReaderValue<decimal>(reader["TrongLuong"]),
                            Unit = GetDbReaderValue<string>(reader["Unit"])
                        };
                        result.Add(item);
                    }
                }
                return result;
            }
        }
    }
}