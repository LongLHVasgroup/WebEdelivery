using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class CustomerModel
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public bool? IsSapdata { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastEditedTime { get; set; }
        public bool? Actived { get; set; }
        public int? CungDuongCode { get; set; }
        public string CungDuongName { get; set; }
    }
}
