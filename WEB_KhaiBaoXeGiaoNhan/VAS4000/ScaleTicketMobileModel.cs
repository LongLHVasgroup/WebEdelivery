using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class ScaleTicketMobileModel
    {
        public Guid ScaleTicketMobileId { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string ScaleTicketCode { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleNumber { get; set; }
        public string BargeNumber { get; set; }
        public string ContainerCount { get; set; }
        public bool? Is20Feet { get; set; }
        public bool? Is40Feet { get; set; }
        public string SoHieuCont1 { get; set; }
        public string SoHieuCont2 { get; set; }
        public string TrailersNumber { get; set; }
        public decimal? PercentReduced { get; set; }
        public decimal? KgReduced { get; set; }
        public string Description { get; set; }
        public bool? IsXeNoiBo { get; set; }
    }
}
