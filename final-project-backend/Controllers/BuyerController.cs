using Microsoft.AspNetCore.Mvc;
using Entities;
using Services;

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
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            var items = await _buyerService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromQuery] Guid buyerId, [FromQuery] Guid itemId)
        {
            try
            {
                var order = await _buyerService.CreateOrderAsync(buyerId, itemId);
                return Ok(new { message = "Order created successfully!", order });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

        [HttpGet("see-all-order/{buyerId}")]
        public async Task<IActionResult> GetOrdersByBuyer(Guid buyerId)
        {
            var orders = await _buyerService.GetOrdersByBuyerIdAsync(buyerId);
            if (orders == null || !orders.Any()) return NotFound("No orders found for this buyer.");
            return Ok(orders);
        }

    }
}
