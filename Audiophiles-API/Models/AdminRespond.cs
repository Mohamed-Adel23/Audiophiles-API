using System.ComponentModel.DataAnnotations.Schema;

namespace Audiophiles_API.Models
{
    public class AdminRespond
    {
        public int Id { get; set; }
        public int UserContactId { get; set; }
        // Each User Contact Has One Respond
        public UserContact UserContact { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public DateTime RespondAt { get; set; }
    }
}
