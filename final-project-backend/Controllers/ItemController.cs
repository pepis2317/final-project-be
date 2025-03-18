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
    }
}