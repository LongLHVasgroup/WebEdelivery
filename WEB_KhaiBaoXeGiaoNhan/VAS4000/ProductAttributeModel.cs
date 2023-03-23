using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class ProductAttributeModel
    {
        public Guid ProductAttributeId { get; set; }
        public string ProductCode { get; set; }
        public string CustomerCode { get; set; }
        public decimal? MinSingleWeight { get; set; }
        public decimal? MaxSingleWeight { get; set; }
    }
}
