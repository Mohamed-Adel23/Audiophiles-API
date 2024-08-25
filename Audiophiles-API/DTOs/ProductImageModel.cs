using System.ComponentModel.DataAnnotations;

namespace Audiophiles_API.DTOs
{
    public class ProductImageModel
    {
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}
