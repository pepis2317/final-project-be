using Entities;
using final_project_backend.Models.Shop;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Shop
{
    public class CreateShopValidator : AbstractValidator<CreateShopRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public CreateShopValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.ShopName).NotNull().NotEmpty().WithMessage("Shop name must not be empty");
            RuleFor(x => x.ShopName).MustAsync(ValidShopName).WithMessage("Shop name already in use");
            RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Address must not be empty");
            RuleFor(x => x.OwnerId).NotNull().NotEmpty().WithMessage("Owner id must not be null");
            RuleFor(x => x.OwnerId).MustAsync(ValidOwnerId).WithMessage("Invalid owner id");
            RuleFor(x => x.OwnerId).MustAsync(OnlyOneShop).WithMessage("Only 1 shop allowed per user");
        }
        private async Task<bool> OnlyOneShop(Guid id, CancellationToken token)
        {
            var data = await _db.Shops.FirstOrDefaultAsync(x => x.OwnerId == id, token);
            if(data != null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> ValidOwnerId(Guid id, CancellationToken token)
        {
            var data = await _db.Users.FirstOrDefaultAsync(q=>q.UserId == id, token);
            if(data == null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> ValidShopName(string name, CancellationToken token)
        {
            var data = await _db.Shops.FirstOrDefaultAsync(q=>q.ShopName == name, token);
            if(data != null)
            {
                return false;
            }
            return true;
        }
    }
}
