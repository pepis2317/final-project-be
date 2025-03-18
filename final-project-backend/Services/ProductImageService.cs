using Entities;
using final_project_backend.Models.ProductImage;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Services
{
    public class ProductImageService
    {
        private readonly FinalProjectTrainingDbContext _context;
        private readonly BlobStorageService _blobStorageService;
        public ProductImageService(FinalProjectTrainingDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }
        public async Task<List<ProductImageResponse>> GetImagesForItem(Guid itemId)
        {
            var images = await _context.ProductImages.Where(q => q.ItemId == itemId).OrderBy(q=>q.IsPrimary).ToListAsync();
            var imageNames = images.Select(i => i.Image).ToList();
            var imageUrls = await Task.WhenAll(imageNames.Select(ImageHelper));
            var result = images.Select((image, index) => new ProductImageResponse
            {
                ImageId = image.ImageId,
                ItemId = image.ItemId,
                Image = imageUrls[index],
                IsPrimary = image.IsPrimary
            }).ToList();
            return result;
        }
        public async Task<string?> GetThumbnail(Guid ItemId)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(q => q.ItemId == ItemId && q.IsPrimary == "true");
            if (image == null)
            {
                return null;
            }
            var thumbnail = await ImageHelper(image.Image);
            return thumbnail;
        }
        public async Task<string?> UploadItemImage(Guid ItemId, Stream imageStream, string fileName, string contentType)
        {
            var check = await _context.ProductImages.FirstOrDefaultAsync(q => q.ItemId == ItemId);
            string isPrimary = "true";
            if (check != null)
            {
                isPrimary = "false";
            }
            string imageUrl = await _blobStorageService.UploadImageAsync(imageStream, fileName, contentType, "item-images", 1080);
            var productImage = new ProductImage
            {
                ItemId = ItemId,
                Image = fileName,
                IsPrimary = isPrimary,
            };
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return imageUrl;
        }
        private async Task<string?> ImageHelper(string? fileName)
        {
            string? imageUrl = await _blobStorageService.GetTemporaryImageUrl(fileName, "item-images");
            return imageUrl;
        }
    }
}
