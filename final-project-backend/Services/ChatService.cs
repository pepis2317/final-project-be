using Azure.Core;
using Entities;
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
