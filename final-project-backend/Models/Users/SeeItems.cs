namespace final_project_backend.Models.Users
{
    public class SeeItems
    {

        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string? ItemDesc { get; set; }
        public int? Quantity { get; set; }
        public int? TotalHarga { get; set; }

    }
}
