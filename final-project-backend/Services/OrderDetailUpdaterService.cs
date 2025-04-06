using Entities;
using Microsoft.EntityFrameworkCore;
using Services;

namespace final_project_backend.Services
{
    public class OrderDetailUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(5);

        public OrderDetailUpdaterService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateOrderStatusesAsync();
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }

        private async Task UpdateOrderStatusesAsync()
        {
            using (var scope = _scopeFactory.CreateScope()) 
            {
                var orderService = scope.ServiceProvider.GetRequiredService<OrderService>(); 
                await orderService.UpdateOrderDetail(); 
            }
        }
    }
}
