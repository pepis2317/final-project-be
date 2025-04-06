using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace final_project_backend.Models.Users
{
    public class RegisterRequest: IRequest<(ProblemDetails?, UserResponse?)>
    {
        public required string UserName { get; set; }
        public required string UserPassword { get; set; }
        public required string UserPhoneNumber { get; set; }
        public required string UserEmail { get; set; }
        public required string UserAddress { get; set; }
        public required DateOnly BirthDate { get; set; }
        public required string Gender { get; set; }

    }
}
