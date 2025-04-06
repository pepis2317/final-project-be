using final_project_backend.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace final_project_backend.Models.Item
{
    public class CreateItemRequest : IRequest<(ProblemDetails?, ItemResponse?)>
    {
        [SwaggerIgnore]
        public Guid ShopId { get; set; }
        public required string ItemName {  get; set; }
        public string? ItemDesc { get; set; }
        public required int Quantity {  get; set; }
        public int? HargaPerItem { get; set; }
    }
}
