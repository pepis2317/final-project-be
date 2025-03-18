using Microsoft.AspNetCore.Mvc;
using Services;
using final_project_backend.Models.Order;

namespace Controllers
{
    [Route("api/v1/buyer")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _buyerService;

        public OrderController(OrderService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var order = await _buyerService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(CreateOrder), new { id = order.OrderId }, order);
        }

        [HttpDelete("delete-orders/{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _buyerService.DeleteOrderAsync(orderId);
            if (!result)
                return NotFound(new { message = "Order not found" });

            return NoContent();
        }

        [HttpGet("find-order/{buyerId}")]
        public async Task<IActionResult> GetOrdersByBuyerId(Guid buyerId)
        {
            var orders = await _buyerService.GetOrdersByBuyerIdAsync(buyerId);
            return Ok(orders);
        }
    }
}