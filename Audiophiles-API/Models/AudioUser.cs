using Microsoft.AspNetCore.Identity;

namespace Audiophiles_API.Models
{
    public class AudioUser: IdentityUser
    {
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
