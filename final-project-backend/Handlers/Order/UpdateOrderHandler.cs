using final_project_backend.Models.Order;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace final_project_backend.Handlers.Order
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, (ProblemDetails?, OrderResponse?)>
    {
        private readonly OrderService _service;
        private readonly IValidator<UpdateOrderRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateOrderHandler(OrderService service, IValidator<UpdateOrderRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<(ProblemDetails?, OrderResponse?)> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
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
            var data = await _service.UpdateOrderAsync(request);
            return (null, data);
        }
    }
}
