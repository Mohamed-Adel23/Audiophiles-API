using System.ComponentModel.DataAnnotations;

namespace Audiophiles_API.DTOs
{
    public class UserContactModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
