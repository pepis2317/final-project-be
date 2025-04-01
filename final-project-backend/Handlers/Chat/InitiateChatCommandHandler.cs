using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using final_project_backend.Models;
using final_project_backend.Commands.Chat;

namespace final_project_backend.Handlers.Chat
{
    public class InitiateChatCommandHandler : IRequestHandler<InitiateChatCommand, Guid>
    {
        private readonly FinalProjectTrainingDbContext _context;

        public InitiateChatCommandHandler(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(InitiateChatCommand request, CancellationToken cancellationToken)
        {
            var sender = await _context.ChatUsers.FindAsync(new object[] { request.SenderId }, cancellationToken);
            if (sender == null)
            {
                sender = new ChatUser
                {
                    Id = request.SenderId,
                    Name = "User",        
                    Role = "user",        
                    CreatedAt = DateTime.UtcNow
                };
                _context.ChatUsers.Add(sender);
            }

            var receiver = await _context.ChatUsers.FindAsync(new object[] { request.ReceiverId }, cancellationToken);
            if (receiver == null)
            {
                receiver = new ChatUser
                {
                    Id = request.ReceiverId,
                    Name = "Seller",      
                    Role = "seller",
                    CreatedAt = DateTime.UtcNow
                };
                _context.ChatUsers.Add(receiver);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var existingChat = await _context.ChatChats
                .FirstOrDefaultAsync(c =>
                    (c.UserId == request.SenderId && c.SellerId == request.ReceiverId) ||
                    (c.UserId == request.ReceiverId && c.SellerId == request.SenderId),
                    cancellationToken);

            if (existingChat != null)
                return existingChat.Id;

            var newChat = new ChatChat
            {
                Id = Guid.NewGuid(),
                UserId = request.SenderId,
                SellerId = request.ReceiverId,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ChatChats.Add(newChat);
            await _context.SaveChangesAsync(cancellationToken);

            return newChat.Id;
        }

    }
}
