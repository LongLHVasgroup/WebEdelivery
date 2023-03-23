using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class ScaleTicketSodetailModel
    {
        public Guid ScaleTicketSodetailId { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string Sonumber { get; set; }
        public string Soline { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Soqty { get; set; }
        public string Unit { get; set; }
        public decimal? SoLuongDaXuat { get; set; }
        public decimal? Qty1 { get; set; }
        public string Unit1 { get; set; }
        public decimal? Qty2 { get; set; }
        public string Unit2 { get; set; }
        public decimal? MinSingleWeight { get; set; }
        public decimal? MaxSingleWeight { get; set; }
        public decimal? ActualSingleWeight { get; set; }
        public bool? IsNoSo { get; set; }
        public Guid Rowguid { get; set; }
    }
}
