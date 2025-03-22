using Entities;
using final_project_backend.Models.Users;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.User
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public UserUpdateValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("User id must be provided");
            RuleFor(x => x.UserId).MustAsync(IsValidId).WithMessage("Invalid user id");
            RuleFor(x => x.UserEmail).Must(IsValidEmail).When(x => !string.IsNullOrEmpty(x.UserEmail)).WithMessage("Invalid email format");
            RuleFor(x => x).MustAsync(CheckNewEmail).When(x => !string.IsNullOrEmpty(x.UserEmail)).WithMessage("Email already in use by another user");
            RuleFor(x => x).MustAsync(IsValidUserName).When(x => !string.IsNullOrEmpty(x.UserName)).WithMessage("Username already in use");
            RuleFor(x => x).MustAsync(IsValidPhone).When(x => !string.IsNullOrEmpty(x.UserPhoneNumber)).WithMessage("Phone number already in use");
        }
        private async Task<bool> IsValidId(Guid userId, CancellationToken token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userId); 
            if(user == null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> IsValidPhone(UserUpdateRequest request, CancellationToken token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserPhoneNumber == request.UserPhoneNumber, token);
            if (user != null)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsValidUserName(UserUpdateRequest request, CancellationToken token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, token);
            if (user != null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> CheckNewEmail(UserUpdateRequest request, CancellationToken token)
        {
            var user = await _db.Users.Where(q => q.UserEmail == request.UserEmail).FirstOrDefaultAsync(token);
            if (user != null && user.UserEmail!= request.UserEmail)
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
    }
}
