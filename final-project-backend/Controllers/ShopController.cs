using final_project_backend.Models.Shop;
using final_project_backend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project_backend.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _service;
        private readonly IMediator _mediator;
        public ShopController(ShopService service, IMediator mediator)
        {
            _service = service;
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
        // GET: api/<ShopController>
        [HttpGet("get-all-shops")]
        public async Task<IActionResult> GetAllShops()
        {
            var data = await _service.GetAllShops();
            if(data.IsNullOrEmpty())
            {
                return BadRequest(Invalid("No shops exist"));
            }
            return Ok(data);
        }
        [HttpGet("get-shop/{ShopId}")]
        public async Task<IActionResult> GetShopById(Guid ShopId)
        {
            var data = await _service.GetShopById(ShopId);
            if(data == null)
            {
                return BadRequest(Invalid("Invalid shop id"));
            }
            return Ok(data);
        }
        [HttpPost("create-shop")]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpPut("edit-shop/{ShopId}")]
        public async Task<IActionResult> EditShop(Guid ShopId, [FromBody] EditOrderRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpDelete("delete-shop/{ShopId}")]
        public async Task<IActionResult> DeleteShop(Guid ShopId)
        {
            var data = await _service.DeleteShop(ShopId);
            if (data == null)
            {
                return BadRequest(Invalid("Invalid shop id"));
            }
            return Ok(data);
        }


    }
}
