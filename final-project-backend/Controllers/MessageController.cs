using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using final_project_backend.Queries.Messages;
using System;
using System.Threading.Tasks;
using final_project_backend.Handlers.Message;
using final_project_backend.Commands.Message;
using final_project_backend.Queries.Messages;

namespace final_project_backend.Controllers
{
    [ApiController]
    [Route("api/v1/message")]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/message/chat/{chatId}
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesByChatId(Guid chatId)
        {
            var result = await _mediator.Send(new GetMessagesByChatIdQuery(chatId));
            return Ok(result);
        }

        // POST api/v1/message
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
