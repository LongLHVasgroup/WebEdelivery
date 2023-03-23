using System.ComponentModel.DataAnnotations;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Fullname { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Owner { get; set; }

        public bool IsCustomer { get; set; }
        public int? Role { get; set; }
    }

    public class UpdateModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdateInfoModel
    {
        private string fullName;
        private string phone;
        private string email;

        public string FullName { get => fullName; set => fullName = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
    }
}