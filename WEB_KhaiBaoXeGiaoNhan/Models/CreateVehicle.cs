using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class CreateVehicle
    {
        public string VehicleNumber { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? RomoocId { get; set; }
        public decimal VehicleWeight { get; set; }
        public int? IsRoMooc { get; set; }
        public decimal? TrongLuongDangKiem { get; set; }
        public bool? IsDauKeo { get; set; }
    }
}
