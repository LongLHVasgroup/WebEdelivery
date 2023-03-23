using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class CustomerCungDuongMapping
    {
        public string CustomerCode { get; set; }
        public int CungDuongCode { get; set; }
        public bool? IsDefault { get; set; }
        public Guid Rowguid { get; set; }
    }
}
