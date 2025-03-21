using final_project_backend.Models.Users;
using final_project_backend.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Handlers.User
{
    public class UserUpdateHandler : IRequestHandler<UserUpdateRequest, (ProblemDetails?, UserResponse?)>
    {
        private readonly UserService _service;
        private readonly IValidator<UserUpdateRequest> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserUpdateHandler(UserService service, IValidator<UserUpdateRequest> validator, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(ProblemDetails?, UserResponse?)> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
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
            var data = await _service.UpdateUserById(request);
            return (null, data);
        }
    }
}
