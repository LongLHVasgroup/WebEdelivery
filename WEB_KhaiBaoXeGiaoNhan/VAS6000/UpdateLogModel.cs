using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class UpdateLogModel
    {
        public Guid UpdateId { get; set; }
        public string Id { get; set; }
        public string CompanyCode { get; set; }
        public string Zkey { get; set; }
        public string FunctionName { get; set; }
        public string TimeSend { get; set; }
        public string TimeReceive { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public Guid Rowguid { get; set; }
    }
}
