using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class AutoNumber
    {
        public string Code { get; set; }
        public int? CurrentNumber { get; set; }
        public Guid Rowguid { get; set; }
    }
}
