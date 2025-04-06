using Entities;
using final_project_backend.Models.Item;
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
        public async Task<IActionResult> UploadItemImage(UploadItemImageRequest request)
        {
            var fileName = $"{Guid.NewGuid()}_{request.file.FileName}";
            var contentType = request.file.ContentType;
            using var stream = request.file.OpenReadStream();
            var blobUrl = await _service.UploadItemImage(request.ItemId, stream, fileName, contentType);
            return Ok(new { blobUrl });
        }
    }
}
