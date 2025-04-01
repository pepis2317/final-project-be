using MediatR;
using System;

namespace final_project_backend.Commands.Chat
{
    public class InitiateChatCommand : IRequest<Guid>
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
