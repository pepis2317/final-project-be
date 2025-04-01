using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.Drawing.Imaging;
using System.Drawing;

namespace final_project_backend.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        public async Task<string?> GetTemporaryImageUrl(string? fileName, string containerName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (!(await blobClient.ExistsAsync()))
            {
                return null;
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            if (blobClient.CanGenerateSasUri)
            {
                var sasUrl = blobClient.GenerateSasUri(sasBuilder).ToString();
                return sasUrl;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> DeletePfpAsync(string fileName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
                return true;
            }
            return false;
        }
        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType, string containerName, int targetSize)
        {
            try
            {
                using var image = Image.FromStream(imageStream);

                int width = image.Width;
                int height = image.Height;

                int size = Math.Min(width, height);
                int x = (width - size) / 2;
                int y = (height - size) / 2;

                using var croppedImage = new Bitmap(size, size);
                using (var graphics = Graphics.FromImage(croppedImage))
                {
                    graphics.DrawImage(image, new Rectangle(0, 0, size, size), new Rectangle(x, y, size, size), GraphicsUnit.Pixel);
                }

                using var resizedImage = new Bitmap(200, 200);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(croppedImage, 0, 0, 200, 200);
                }

                using var memoryStream = new MemoryStream();
                resizedImage.Save(memoryStream, ImageFormat.Jpeg); 
                memoryStream.Position = 0;

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();
                var blobClient = containerClient.GetBlobClient(fileName);
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                };
                await blobClient.UploadAsync(memoryStream, blobHttpHeaders);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading image: {ex}");
            }
        }
    }
}
