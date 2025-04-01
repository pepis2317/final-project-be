using Entities;
using final_project_backend.Models.Cart;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Cart
{
    public class EditIncompleteCartValidator : AbstractValidator<CartItemEditRequest>
    {
        /*Random Push*/
        private readonly FinalProjectTrainingDbContext _db;
        public EditIncompleteCartValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x=>x.CartItemId).NotEmpty().NotNull().WithMessage("Cart item id must not be null");
            RuleFor(x => x.CartItemId).MustAsync(ValidCartItemId).WithMessage("Invalid cart id");
        }
        public async Task<bool> ValidCartItemId(Guid CartItemId, CancellationToken token)
        {
            var data = await _db.CartItems.FirstOrDefaultAsync(q=>q.CartItemId == CartItemId);
            if (data == null)
            {
                return false;
            }
            return true;
        }
    }
}
