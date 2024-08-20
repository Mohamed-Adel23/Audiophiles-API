namespace Audiophiles_API.Models
{
    public class UserContact
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime ContactAt { get; set; }
    }
}
