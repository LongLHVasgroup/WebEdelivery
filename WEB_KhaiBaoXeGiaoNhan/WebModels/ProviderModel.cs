﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class ProviderModel
    {
        public Guid ProviderId { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public string Address { get; set; }
        public string AccountGroup { get; set; }
        public bool? IsSapdata { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastEditedTime { get; set; }
        public bool? Actived { get; set; }
    }
}
