using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Models.Users
{
    public class LoginRequest: IRequest<(ProblemDetails?, LoginResponse?)>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
