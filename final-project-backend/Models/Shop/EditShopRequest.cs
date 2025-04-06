using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace final_project_backend.Models.Shop
{
    public class EditShopRequest : IRequest<(ProblemDetails?, ShopModel?)>
    {
        [SwaggerIgnore]
        public Guid ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
    }
}
