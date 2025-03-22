using Entities;
using final_project_backend.Models.Order;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Order
{
    public class CreateOrderValidator:AbstractValidator<CreateOrderRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public CreateOrderValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.BuyerId).NotEmpty().NotNull().WithMessage("Buyer id must not be null");
            RuleFor(x => x.ItemId).NotEmpty().NotNull().WithMessage("Item id must not be null");
            RuleFor(x => x.Quantity).NotEmpty().NotNull().WithMessage("Quantity must not be null");
            RuleFor(x => x.TotalHarga).NotEmpty().NotNull().WithMessage("Total harga diisi yak");

            RuleFor(x => x.BuyerId).MustAsync(ValidBuyerId).WithMessage("Invalid buyer id");
            RuleFor(x => x.ItemId).MustAsync(ValidItemId).WithMessage("Invalid item id");
            RuleFor(x => x.Quantity).Must(q => q > 0).WithMessage("Quanitty must be greater than 0");
            RuleFor(x => x.TotalHarga).Must(q => q > 0).WithMessage("Total harga lebih gede dri 0 yak");
        }
        private async Task<bool> ValidItemId(Guid ItemId, CancellationToken token)
        {
            var data = await _db.Items.FirstOrDefaultAsync(x => x.ItemId == ItemId);
            if(data == null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> ValidBuyerId(Guid buyerId, CancellationToken token)
        {
            var data = await _db.Users.FirstOrDefaultAsync(x => x.UserId == buyerId, token);
            if (data == null)
            {
                return false;
            }
            return true;
        }
    }
}
