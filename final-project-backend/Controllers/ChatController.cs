using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using final_project_backend.Queries.Chat;
using System;
using System.Threading.Tasks;
using final_project_backend.Commands.Chat;
using Services;

namespace final_project_backend.Controllers
{
    [ApiController]
    [Route("api/v1/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ChatService _service;

        public ChatController(IMediator mediator, ChatService service)
        {
            _mediator = mediator;
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatsByUserId(Guid userId)
        {
            //var result = await _mediator.Send(new GetUserChatsQuery(userId));
            var result = await _service.GetChatsByUserId(userId);
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
