using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class VehicleModel
    {
        public Guid VehicleId { get; set; }
        public int? Type { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleOwner { get; set; }
        public Guid? DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverCardNo { get; set; }
        public decimal VehicleWeight { get; set; }
        public int? IsRoMooc { get; set; }
        public decimal? TrongLuongDangKiem { get; set; }
        public decimal? TyLeVuot { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsLockEdit { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastEditUserId { get; set; }
        public DateTime? LastEditTime { get; set; }
        public decimal? TempWeight { get; set; }
        public DateTime? UpdateTempWeightTime { get; set; }
        public bool? IsContainer { get; set; }
        public bool? IsDauKeo { get; set; }
    }
}
