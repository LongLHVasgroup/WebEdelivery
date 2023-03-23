namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class UserResponseModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Taxcode { get; set; }
        public bool? IsService { get; set; }
    }
}