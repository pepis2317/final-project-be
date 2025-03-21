using final_project_backend.Models.Users;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Handlers.User
{
    public class LoginHandler : IRequestHandler<LoginRequest, (ProblemDetails?, LoginResponse?)>
    {
        private readonly UserService _service;
        private readonly IValidator<LoginRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginHandler(UserService service ,IValidator<LoginRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(ProblemDetails?, LoginResponse?)> Handle(LoginRequest request, CancellationToken cancellationToken)
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
            var data = await _service.Login(request.Email, request.Password);
            return (null, data);
        }
    }
}
