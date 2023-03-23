using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class SomasterModel
    {
        public string Sonumber { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal? QtyTotal { get; set; }
        public bool? IsNhapKhau { get; set; }
        public bool? IsCompelete { get; set; }
        public decimal? SoLuongDaXuat { get; set; }
    }
}
