using final_project_backend.Commands.Chat;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Handlers.Chat
{
    public class CreateChatHandler : IRequestHandler<CreateChatCommand, ChatChat>
    {
        private readonly FinalProjectTrainingDbContext _context;

        public CreateChatHandler(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<ChatChat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var existingChat = await _context.ChatChats.FirstOrDefaultAsync(
                c => c.UserId == request.UserId && c.SellerId == request.SellerId,
                cancellationToken
            );

            if (existingChat != null)
            {
                return existingChat;
            }

            var newChat = new ChatChat
            {
                UserId = request.UserId,
                SellerId = request.SellerId,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ChatChats.Add(newChat);
            await _context.SaveChangesAsync(cancellationToken);

            return newChat;
        }
    }
}
