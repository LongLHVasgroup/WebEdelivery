using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate.Models
{
    public class VehicleRegisterMobileModel
    {
        [Key]
        public Guid VehicleRegisterMobileId { get; set; }
        public Guid? UserRegisterId { get; set; }
        public DateTime? RegisterTime { get; set; }
        public DateTime? ThoiGianToiDuKien { get; set; }
        public DateTime? ThoiGianToiThucTe { get; set; }
        public string Dvvccode { get; set; }
        public string Dvvc { get; set; }
        public string SoDonHang { get; set; }
        public string GiaoNhan { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverIdCard { get; set; }
        public decimal? TrongLuongGiaoDuKien { get; set; }
        public decimal? TrongLuongGiaoThucTe { get; set; }
        public decimal? TapChat { get; set; }
        public int? CungDuongCode { get; set; }
        public string CungDuongName { get; set; }
        public string Assets { get; set; }
        public string Note { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ScaleTicketCode { get; set; }
        public bool? AllowEdit { get; set; }
        public Guid? UserActiveId { get; set; }
        public bool? IsActive { get; set; }
    }
}
