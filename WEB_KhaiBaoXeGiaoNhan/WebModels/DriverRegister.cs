using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class DriverRegister
    {
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public Guid Creator { get; set; }
        public string DriverCardNo { get; set; }
        public bool? Active { get; set; }
        public Guid OwnerId { get; set; }
    }
}
