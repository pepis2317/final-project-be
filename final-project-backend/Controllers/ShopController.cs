using final_project_backend.Models.Shop;
using final_project_backend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project_backend.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _service;
        public ShopController(ShopService service)
        {
            _service = service;
        }
        // GET: api/<ShopController>
        [HttpGet("get-all-shops")]
        public async Task<IActionResult> GetAllShops()
        {
            var data = await _service.GetAllShops();
            return Ok(data);
        }
        [HttpGet("get-shop/{ShopId}")]
        public async Task<IActionResult> GetShopById(Guid ShopId)
        {
            var data = await _service.GetShopById(ShopId);
            return Ok(data);
        }
        [HttpPost("create-shop")]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequest request)
        {
            var data = await _service.CreateShop(request);
            return Ok(data);
        }
        [HttpPut("edit-shop/{ShopId}")]
        public async Task<IActionResult> EditShop(Guid ShopId, [FromBody] EditShopRequest request)
        {
            var data = await _service.EditShop(ShopId, request);
            return Ok(data);
        }
        [HttpDelete("delete-shop/{ShopId}")]
        public async Task<IActionResult> DeleteShop(Guid ShopId)
        {
            var data = await _service.DeleteShop(ShopId);
            return Ok(data);
        }


    }
}
