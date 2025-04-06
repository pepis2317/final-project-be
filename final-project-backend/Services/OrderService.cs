using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Order;
using Azure.Core;
namespace Services
{
    public class OrderService
    {
        private readonly FinalProjectTrainingDbContext _context;
        private readonly ItemService _itemService;

        public OrderService(FinalProjectTrainingDbContext context, ItemService itemService)
        {
            _context = context;
            _itemService = itemService;
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
                OrderDate = DateTime.UtcNow,
                Confirmed = "unconfirmed"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return new OrderResponse
            {
                OrderId = Guid.NewGuid(),
                Quantity = request.Quantity,
                TotalHarga = request.TotalHarga,
                OrderDetails = "Pending",
                OrderDate = DateTime.UtcNow,
                Confirmed = "unconfirmed"
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
                OrderDate = data.OrderDate,
                Confirmed = data.Confirmed
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
            var orders = await _context.Orders.Where(o => o.BuyerId == buyerId).OrderByDescending(q => q.OrderDate).ToListAsync();
            var result = new List<OrderResponse>();
            foreach(var order in orders)
            {
                var item = await _itemService.GetItemById(order.ItemId);
                result.Add(new OrderResponse
                {
                    OrderId = order.OrderId,
                    OrderDetails = order.OrderDetails,
                    OrderDate = order.OrderDate,
                    Quantity = order.Quantity,
                    TotalHarga = order.TotalHarga,
                    Confirmed = order.Confirmed,
                    Item = item
                });
            }
            return result;
        }
        public async Task<IEnumerable<OrderResponse>> GetUnconfirmedOrdersByBuyerIdAsync (Guid buyerId)
        {
            var orders = await _context.Orders.Where(o => o.BuyerId == buyerId && o.Confirmed == "unconfirmed").OrderByDescending(q=>q.OrderDate).ToListAsync();
            var result = new List<OrderResponse>();
            foreach (var order in orders)
            {
                var item = await _itemService.GetItemById(order.ItemId);
                result.Add(new OrderResponse
                {
                    OrderId = order.OrderId,
                    OrderDetails = order.OrderDetails,
                    OrderDate = order.OrderDate,
                    Quantity = order.Quantity,
                    TotalHarga = order.TotalHarga,
                    Confirmed = order.Confirmed,
                    Item = item
                });
            }
            return result;
        }
        public async Task<OrderResponse?>ConfirmOrder(Guid OrderId)
        {
            var data = await _context.Orders.FirstOrDefaultAsync(q=>q.OrderId == OrderId);
            if(data == null)
            {
                return null;
            }
            data.Confirmed = "confirmed";
            _context.Orders.Update(data);
            await _context.SaveChangesAsync();
            return new OrderResponse
            {
                OrderId = data.OrderId,
                Quantity = data.Quantity,
                TotalHarga = data.TotalHarga,
                OrderDetails = data.OrderDetails,
                OrderDate = data.OrderDate,
                Confirmed= data.Confirmed
            };
        }
    }
}
