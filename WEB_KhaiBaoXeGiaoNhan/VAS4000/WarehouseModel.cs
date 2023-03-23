using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class WarehouseModel
    {
        public Guid WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastEditedTime { get; set; }
        public bool? IsSapdata { get; set; }
        public bool? Actived { get; set; }
    }
}
