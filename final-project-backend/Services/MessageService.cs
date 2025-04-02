using Azure.Core;
using Entities;
using final_project_backend.Commands.Message;
using final_project_backend.Models.Message;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Services
{
    public class MessageService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public MessageService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<MessageResponse>> GetMessagesByChatIdAsync(Guid chatId)
        {
            var chats = await _context.ChatMessages.Include(m => m.Sender).Where(m => m.ChatId == chatId).OrderBy(m => m.CreatedAt).ToListAsync();
            var result = new List<MessageResponse>();
            foreach(var chat in chats)
            {
                var sender = new MessageSender
                {
                    Id = chat.SenderId,
                    Name = chat.Sender.Name
                };
                result.Add(new MessageResponse
                {
                    Id = chat.Id,
                    ChatId = chat.ChatId,
                    Sender = sender,
                    MessageText = chat.MessageText,
                    CreatedAt = chat.CreatedAt
                });
            }
            return result;

        }

        public async Task<ChatMessage> CreateMessage(CreateMessageCommand request)
        {
            var message = new ChatMessage
            {
                ChatId = request.ChatId,
                SenderId = request.SenderId,
                MessageText = request.MessageText,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);

            var chat = await _context.ChatChats.FirstOrDefaultAsync(c => c.Id == request.ChatId);
            if (chat != null)
            {
                chat.LastMessage = request.MessageText;
                chat.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            await _context.Entry(message).Reference(m => m.Sender).LoadAsync();
            var sender = new MessageSender
            {
                Id = message.SenderId,
                Name = message.Sender.Name,
            };
            return message;
        }
        public async Task<ChatMessage> CreateMessageAsync(ChatMessage message)
        {
            _context.Set<ChatMessage>().Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
