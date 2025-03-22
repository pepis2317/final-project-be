using final_project_backend.Models.Item;
using FluentValidation;

namespace final_project_backend.Validators.Item
{
    public class EditItemValidator : AbstractValidator<EditItemRequest>
    {
        public EditItemValidator()
        {
            RuleFor(x => x.ItemName).NotEmpty().When(x => !string.IsNullOrEmpty(x.ItemName)).WithMessage("Name must not be null");
            RuleFor(x => x.Quantity).Must(q => q > 0).When(x => !(x.Quantity==null)).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.HargaPerItem).Must(q => q > 0).When(x => !(x.HargaPerItem == null)).WithMessage("Price must be greater than 0");
        }
    }
}
