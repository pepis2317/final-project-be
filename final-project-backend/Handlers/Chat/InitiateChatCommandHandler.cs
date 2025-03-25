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
            // Cek apakah chat antara user dan seller sudah ada
            var existingChat = await _context.Chats
                .FirstOrDefaultAsync(c =>
                    (c.UserId == request.SenderId && c.SellerId == request.ReceiverId) ||
                    (c.UserId == request.ReceiverId && c.SellerId == request.SenderId),
                    cancellationToken);

            if (existingChat != null)
                return existingChat.Id;

            // Buat chat baru
            var newChat = new Entities.Chat
            {
                Id = Guid.NewGuid(),
                UserId = request.SenderId,
                SellerId = request.ReceiverId,
                UpdatedAt = DateTime.UtcNow
            };

            // Buat ChatUser untuk user
            var userChatUser = new ChatUser
            {
                Id = Guid.NewGuid(),
                ChatId = newChat.Id,
                UserId = request.SenderId,
                Role = "user",
                CreatedAt = DateTime.UtcNow
            };

            // Buat ChatUser untuk seller
            var sellerChatUser = new ChatUser
            {
                Id = Guid.NewGuid(),
                ChatId = newChat.Id,
                SellerId = request.ReceiverId,
                Role = "seller",
                CreatedAt = DateTime.UtcNow
            };

            _context.Chats.Add(newChat);
            _context.ChatUsers.AddRange(userChatUser, sellerChatUser);

            await _context.SaveChangesAsync(cancellationToken);

            return newChat.Id;
        }
    }
}
