using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class PogiaKhacMapping
    {
        public Guid Id { get; set; }
        public string PoNumber { get; set; }
        public bool? IsGiaKhac { get; set; }
        public Guid? Creator { get; set; }
        public Guid? Modifier { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? Deleted { get; set; }
    }
}
