using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ChatUserService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public ChatUserService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatUser>> GetAllUsersAsync()
        {
            return await _context.Set<ChatUser>().ToListAsync();
        }

        public async Task<ChatUser?> GetUserByIdAsync(Guid id)
        {
            return await _context.Set<ChatUser>()
                .Include(u => u.ChatMessages)
                .Include(u => u.ChatChatSellers)
                .Include(u => u.ChatChatUsers)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ChatUser> AddUserAsync(ChatUser user)
        {
            _context.Set<ChatUser>().Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<ChatUser?> UpdateUserAsync(Guid id, ChatUser updated)
        {
            var user = await _context.Set<ChatUser>().FindAsync(id);
            if (user == null) return null;

            user.Name = updated.Name;
            user.Role = updated.Role;
            user.CreatedAt = updated.CreatedAt;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Set<ChatUser>().FindAsync(id);
            if (user == null) return false;

            _context.Set<ChatUser>().Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
