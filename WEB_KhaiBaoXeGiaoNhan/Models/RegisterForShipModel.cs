using System;
using System.Collections.Generic;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class RegisterForShipModel
    {
        public string OrderNumber { get; set; }
        public string ProductCode { get; set; }
        public DateTime StartDate { get; set; }
        public List<VehicleForShip> ListVehicle { get; set; }
    }

    public class VehicleForShip
    {
        public int? Id { get; set; }
        public string VehicleNumber { get; set; }
        public Guid DriverID1 { get; set; }
        public Guid DriverID2 { get; set; }
        public string Romooc { get; set; }
    }
}
