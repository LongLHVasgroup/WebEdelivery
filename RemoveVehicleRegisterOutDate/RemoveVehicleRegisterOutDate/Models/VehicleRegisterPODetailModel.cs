using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate.Models
{
    public class VehicleRegisterPODetailModel
    {
        [Key]
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
