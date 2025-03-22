using final_project_backend.Models.Item;
using final_project_backend.Models.Shop;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace final_project_backend.Handlers.Item
{
    public class CreateItemHandler :IRequestHandler <CreateItemRequest, (ProblemDetails?, ItemResponse?)>
    {
        private readonly ItemService _service;
        private readonly IValidator<CreateItemRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateItemHandler(ItemService service, IValidator<CreateItemRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<(ProblemDetails?, ItemResponse?)> Handle(CreateItemRequest request, CancellationToken cancellationToken)
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
            var data = await _service.CreateItem(request);
            return (null, data);
        }
    }
}
