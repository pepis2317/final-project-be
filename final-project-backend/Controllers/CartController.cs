using final_project_backend.Models.Cart;
using final_project_backend.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace final_project_backend.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;
        private readonly IMediator _mediator;
        public CartController(CartService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet("check-cart/{UserId}/{ItemId}")]
        public async Task<IActionResult> CheckInCart(Guid UserId, Guid ItemId)
        {
            var data = await _service.CheckInCart(UserId, ItemId);  
            return Ok(data);
        }
        [HttpGet("get-incomplete-cart")]
        public async Task<IActionResult> GetIncompleteCart([FromQuery] Guid UserId)
        {
            var data = await _service.GetIncompleteCartItems(UserId);
            return Ok(data);
        }
        [HttpPost("post-cart-item")]
        public async Task<IActionResult> PostIncompleteCartItem([FromBody] CartItemRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpPut("edit-cart-item")]
        public async Task<IActionResult> EditIncompleteCartItem([FromBody] CartItemEditRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpDelete("delete-cart-item/{CartItemId}")]
        public async Task<IActionResult> DeleteIncompleteCartItem(Guid CartItemId)
        {
            var data = await _service.DeleteIncompleteCartItem(CartItemId);
            return Ok(data);
        }
        [HttpPut("complete-cart/{UserId}")]
        public async Task<IActionResult> CompleteCart(Guid UserId)
        {
            var data = await _service.CompleteCart(UserId);
            return Ok(data);
        }
    }
}
