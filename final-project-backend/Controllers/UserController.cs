using Entities;
using final_project_backend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project_backend.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        // GET: api/<UserController>
        [HttpGet("get-all-users")]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAllUsers();
            return Ok(data);
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] User updatedUser)
        {
            var result = await _service.UpdateUserById(userId, updatedUser);
            if (result == null) return NotFound("User not found.");
            return Ok(result);
        }
    }
}
