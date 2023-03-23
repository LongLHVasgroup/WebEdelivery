using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSyncPoToWeb.Models
{
    public partial class POMasterModel
    {
        public string PONumber { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public decimal? QtyTotal { get; set; }
        public bool? IsNhapKhau { get; set; }
        public bool? IsCompelete { get; set; }
        public string Note { get; set; }
        public decimal? SoLuongDaNhap { get; set; }
        public string CompanyCode { get; set; }
    }
}
