using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class MessageService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public MessageService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetMessagesByChatIdAsync(Guid chatId)
        {
            return await _context.Set<ChatMessage>()
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<ChatMessage> CreateMessageAsync(ChatMessage message)
        {
            _context.Set<ChatMessage>().Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
