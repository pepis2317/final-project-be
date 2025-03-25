using Entities;
using final_project_backend.Models.Cart;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Cart
{
    public class PostIncompleteCartValidator:AbstractValidator<CartItemRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public PostIncompleteCartValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("User id must not be null");
            RuleFor(x => x.UserId).MustAsync(ValidUserId).WithMessage("Invalid user id");
            RuleFor(x => x.ItemId).NotEmpty().NotNull().WithMessage("Item id must not be null");
            RuleFor(x => x.ItemId).MustAsync(ValidItemId).WithMessage("Invalid item id");
            RuleFor(x => x.Quantity).NotEmpty().NotNull().WithMessage("Quantity must not be null");
            RuleFor(x => x.Quantity).Must(x => x > 0).WithMessage("Quantity must be greater than 0");
        }
        public async Task<bool> ValidUserId(Guid UserId, CancellationToken token)
        {
            var data = await _db.Users.FirstOrDefaultAsync(x=>x.UserId == UserId, token);
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> ValidItemId(Guid ItemId, CancellationToken token)
        {
            var data = await _db.Items.FirstOrDefaultAsync(x=>x.ItemId ==  ItemId, token);
            if (data == null)
            {
                return false;
            }
            return true;
        }
    }
}
