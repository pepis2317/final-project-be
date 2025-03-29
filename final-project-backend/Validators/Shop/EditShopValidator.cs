using Entities;
using final_project_backend.Models.Shop;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Shop
{
    public class EditShopValidator:AbstractValidator<EditShopRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public EditShopValidator(FinalProjectTrainingDbContext db)
        {
            _db = db;
            RuleFor(x => x).MustAsync(ValidShopName).When(x => !string.IsNullOrEmpty(x.ShopName)).WithMessage("Shop name already in use");

        }
        private async Task<bool> ValidShopName(EditShopRequest request, CancellationToken token)
        {
            var data = await _db.Shops.FirstOrDefaultAsync(q => q.ShopName == request.ShopName, token);
            if (data != null && data.ShopId!=request.ShopId)
            {
                return false;
            }
            return true;
        }
    }
}
