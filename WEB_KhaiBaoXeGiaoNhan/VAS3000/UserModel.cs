using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.VAS3000
{
    public partial class UserModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PasswordEnscrypt { get; set; }
        public string RoldCode { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastEditedTime { get; set; }
        public bool? Actived { get; set; }
        public string DeviceToken { get; set; }
        public string GroupUser { get; set; }
    }
}
