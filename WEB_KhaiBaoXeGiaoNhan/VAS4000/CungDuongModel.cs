using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class CungDuongModel
    {
        public int CungDuongCode { get; set; }
        public string CungDuongName { get; set; }
        public decimal? KhoangCach { get; set; }
        public string Plant { get; set; }
    }
}
