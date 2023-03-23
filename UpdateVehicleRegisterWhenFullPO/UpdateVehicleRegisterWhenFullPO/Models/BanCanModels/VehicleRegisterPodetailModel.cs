using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UpdateVehicleRegisterWhenFullPO.Models.BanCanModels
{
    public class VehicleRegisterPodetailModel
    {
        public Guid VehicleRegisterPodetailId { get; set; }
        public Guid? VehicleRegisterMobileId { get; set; }
        public string Ponumber { get; set; }
        public string Poline { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? TiLe { get; set; }
        public string Unit { get; set; }
        public decimal? TrongLuong { get; set; }
    }
}