using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSyncPoToWeb.Models
{
    public partial class POLineModel
    {
        public string PONumber { get; set; }
        public string POLine { get; set; }
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
        public string WarehouseCode { get; set; }
    }
}
