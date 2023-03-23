using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class CheckingScrapModel
    {
        public Guid CheckingScrapId { get; set; }
        public int ChekingScrapIdInt { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public Guid? ScaleTicketMobileId { get; set; }
        public string Rfid { get; set; }
        public DateTime? InHourGuard { get; set; }
        public DateTime? OutHourGuard { get; set; }
        public string GiaoNhan { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverIdCard { get; set; }
        public bool IsVehicleNew { get; set; }
        public string InGateId { get; set; }
        public string ReceiveType { get; set; }
        public Guid? User1Id { get; set; }
        public string Note1 { get; set; }
        public Guid? User2Id { get; set; }
        public Guid? User3Id { get; set; }
        public string Note3 { get; set; }
        public DateTime? CheckingTime { get; set; }
        public Guid? User4Id { get; set; }
        public DateTime? VerifyTime { get; set; }
        public int? Status { get; set; }
        public bool? Actived { get; set; }
        public bool? IsDone { get; set; }
        public string OutGateId { get; set; }
        public Guid? User5Id { get; set; }
        public int? Step { get; set; }
        public bool? IsLockCard { get; set; }
        public DateTime? StartCheckingTime { get; set; }
        public Guid? VehicleRegisterMobileId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderCode { get; set; }
        public string Romooc { get; set; }
    }
}
