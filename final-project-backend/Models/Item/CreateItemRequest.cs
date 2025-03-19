namespace final_project_backend.Models.Item
{
    public class CreateItemRequest
    {
        public required string ItemName {  get; set; }
        public string? ItemDesc { get; set; }
        public required int Quantity {  get; set; }
        public int? HargaPerItem { get; set; }
    }
}
