using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS6000
{
    public partial class CheckIpPoSoModel
    {
        public Guid Id { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string Ponumber { get; set; }
        public string Sonumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string UserCheck { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TmpNote { get; set; }
        public DateTime? TmpDate { get; set; }
        public bool? TmpBool { get; set; }
        public Guid Rowguid { get; set; }
        public string UserApprove { get; set; }
        public DateTime? ApproveDate { get; set; }
        public bool? IsApprove { get; set; }
        public bool? Ipstatus { get; set; }
    }
}
