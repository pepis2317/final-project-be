namespace final_project_backend.Models.Cart
{
    public class CartResponse
    {
        public required Guid CartId{get;set;}
        public required Guid BuyerId {get;set;}
        public required DateTime? CompletedAt { get;set;}
    }
}
