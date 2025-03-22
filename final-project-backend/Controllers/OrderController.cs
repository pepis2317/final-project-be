using Microsoft.AspNetCore.Mvc;
using Services;
using final_project_backend.Models.Order;
using MediatR;

namespace Controllers
{
    [Route("api/v1/buyer")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _buyerService;
        private readonly IMediator _mediator;

        public OrderController(OrderService buyerService, IMediator mediator)
        {
            _buyerService = buyerService;
            _mediator = mediator;
        }

        private ProblemDetails Invalid(string details)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "http://veryCoolAPI.com/errors/invalid-data",
                Title = "Invalid Request Data",
                Detail = details,
                Instance = HttpContext.Request.Path
            };
            return problemDetails;
        }


        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }

        [HttpDelete("delete-orders/{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _buyerService.DeleteOrderAsync(orderId);
            if (!result)
            {
                return BadRequest(Invalid( "Order not found" ));
            }
            return Ok("Order deleted successfully");
        }

        [HttpGet("find-order/{buyerId}")]
        public async Task<IActionResult> GetOrdersByBuyerId(Guid buyerId)
        {
            var orders = await _buyerService.GetOrdersByBuyerIdAsync(buyerId);
            return Ok(orders);
        }
        [HttpPut("edit-order/{OrderId}")]
        public async Task<IActionResult> UpdateOrder(Guid OrderId, [FromBody] UpdateOrderRequest request)
        {
            request.OrderId = OrderId;
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
    }
}