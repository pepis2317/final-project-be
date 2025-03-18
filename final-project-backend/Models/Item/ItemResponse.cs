namespace final_project_backend.Models.Item
{
    public class ItemResponse
    {

        public Guid ItemId { get; set; }
        public Guid ShopId { get; set; }
        public string ItemName { get; set; } = null!;
        public string? ItemDesc { get; set; }
        public int? Quantity { get; set; }
        public int? TotalHarga { get; set; }
    }
}
