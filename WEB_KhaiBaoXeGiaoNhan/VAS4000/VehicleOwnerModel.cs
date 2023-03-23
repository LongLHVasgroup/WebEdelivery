using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class VehicleOwnerModel
    {
        public string VehicleOwner { get; set; }
        public string VehicleOwnerName { get; set; }
        public string ProviderCode { get; set; }
        public string CustomerCode { get; set; }
        public bool? Actived { get; set; }
    }
}
