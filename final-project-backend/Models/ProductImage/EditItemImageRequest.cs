namespace final_project_backend.Models.ProductImage
{
    public class EditItemImageRequest
    {
        public required Guid ItemId { get; set; }
        public required IFormFile File { get; set; }
    }
}
