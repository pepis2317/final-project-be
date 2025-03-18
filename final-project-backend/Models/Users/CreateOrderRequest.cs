﻿namespace final_project_backend.Models.Users
{
    public class CreateOrderRequest
    {
        public Guid BuyerId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public int TotalHarga { get; set; }
        public string? OrderDetails { get; set; }
    }
}
