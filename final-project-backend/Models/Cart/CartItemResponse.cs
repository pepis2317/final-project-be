using final_project_backend.Models.Item;

namespace final_project_backend.Models.Cart
{
    public class CartItemResponse
    {
        public required Guid CartId { get; set; }
        public required Guid CartItemId { get; set; }
        public  ItemResponse? Item { get; set; }
        public required int Quantity {  get; set; }

    }
}
