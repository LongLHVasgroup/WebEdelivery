using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class ScaleTicketMobileHistoryModel
    {
        public Guid ScaleTicketMobileHistoryId { get; set; }
        public Guid? ScaleTicketId { get; set; }
        public string HistoryNote { get; set; }
        public string HistoryType { get; set; }
        public string HistoryValue { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateAccountId { get; set; }
    }
}
