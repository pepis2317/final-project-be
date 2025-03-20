using Entities;
using final_project_backend.Models.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.User
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public RegisterValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.UserAddress).NotNull().NotEmpty();
            RuleFor(x => x.UserPhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.UserEmail).NotNull().NotEmpty();
            RuleFor(x => x.BirthDate).NotNull().NotEmpty();
            RuleFor(x => x.UserPhoneNumber).MustAsync(IsValidPhone).WithMessage("Phone number already in use");
            RuleFor(x => x.UserName).MustAsync(IsValidUserName).WithMessage("Username already in use");
            RuleFor(x => x.UserEmail).Must(IsValidEmail).WithMessage("Invalid email format");
            RuleFor(x => x.UserEmail).MustAsync(CheckNewEmail).WithMessage("Email already in use by another user");

        }
        private async Task<bool> CheckNewEmail(string email, CancellationToken token)
        {
            var user = await _db.Users.Where(q => q.UserEmail == email).FirstOrDefaultAsync(token);
            if (user != null && user.UserEmail != email)
            {
                return false;
            }
            return true;
        }
        private bool IsValidEmail(string? email)
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
        private async Task<bool> IsValidUserName(string username, CancellationToken token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == username, token);
            if (user != null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> IsValidPhone(string phone, CancellationToken token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserPhoneNumber == phone, token);
            if (user != null)
            {
                return false;
            }
            return true;
        }
    }
}
