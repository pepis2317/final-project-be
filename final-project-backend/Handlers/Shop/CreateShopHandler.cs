using final_project_backend.Models.Shop;
using final_project_backend.Models.Users;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Handlers.Shop
{
    public class CreateShopHandler : IRequestHandler<CreateShopRequest, (ProblemDetails?, ShopModel?)>
    {
        private readonly ShopService _service;
        private readonly IValidator<CreateShopRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateShopHandler(ShopService service, IValidator<CreateShopRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<(ProblemDetails?, ShopModel?)> Handle(CreateShopRequest request, CancellationToken cancellationToken)
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
            var data = await _service.CreateShop(request);
            return (null, data);
        }
    }
}
