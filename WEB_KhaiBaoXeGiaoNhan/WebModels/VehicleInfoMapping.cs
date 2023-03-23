﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class VehicleInfoMapping
    {
        public Guid VehicleId { get; set; }
        public string VehicleOwner { get; set; }
        public Guid? RomoocId { get; set; }
        public Guid? DriverId { get; set; }
    }
}
