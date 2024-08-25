using Audiophiles_API.DTOs;
using Audiophiles_API.IServices.IFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Audiophiles_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ProductImagesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("product", Name = "product")]
        public async Task<IActionResult> UploadProductImage(ProductImageModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Invoke File Service
            var result = await _fileService.UploadFileToServerAsync(model.Image, false, "Product");
            if(!string.IsNullOrEmpty(result.Message))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("gallery", Name = "gallery")]
        public async Task<IActionResult> UploadProductImages(GalleryImagesModel model)
        {
            var results = new List<UploadFile>();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (var image in model.Images)
            {
                // Invoke File Service
                var result = await _fileService.UploadFileToServerAsync(image, false, "Gallery");
                if (!string.IsNullOrEmpty(result.Message))
                    return BadRequest(result);
                results.Add(result);
            }

            return Ok(results);
        }
    }
}
