using System.ComponentModel.DataAnnotations;

namespace Audiophiles_API.DTOs
{
    public class GalleryImagesModel
    {
        [Required]
        public IFormFileCollection Images {  get; set; }
    }
}
