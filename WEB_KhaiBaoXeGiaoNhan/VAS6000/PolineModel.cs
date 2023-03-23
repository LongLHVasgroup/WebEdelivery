using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class PolineModel
    {
        public string Ponumber { get; set; }
        public string Poline { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public string CompanyCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? OverTolerance { get; set; }
        public decimal? UnderTolerance { get; set; }
        public bool? IsUnlimited { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool? IsRelease { get; set; }
        public bool? IsDeliveryCompleted { get; set; }
        public decimal? SoLuongDaNhap { get; set; }
        public bool? IsPmccompleted { get; set; }
        public Guid Rowguid { get; set; }
        public string WarehouseCode { get; set; }
    }
}
