using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class SolineModel
    {
        public string Soline { get; set; }
        public string Sonumber { get; set; }
        public string CompanyCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? OverTolerance { get; set; }
        public decimal? UnderTolerance { get; set; }
        public bool? IsUnlimited { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Ponumber { get; set; }
        public bool? IsComplete { get; set; }
        public decimal? SoLuongDaXuat { get; set; }
        public Guid Rowguid { get; set; }
        public bool? IsClosed { get; set; }
    }
}
