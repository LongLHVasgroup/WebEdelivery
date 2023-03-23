using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class BargeModel
    {
        public Guid BargeId { get; set; }
        public int? Type { get; set; }
        public string BargeNumber { get; set; }
        public string BargeOwner { get; set; }
    }
}
