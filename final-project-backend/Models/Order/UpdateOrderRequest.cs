using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace final_project_backend.Models.Order
{
    public class UpdateOrderRequest : IRequest<(ProblemDetails?, OrderResponse?)>
    {
        [SwaggerIgnore]
        public Guid OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? TotalHarga { get; set; }
        public string? OrderDetails { get; set; }
    }
}
