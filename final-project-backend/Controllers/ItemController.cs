using final_project_backend.Models.Item;
using final_project_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }
        [HttpGet("see-all")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetItemById(Guid ItemId)
        {
            var item = await _itemService.GetItemById(ItemId);
            return Ok(item);
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
            var item = await _itemService.CreateItem(ShopId, request);
            return Ok(item);
        }
        [HttpPut("edit-item")]
        public async Task<IActionResult> EditItem(Guid ItemId,[FromBody] EditItemRequest request)
        {
            var item = await _itemService.EditItem(ItemId,request);
            return Ok(item);
        }
        [HttpDelete("delete-item")]
        public async Task<IActionResult> DeleteItem(Guid ItemId)
        {
            var data = await _itemService.DeleteItem(ItemId);
            if (data == null)
            {
                return NotFound(new { message = "Item tidak ditemukan" });
            }

            return Ok(new { message = $"Item '{data}' berhasil dihapus" }); 
        }

    }
}