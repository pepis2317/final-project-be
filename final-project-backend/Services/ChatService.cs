using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ChatService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public ChatService(FinalProjectTrainingDbContext context)
        {
            _context = context;
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
