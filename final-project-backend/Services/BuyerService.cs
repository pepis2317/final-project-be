using Microsoft.EntityFrameworkCore;
using Entities;

namespace Services
{
    public class BuyerService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public BuyerService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Guid buyerId, Guid itemId)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderDetails = "Pending",
                OrderDate = DateTime.UtcNow,
                BuyerId = buyerId,
                ItemId = itemId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByBuyerIdAsync(Guid buyerId)
        {
            return await _context.Orders
                .Where(o => o.BuyerId == buyerId)
                .ToListAsync();
        }
    }
}
