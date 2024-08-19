using System.ComponentModel.DataAnnotations;

namespace Audiophiles_API.DTOs.Auth
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
