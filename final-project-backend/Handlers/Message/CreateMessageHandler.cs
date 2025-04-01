using final_project_backend.Commands.Message;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

// Alias untuk hindari konflik nama namespace vs class
using MessageEntity = Entities.ChatMessage;

namespace final_project_backend.Handlers.Message
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageCommand, MessageEntity>
    {
        private readonly FinalProjectTrainingDbContext _context;

        public CreateMessageHandler(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<MessageEntity> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new MessageEntity
            {
                ChatId = request.ChatId,
                SenderId = request.SenderId,
                MessageText = request.MessageText,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);

            var chat = await _context.ChatChats.FirstOrDefaultAsync(c => c.Id == request.ChatId, cancellationToken);
            if (chat != null)
            {
                chat.LastMessage = request.MessageText;
                chat.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);

            await _context.Entry(message).Reference(m => m.Sender).LoadAsync(cancellationToken);

            return message;
        }
    }
}
