using Entities;
using final_project_backend.Models.Shop;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Shop
{
    public class EditShopValidator:AbstractValidator<EditOrderRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public EditShopValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x.ShopName).MustAsync(ValidShopName).When(x => !string.IsNullOrEmpty(x.ShopName)).WithMessage("Shop name already in use");

        }
        private async Task<bool> ValidShopName(string name, CancellationToken token)
        {
            var data = await _db.Shops.FirstOrDefaultAsync(q => q.ShopName == name, token);
            if (data != null)
            {
                return false;
            }
            return true;
        }
    }
}
