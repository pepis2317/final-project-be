using final_project_backend.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Models.Shop
{
    public class CreateShopRequest : IRequest<(ProblemDetails?, ShopModel?)>
    {

        public required string ShopName {  get; set; }
        public required Guid OwnerId { get; set; }
        public required string? Description { get; set; }
        public required string Address {  get; set; }

    }
}
