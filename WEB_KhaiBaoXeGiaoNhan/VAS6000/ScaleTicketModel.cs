using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class ScaleTicketModel
    {
        public Guid ScaleTicketId { get; set; }
        public string ScaleTicketCode { get; set; }
        public string ScaleTicketTypeCode { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleNumber { get; set; }
        public string BargeNumber { get; set; }
        public string ContainerCount { get; set; }
        public string TrailersNumber { get; set; }
        public DateTime? InHour { get; set; }
        public DateTime? OutHour { get; set; }
        public decimal? FirstWeight { get; set; }
        public decimal? SecondWeight { get; set; }
        public decimal? ActualWeight { get; set; }
        public decimal? PercentReduced { get; set; }
        public decimal? KgReduced { get; set; }
        public decimal? ActualWeightAfterReduction { get; set; }
        public decimal? TotalReduced { get; set; }
        public string Description { get; set; }
        public bool? IsTest { get; set; }
        public string SoftCode { get; set; }
        public Guid? FirstUserId { get; set; }
        public Guid? SecondUserId { get; set; }
        public bool? IsSendToSapcompleted { get; set; }
        public bool? IsInvalidSap { get; set; }
        public string InvalidSapmessage { get; set; }
        public bool? IsXeNoiBo { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsQuaTai { get; set; }
        public bool? StatusIp { get; set; }
        public bool? IsDonTrongMin { get; set; }
        public bool? IsDonTrongMax { get; set; }
        public bool? IsVuotTyLeOd { get; set; }
        public int? CungDuongCode { get; set; }
        public string CungDuongName { get; set; }
        public string VehicleOwner { get; set; }
        public string VehicleOwnerName { get; set; }
        public Guid Rowguid { get; set; }
        public bool? Is20Feet { get; set; }
        public bool? Is40Feet { get; set; }
        public string SoHieuCont1 { get; set; }
        public string SoHieuCont2 { get; set; }
    }
}
