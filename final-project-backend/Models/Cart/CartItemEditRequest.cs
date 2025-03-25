using final_project_backend.Models.Item;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Models.Cart
{
    public class CartItemEditRequest : IRequest<(ProblemDetails?, CartItemResponse?)>
    {
        public required Guid CartItemId { get; set; }
        public int? Quantity {  get; set; }
    }
}
