using System;
using System.Collections.Generic;
using MediatR;
using MessageEntity = Entities.ChatMessage;

namespace final_project_backend.Queries.Messages
{
    public class GetMessagesByChatIdQuery : IRequest<List<MessageEntity>>
    {
        public Guid ChatId { get; set; }

        public GetMessagesByChatIdQuery(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
