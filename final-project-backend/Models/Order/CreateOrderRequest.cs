using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Models.Order
{
    public class CreateOrderRequest : IRequest<(ProblemDetails?, OrderResponse?)>
    {
        public Guid BuyerId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public int TotalHarga { get; set; }
        //public string? OrderDetails { get; set; }
    }
}
