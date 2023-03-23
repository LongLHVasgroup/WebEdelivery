using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class Domodel
    {
        public string Donumber { get; set; }
        public string CompanyCode { get; set; }
        public decimal? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? SoCuonBo { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
