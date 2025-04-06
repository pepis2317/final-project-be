using final_project_backend.Models.Users;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Handlers.User
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, (ProblemDetails?, UserResponse?)>
    {
        private readonly UserService _service;
        private readonly IValidator<RegisterRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RegisterHandler(UserService service, IValidator<RegisterRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(ProblemDetails?, UserResponse?)> Handle(RegisterRequest request, CancellationToken cancellationToken)
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
            var data = await _service.Register(request);
            return (null, data);
        }
    }
}
