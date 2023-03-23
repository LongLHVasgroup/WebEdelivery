using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class ScaleTicketSomodel
    {
        public Guid ScaleTicketSoid { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Sonumber { get; set; }
        public string SonumberOrther { get; set; }
        public string Donumber { get; set; }
        public string DonumberOther { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string WarehouseExport { get; set; }
        public string WarehouseExportCode { get; set; }
        public string SoPhieuKho { get; set; }
        public decimal? TrongLuongOd { get; set; }
        public decimal? ChenhLech { get; set; }
    }
}
