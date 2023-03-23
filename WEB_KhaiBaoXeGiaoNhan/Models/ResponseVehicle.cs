using System;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class ResponseVehicle
    {
        public Guid VehicleId { get; set; }
        public int? Type { get; set; }
        public string VehicleNumber { get; set; }
        public Guid? DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverCardNo { get; set; }
        public Guid? RomoocId { get; set; }
        public string RomoocNumber { get; set; }
        public decimal VehicleWeight { get; set; }
        public int? IsRoMooc { get; set; }
        public decimal? TrongLuongDangKiem { get; set; }
        public decimal? TyLeVuot { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsLockEdit { get; set; }
        public bool? IsContainer { get; set; }
        public bool? IsDauKeo { get; set; }
    }
}