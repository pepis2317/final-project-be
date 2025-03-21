using final_project_backend.Models.Item;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace final_project_backend.Handlers.Item
{
    public class EditItemHandler : IRequestHandler<EditItemRequest, (ProblemDetails?, ItemResponse?)>
    {
        private readonly ItemService _service;
        private readonly IValidator<EditItemRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditItemHandler(ItemService service, IValidator<EditItemRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<(ProblemDetails?, ItemResponse?)> Handle(EditItemRequest request, CancellationToken cancellationToken)
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
            var data = await _service.EditItem(request);
            return (null, data);
        }
    }
}
