using Entities;
using final_project_backend.Models.Item;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Item
{
    public class ItemQueryValidator :AbstractValidator<ItemQuery>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public ItemQueryValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.ShopId).MustAsync(ValidShopId).When(x => x.ShopId != null).WithMessage("Invalid shop id");
        }
        private async Task<bool> ValidShopId(Guid? ShopId, CancellationToken token)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(q => q.ShopId == ShopId);
            if (shop == null)
            {
                return false;
            }
            return true;
        }
    }
}
