using Entities;
using final_project_backend.Models.Item;
using final_project_backend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly IMediator _mediator;

        public ItemsController(ItemService itemService, IMediator mediator)
        {
            _itemService = itemService;
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
        [HttpGet("see-all")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            if (items.IsNullOrEmpty())
            {
                return BadRequest(Invalid("No items exist"));
            }
            return Ok(items);
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetItemById(Guid ItemId)
        {
            var item = await _itemService.GetItemById(ItemId);
            if(item == null)
            {
                return BadRequest(Invalid("Invalid item id"));
            }
            return Ok(item);
        }

        [HttpGet("get-query")]
        public async Task<IActionResult> GetFromQuery([FromQuery] string searchTerm)
        {
            var items = await _itemService.GetItemsFromQuery(null, searchTerm);
            if (items == null)
            {
                return BadRequest(Invalid("Nothing found using query"));
            }
            return Ok(items);
        }
        [HttpGet("get-query/{ShopId}")]
        public async Task<IActionResult> GetFromShop(Guid ShopId, [FromQuery] string searchTerm)
        {
            var items = await _itemService.GetItemsFromQuery(ShopId, searchTerm);
            if (items == null)
            {
                return BadRequest(Invalid("Nothing found using query in this shop"));
            }
            return Ok(items);
        }
        [HttpGet("get-shop-items")]
        public async Task<IActionResult> GetItemsFromShop(Guid ShopId)
        {
            var items = await _itemService.GetItemsFromShop(ShopId);
            return Ok(items);
        }
        [HttpPost("create-item")]
        public async Task<IActionResult> CreateItem(Guid ShopId,[FromBody] CreateItemRequest request)
        {
            request.ShopId = ShopId;
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpPut("edit-item")]
        public async Task<IActionResult> EditItem(Guid ItemId,[FromBody] EditItemRequest request)
        {
            request.ItemId = ItemId;
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpDelete("delete-item")]
        public async Task<IActionResult> DeleteItem(Guid ItemId)
        {
            var data = await _itemService.DeleteItem(ItemId);
            if (data == null)
            {
                return BadRequest(Invalid("Invalid item id"));
            }
            return Ok(data);
        }
    }
}