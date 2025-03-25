using final_project_backend.Models.Cart;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Handlers.Cart
{
    public class EditIncompleteCartHandler : IRequestHandler<CartItemEditRequest, (ProblemDetails?, CartItemResponse?)>
    {
        private readonly CartService _service;
        private readonly IValidator<CartItemEditRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditIncompleteCartHandler(CartService service, IValidator<CartItemEditRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(ProblemDetails?, CartItemResponse?)> Handle(CartItemEditRequest request, CancellationToken cancellationToken)
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
            var data = await _service.EditIncompleteCartItem(request);
            return (null, data);
        }
    }
}
