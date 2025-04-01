using MediatR;
using MessageEntity = Entities.ChatMessage;

namespace final_project_backend.Commands.Message
{
    public class CreateMessageCommand : IRequest<MessageEntity>
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; } = string.Empty;
    }
}
