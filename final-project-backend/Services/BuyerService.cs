using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Users;
namespace Services
{
    public class BuyerService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public BuyerService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeeItems>> GetAllItemsAsync()
        {
            return await _context.Items
                .Select(item => new SeeItems
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ItemDesc = item.ItemDesc,
                    Quantity = item.Quantity,
                    TotalHarga = item.TotalHarga
                })
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                BuyerId = request.BuyerId,
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                TotalHarga = request.TotalHarga,
                OrderDetails = request.OrderDetails,
                OrderDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<OrderResponse>> GetOrdersByBuyerIdAsync(Guid buyerId)
        {
            return await _context.Orders
                .Where(o => o.BuyerId == buyerId)
                .Select(o => new OrderResponse
                {
                    OrderId = o.OrderId,
                    OrderDetails = o.OrderDetails,
                    OrderDate = o.OrderDate,
                    Quantity = o.Quantity,
                    TotalHarga = o.TotalHarga
                })
                .ToListAsync();
        }
    }
}
