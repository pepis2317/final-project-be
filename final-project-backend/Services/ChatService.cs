using Azure.Core;
using Entities;
using final_project_backend.Commands.Chat;
using final_project_backend.Models.Chat;
using final_project_backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Services
{
    public class ChatService
    {
        private readonly FinalProjectTrainingDbContext _context;
        private readonly BlobStorageService _blobStorageService;

        public ChatService(FinalProjectTrainingDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;

        }
        private async Task<string?> PfpHelper(string? fileName)
        {
            string? imageUrl = await _blobStorageService.GetTemporaryImageUrl(fileName, "user-pfps");
            return imageUrl;
        }
        public async Task<Guid> InitializeChat(InitiateChatCommand request)
        {
            var sender = await _context.ChatUsers.FirstOrDefaultAsync(q=>q.Id==request.SenderId);
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

            var receiver = await _context.ChatUsers.FirstOrDefaultAsync(q=>q.Id==request.ReceiverId);
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

            await _context.SaveChangesAsync();

            var existingChat = await _context.ChatChats
                .FirstOrDefaultAsync(c =>
                    (c.UserId == request.SenderId && c.SellerId == request.ReceiverId) ||
                    (c.UserId == request.ReceiverId && c.SellerId == request.SenderId));

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
            await _context.SaveChangesAsync();
            return newChat.Id;
        }
        public async Task<List<ChatResponse>> GetChatsByUserId(Guid UserId)
        {
            var chats = await _context.ChatChats.Include(c => c.User).Include(c => c.Seller).Where(c => c.UserId == UserId || c.SellerId == UserId).OrderByDescending(c => c.UpdatedAt).ToListAsync();
            var result = new List<ChatResponse>();
            foreach(var chat in chats)
            {
                var user = chat.User.Id != UserId? chat.User : chat.Seller;

                var userData = await _context.Users.FirstOrDefaultAsync(q=>q.UserId == user.Id);
                var pfp = userData==null?"":await PfpHelper(userData.UserProfile);

                var userImChattingTo = new ChatUserModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Avatar = pfp
                };
                result.Add(new ChatResponse
                {
                    Id = chat.Id,
                    User = userImChattingTo,
                    LastMessage = chat.LastMessage,
                    UpdatedAt = chat.UpdatedAt
                });
            }
            return result;

        }
        public async Task<List<ChatChat>> GetAllChatsAsync()
        {
            return await _context.Set<ChatChat>().ToListAsync();
        }

        public async Task<ChatChat?> GetChatByIdAsync(Guid id)
        {
            return await _context.Set<ChatChat>().FindAsync(id);
        }

        public async Task<ChatChat> CreateChatAsync(ChatChat chat)
        {
            _context.Set<ChatChat>().Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<bool> DeleteChatAsync(Guid id)
        {
            var chat = await GetChatByIdAsync(id);
            if (chat == null) return false;

            _context.Set<ChatChat>().Remove(chat);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
