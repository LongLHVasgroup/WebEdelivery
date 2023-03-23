using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class ProductConvertModel
    {
        public Guid ProductConvertId { get; set; }
        public string WarehouseCode { get; set; }
        public string ProductCodeFrom { get; set; }
        public string ProductCodeTo { get; set; }
    }
}
