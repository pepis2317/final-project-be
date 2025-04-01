using Entities;
using MediatR;
using System;

namespace final_project_backend.Commands.Chat
{
    public class CreateChatCommand : IRequest<Entities.ChatChat>
    {
        public Guid UserId { get; set; }
        public Guid SellerId { get; set; }
    }
}
