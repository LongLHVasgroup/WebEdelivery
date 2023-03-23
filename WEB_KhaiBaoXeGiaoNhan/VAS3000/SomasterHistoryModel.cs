using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class SomasterHistoryModel
    {
        public Guid Id { get; set; }
        public string Sonumber { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
