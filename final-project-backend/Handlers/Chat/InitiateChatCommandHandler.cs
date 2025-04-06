using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using final_project_backend.Models;
using final_project_backend.Commands.Chat;
using Services;

namespace final_project_backend.Handlers.Chat
{
    public class InitiateChatCommandHandler : IRequestHandler<InitiateChatCommand, Guid>
    {
        private readonly ChatService _chatService;

        public InitiateChatCommandHandler(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Guid> Handle(InitiateChatCommand request, CancellationToken cancellationToken)
        {
            var id = await _chatService.InitializeChat(request);
            return id;
        }

    }
}
