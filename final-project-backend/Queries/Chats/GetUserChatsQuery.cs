using Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace final_project_backend.Queries.Chat
{
    public class GetUserChatsQuery : IRequest<List<Entities.ChatChat>>
    {
        public Guid UserId { get; set; }

        public GetUserChatsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
