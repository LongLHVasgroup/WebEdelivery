using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class MappingModel
    {
        public MappingModel()
        {
            Services = new List<SubMappingModel>();
        }

        public List<SubMappingModel> Services { get; set; }

        [Required]
        public Guid MasterID { get; set; }

        public string OrderNumber { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BillNumber { get; set; }
        public string ShipNumber { get; set; }
        public Boolean? IsCont { get; set; }
    }

    public class SubMappingModel
    {
        public SubMappingModel()
        {
            ServicesID = new Guid();
        }

        [Required]
        public Guid ServicesID { get; set; }

        public decimal? Quantity { get; set; }
        public decimal? SoLuongCont { get; set; }
        public int CungDuongCode { get; set; }
    }
}