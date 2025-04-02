using final_project_backend.Commands.Message;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

// Alias untuk hindari konflik nama namespace vs class
using MessageEntity = Entities.ChatMessage;
using Services;

namespace final_project_backend.Handlers.Message
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageCommand, MessageEntity>
    {
        private readonly MessageService _service;

        public CreateMessageHandler(MessageService service)
        {
            _service = service;
        }

        public async Task<MessageEntity> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.CreateMessage(request);
            return data;
        }
    }
}
