using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class ScaleTicketPodetailModel
    {
        public Guid ScaleTicketPodetailId { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string Ponumber { get; set; }
        public string Poline { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Poqty { get; set; }
        public string Unit { get; set; }
        public decimal? SoLuongDaNhap { get; set; }
        public decimal? TyLeTrongLuong { get; set; }
        public decimal? Qty1 { get; set; }
        public decimal? TapChat { get; set; }
        public string Unit1 { get; set; }
        public decimal? Qty2 { get; set; }
        public string Unit2 { get; set; }
        public bool? IsNoPo { get; set; }
        public bool? IsSendToSapcompleted { get; set; }
    }
}
