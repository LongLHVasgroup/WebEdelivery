using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class SyncPotoSapmodel
    {
        public int Id { get; set; }
        public string ScaleTicketCode { get; set; }
        public string VehicleNumber { get; set; }
        public string BargeNumber { get; set; }
        public string ContainerCount { get; set; }
        public string TrailersNumber { get; set; }
        public string Ponumber { get; set; }
        public string Poline { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Poqty { get; set; }
        public decimal? Qty1 { get; set; }
        public decimal? Qty2 { get; set; }
        public string Unit2 { get; set; }
        public string WarehouseEntryCode { get; set; }
        public DateTime? InHour { get; set; }
        public decimal? TapChat { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public bool? IsSendToSapcompleted { get; set; }
        public bool? IsInvalidSap { get; set; }
        public string InvalidSapmessage { get; set; }
    }
}
