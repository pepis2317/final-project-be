using Azure.Core;
using Entities;
using final_project_backend.Models.Users;
using final_project_backend.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project_backend.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IMediator _mediator;
        public UserController(UserService service, IMediator mediator)
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

        // GET: api/<UserController>
        [HttpGet("get-all-users")]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAllUsers();
            if (data.IsNullOrEmpty())
            {
                return BadRequest(Invalid("No users exist"));
            }
            return Ok(data);
        }
        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> Get([FromQuery]Guid UserId)
        {
            var data = await _service.Get(UserId);
            return Ok(data);
        }
        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserUpdateRequest updatedUser)
        {
            updatedUser.UserId = userId;
            var result = await _mediator.Send(updatedUser);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpPost("upload-pfp")]
        public async Task<IActionResult> UploadPfp(UploadPfpRequest request)
        {
            var fileName = $"{Guid.NewGuid()}_{request.file.FileName}";
            var contentType = request.file.ContentType;
            using var stream = request.file.OpenReadStream();
            var blobUrl= await _service.UploadPfp(request.UserId, stream, fileName, contentType);
            if (blobUrl == null)
            {
                return BadRequest(Invalid("Issue with getting blobUrl"));
            }
            return Ok(new { blobUrl });
        }
        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [Authorize]
        [HttpGet("user-data")]
        public async Task<IActionResult> GetUserData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value;
            if(userId == null)
            {
                return BadRequest(Invalid("User id not found in JWT"));
            }
            var user = await _service.Get(Guid.Parse(userId));
            return Ok(user);
        }
        [HttpPost("user-login")]
        public async Task<IActionResult> Login([FromQuery] LoginRequest request)
        {
            var result = await _mediator.Send(request);
            if(result.Item1 != null)
            {
                return BadRequest(result.Item1);
            }
            return Ok(result.Item2);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var data = await _service.RefreshToken(request.RefreshToken);
            if (data == null)
            {
                return BadRequest(Invalid("Expired refresh token"));
            }
            return Ok(data);
        }

        [HttpGet("find-user/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _service.Get(userId);
            if (user == null)
            {
                return NotFound(Invalid("User not found"));
            }
            return Ok(user);
        }
    }
}
