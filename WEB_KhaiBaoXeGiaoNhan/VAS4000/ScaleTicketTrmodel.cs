using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class ScaleTicketTrmodel
    {
        public Guid ScaleTicketTrid { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string WarehouseEntry { get; set; }
        public string WarehouseEntryCode { get; set; }
        public string WarehouseExport { get; set; }
        public string WarehouseExportCode { get; set; }
        public decimal? Qty1 { get; set; }
        public string Unit1 { get; set; }
        public decimal? Qty2 { get; set; }
        public string Unit2 { get; set; }
    }
}
