using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class TestModel
    {
        public Guid TestId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Note { get; set; }
    }
}
