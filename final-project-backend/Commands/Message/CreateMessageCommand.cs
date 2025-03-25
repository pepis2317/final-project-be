using Entities;
using MediatR;
using System;

namespace final_project_backend.Commands.Message
{
    using Entities;
    public class CreateMessageCommand : IRequest<Message>
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; } = string.Empty;
    }
}
