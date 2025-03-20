using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Order;
namespace Services
{
    public class OrderService
    {
        private readonly FinalProjectTrainingDbContext _context;


        public OrderService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }


        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                BuyerId = request.BuyerId,
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                TotalHarga = request.TotalHarga,
                OrderDetails = "Pending",
                OrderDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return new OrderResponse
            {
                OrderId = Guid.NewGuid(),
                Quantity = request.Quantity,
                TotalHarga = request.TotalHarga,
                OrderDetails = "Pending",
                OrderDate = DateTime.UtcNow
            };
        }
        public async Task <OrderResponse?> UpdateOrderAsync(UpdateOrderRequest request)
        {
            var data = await _context.Orders.FirstOrDefaultAsync(q => q.OrderId == request.OrderId);
            if (data == null)
            {
                return null;
            }
            data.Quantity = request.Quantity == null? data.Quantity: request.Quantity;
            data.TotalHarga = request.TotalHarga == null? data.TotalHarga:request.TotalHarga;
            data.OrderDetails = string.IsNullOrEmpty(request.OrderDetails)? data.OrderDetails:request.OrderDetails;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return new OrderResponse
            {
                OrderId = data.OrderId,
                Quantity = data.Quantity,
                TotalHarga = data.TotalHarga,
                OrderDetails = data.OrderDetails,
                OrderDate = data.OrderDate
            };
        }
        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task UpdateOrderDetail()
        {
            var orders = await _context.Orders.Where(o => o.OrderDetails != "Arrived").ToListAsync();

            foreach (var order in orders)
            {
                var timeElapsed = DateTime.UtcNow - order.OrderDate;

                if (timeElapsed >= TimeSpan.FromMinutes(10))
                {
                    order.OrderDetails = "Arrived";
                }
                else if (timeElapsed >= TimeSpan.FromMinutes(5))
                {
                    order.OrderDetails = "Sending";
                }
            }

            await _context.SaveChangesAsync();
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
                }).ToListAsync();
        }
    }
}
