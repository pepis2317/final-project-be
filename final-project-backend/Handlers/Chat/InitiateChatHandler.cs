using Entities;
using final_project_backend.Commands.Chat;
using final_project_backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

using ChatEntity = Entities.ChatChat;

namespace final_project_backend.Handlers.Chat
{
    public class InitiateChatHandler : IRequestHandler<InitiateChatCommand, Guid>
    {
        private readonly FinalProjectTrainingDbContext _context;

        public InitiateChatHandler(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(InitiateChatCommand request, CancellationToken cancellationToken)
        {
            var existingChat = await _context.ChatChats
                .FirstOrDefaultAsync(c =>
                    (c.UserId == request.SenderId && c.SellerId == request.ReceiverId) ||
                    (c.UserId == request.ReceiverId && c.SellerId == request.SenderId),
                    cancellationToken);

            if (existingChat is not null)
            {
                return existingChat.Id;
            }

            var newChat = new ChatEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.SenderId,
                SellerId = request.ReceiverId,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.ChatChats.AddAsync(newChat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newChat.Id;
        }
    }
}
