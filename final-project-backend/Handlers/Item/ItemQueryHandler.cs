using final_project_backend.Models.Item;
using final_project_backend.Validators.Item;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace final_project_backend.Handlers.Item
{
    public class ItemQueryHandler : IRequestHandler<ItemQuery, (ProblemDetails?, List<ItemResponse>?)>
    {
        private readonly ItemService _service;
        private readonly IValidator<ItemQuery> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ItemQueryHandler(ItemService service,IValidator<ItemQuery> validator, IHttpContextAccessor  httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<(ProblemDetails?, List<ItemResponse>?)> Handle(ItemQuery request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                var problemDetails = new ProblemDetails
                {
                    Type = "http://veryCoolAPI.com/errors/invalid-data",
                    Title = "Invalid Request Data",
                    Detail = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)),
                    Instance = _httpContextAccessor.HttpContext?.Request.Path
                };
                return (problemDetails, null);
            }
            var data = await _service.GetItemsFromQuery(request.ShopId, request.SearchTerm);
            return (null, data);
        }
    }
}
