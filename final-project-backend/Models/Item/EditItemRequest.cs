namespace final_project_backend.Models.Item
{
    public class EditItemRequest
    {
        public string? ItemName {  get; set; }
        public string? ItemDesc { get; set; }
        public int? Quantity { get; set; }
        public int? HargaPerItem { get; set; }
    }
}
