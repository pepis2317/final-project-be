using final_project_backend.Models.Shop;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Handlers.Shop
{
    public class EditShopHandler : IRequestHandler<EditShopRequest, (ProblemDetails?, ShopModel?)>
    {
        private readonly ShopService _service;
        private readonly IValidator<EditShopRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditShopHandler(ShopService service, IValidator<EditShopRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<(ProblemDetails?, ShopModel?)> Handle(EditShopRequest request, CancellationToken cancellationToken)
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
            var data = await _service.EditShop(request);
            return (null, data);
        }
    }
}
