using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class ProductModel
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitCode { get; set; }
        public decimal? MinSingleWeight { get; set; }
        public decimal? MaxSingleWeight { get; set; }
        public bool? IsSapdata { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastEditedTime { get; set; }
        public bool? Actived { get; set; }
        public Guid Rowguid { get; set; }
        public bool? IsShowMobile { get; set; }
    }
}
