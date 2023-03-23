using System.ComponentModel.DataAnnotations;
namespace Models
{
        public class AuthenticateModel
        {
            [Required]
            public string userName { get; set; }

            [Required]
            public string password { get; set; }
        }
}
