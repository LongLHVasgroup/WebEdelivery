using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class ScaleTicketPomodel
    {
        public Guid ScaleTicketPoid { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public string Ponumber { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string WarehouseEntry { get; set; }
        public string WarehouseEntryCode { get; set; }
        public bool? IsHasPo { get; set; }
        public decimal? SoLuongYcconLaiPo { get; set; }
    }
}
