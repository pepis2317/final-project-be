using Azure.Core;
using Entities;
using final_project_backend.Models.Users;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.User
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        private readonly IDataProtector _protector;
        public LoginValidator(FinalProjectTrainingDbContext db, IDataProtectionProvider provider)
        {
            _db = db;
            _protector = provider.CreateProtector("CredentialsProtector");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email must be filled");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password must be filled");
            RuleFor(x => x.Email).Must(IsValidEmail).WithMessage("Invalid email format");
            RuleFor(x => x).MustAsync(ValidCredentials).WithMessage("Invalid credentials");
        }
        private async Task<bool> ValidCredentials(LoginRequest request, CancellationToken token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(q => q.UserEmail == request.Email, token);
            if (user == null)
            {
                return false;
            }
            return _protector.Unprotect(user.UserPassword) == request.Password;
        }
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

    }
}
