using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UpdateVehicleRegisterWhenFullPO.Models.WebModels
{
    public partial class PomasterModel
    {
        [Key]
        public string Ponumber { get; set; }

        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public decimal? QtyTotal { get; set; }
        public bool? IsNhapKhau { get; set; }
        public bool? IsCompelete { get; set; }
        public string Note { get; set; }
        public decimal? SoLuongDaNhap { get; set; }
        public string CompanyCode { get; set; }
    }
}