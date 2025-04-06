using Entities;
using final_project_backend.Models.Item;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Item
{
    public class CreateItemValidator :AbstractValidator<CreateItemRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public CreateItemValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;

            RuleFor(x => x.ShopId).NotEmpty().NotNull().WithMessage("Shop id must not be null");
            RuleFor(x => x.ShopId).MustAsync(CheckShopId).WithMessage("Invalid shop id");
            RuleFor(x => x.ItemName).NotEmpty().NotNull().WithMessage("Name must not be null");
            RuleFor(x => x.Quantity).NotEmpty().NotNull().WithMessage("Quantity must not be null");
            RuleFor(x => x.HargaPerItem).NotEmpty().NotNull().WithMessage("Price must not be null");
            RuleFor(x => x.Quantity).Must(q => q > 0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.HargaPerItem).Must(q => q > 0).WithMessage("Price must be greater than 0");
        }
        private async Task<bool> CheckShopId(Guid shopId, CancellationToken token)
        {
            var data = await _db.Shops.FirstOrDefaultAsync(q=>q.ShopId ==  shopId, token);
            if(data ==null)
            {
                return false;
            }
            return true;
        }
    }
}
