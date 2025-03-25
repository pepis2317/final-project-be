using Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace final_project_backend.Queries.Messages
{
    public class GetMessagesByChatIdQuery : IRequest<List<Message>>
    {
        public Guid ChatId { get; }

        public GetMessagesByChatIdQuery(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
