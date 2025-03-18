namespace final_project_backend.Models.ProductImage
{
    public class ProductImageResponse
    {
        public required Guid ImageId {get; set;}
        public required Guid ItemId {get; set;}
        public required string? Image { get;set;}
        public required string IsPrimary { get; set;}
    }
}
