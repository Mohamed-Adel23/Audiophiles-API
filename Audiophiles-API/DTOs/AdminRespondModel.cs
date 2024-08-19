using System.ComponentModel.DataAnnotations;

namespace Audiophiles_API.DTOs
{
    public class AdminRespondModel
    {
        public int UserContactId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
