
using Entities;
using final_project_backend.Models.Order;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Validators.Order
{
    public class UpdateOrderValidator :AbstractValidator<UpdateOrderRequest>
    {
        private readonly FinalProjectTrainingDbContext _db;
        public UpdateOrderValidator(FinalProjectTrainingDbContext db) {
            _db = db;
            RuleFor(x => x.OrderId).NotEmpty().NotNull().WithMessage("Order id must not be null");
            RuleFor(x => x.OrderId).MustAsync(ValidOrderId).WithMessage("Invalid order id");
            RuleFor(x => x.TotalHarga).Must(q => q > 0).When(q => q.TotalHarga!=null).WithMessage("Total harga lebih dri 0 plis");
            RuleFor(x => x.Quantity).Must(q => q > 0).When(q => q.Quantity!=null).WithMessage("Quantity must be greater than 0");
        }
        private async Task<bool>ValidOrderId(Guid orderId, CancellationToken token)
        {
            var data = await _db.Orders.FirstOrDefaultAsync(q => q.OrderId == orderId, token);
            if (data == null)
            {
                return false;
            }
            return true;
        }
    }
}
