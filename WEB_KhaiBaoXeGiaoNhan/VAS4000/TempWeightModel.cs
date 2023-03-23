using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class TempWeightModel
    {
        public Guid Id { get; set; }
        public Guid? ScaleticketId { get; set; }
        public decimal? Weight { get; set; }
        public string OrderNumner { get; set; }
        public bool? Status { get; set; }
    }
}
