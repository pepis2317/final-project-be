using final_project_backend.Models.Item;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace final_project_backend.Models.Cart
{
    public class CartItemRequest : IRequest<(ProblemDetails?, CartItemResponse?)>
    {
        [SwaggerIgnore]
        public Guid CartItemId { get; set; }
        public required Guid UserId { get; set; }
        public required Guid ItemId { get; set; }
        public required int Quantity {  get; set; }
    }
}
