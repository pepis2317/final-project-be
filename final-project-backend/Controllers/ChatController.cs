using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using final_project_backend.Queries.Chat;
using System;
using System.Threading.Tasks;
using final_project_backend.Commands.Chat;

namespace final_project_backend.Controllers
{
    [ApiController]
    [Route("api/v1/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatsByUserId(Guid userId)
        {
            var result = await _mediator.Send(new GetUserChatsQuery(userId));
            return Ok(result);
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiateChat([FromBody] InitiateChatCommand command)
        {
            var chatId = await _mediator.Send(command);
            return Ok(new { chatId });
        }
    }
}
