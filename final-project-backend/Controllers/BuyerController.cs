using Microsoft.AspNetCore.Mvc;
using Services;
using final_project_backend.Models.Users;

namespace Controllers
{
    [Route("api/v1/buyer")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly BuyerService _buyerService;

        public BuyerController(BuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet("see-all-items")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _buyerService.GetAllItemsAsync();
            return Ok(items);
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