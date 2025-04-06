using System;
using System.Collections.Generic;
using final_project_backend.Models.Message;
using MediatR;
using MessageEntity = Entities.ChatMessage;

namespace final_project_backend.Queries.Messages
{
    public class GetMessagesByChatIdQuery : IRequest<List<MessageResponse>>
    {
        public Guid ChatId { get; set; }

        public GetMessagesByChatIdQuery(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
