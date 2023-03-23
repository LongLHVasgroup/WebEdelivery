using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class LogsTest
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Function { get; set; }
        public Guid Rowguid { get; set; }
    }
}
