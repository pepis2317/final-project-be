using final_project_backend.Models.Item;
using final_project_backend.Models.Order;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace final_project_backend.Handlers.Order
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, (ProblemDetails?, OrderResponse?)>
    {
        private readonly OrderService _service;
        private readonly IValidator<CreateOrderRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateOrderHandler(OrderService service, IValidator<CreateOrderRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<(ProblemDetails?, OrderResponse?)> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
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
            var data = await _service.CreateOrderAsync(request);
            return (null, data);
        }
    }
}
