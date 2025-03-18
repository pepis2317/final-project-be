namespace final_project_backend.Models.Order
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string? OrderDetails { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? Quantity { get; set; }
        public int? TotalHarga { get; set; }
    }
}
