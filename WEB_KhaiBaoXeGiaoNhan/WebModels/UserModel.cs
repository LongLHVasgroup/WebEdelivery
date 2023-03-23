using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class UserModel
    {
        public Guid Userid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? Memberof { get; set; }
        public bool? Active { get; set; }
        public string Token { get; set; }
        public int? Rolecode { get; set; }
        public string UserType { get; set; }
        public string Taxcode { get; set; }
        public bool? IsService { get; set; }
        public bool? IsVeTinh { get; set; }
        public string CompanyCode { get; set; }
    }
}
