namespace final_project_backend.Models.Item
{
    public class UploadItemImageRequest
    {
        public required Guid ItemId { get; set; }
        public required IFormFile file { get; set; }
    }
}
