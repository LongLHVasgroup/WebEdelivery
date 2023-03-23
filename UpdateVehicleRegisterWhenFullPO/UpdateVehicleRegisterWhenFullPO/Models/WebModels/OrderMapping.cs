using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UpdateVehicleRegisterWhenFullPO.Models.WebModels
{
    public class OrderMapping
    {
        [Key]
        public Guid MappingId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid MasterId { get; set; }
        public string OrderNumber { get; set; }
        public decimal? SoLuong { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public int? CungDuongCode { get; set; }
        public decimal? SoLuongCont { get; set; }
        public bool? IsCont { get; set; }
        public string BillNumber { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string ShipNumber { get; set; }
        public bool? IsDone { get; set; }
    }
}
