using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Models.Item
{
    public class ItemQuery : IRequest<(ProblemDetails?, List<ItemResponse>?)>
    {
        public string? SearchTerm { get; set; }
        public Guid? ShopId { get; set; }
    }
}
