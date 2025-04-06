namespace final_project_backend.Models.Shop
{
    public class ShopModel
    {
        public required Guid ShopId { get; set; }
        public  string ShopName { get; set; }
        public  Guid OwnerId { get; set; }
        public string? Description { get; set; }
        public  decimal Rating { get; set; }
        public  string? Address {  get; set; }
        public  DateTime? CreatedAt { get; set; }
    }
}
