using Entities;
using final_project_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly ProductImageService _service;
        public ProductImagesController(ProductImageService service)
        {
            _service = service;
        }
        [HttpGet("get-images-for-item")]
        public async Task<IActionResult> GetImagesForItem(Guid ItemId)
        {
            var data = await _service.GetImagesForItem(ItemId);
            return Ok(data);
        }
        [HttpPost("upload-image-for-item")]
        public async Task<IActionResult> UploadItemImage(Guid ItemId, IFormFile File)
        {
            var fileName = $"{Guid.NewGuid()}_{File.FileName}";
            var contentType = File.ContentType;
            using var stream = File.OpenReadStream();
            var imageUrl = await _service.UploadItemImage(ItemId, stream, fileName, contentType);
            return Ok(new { imageUrl });
        }
    }
}
