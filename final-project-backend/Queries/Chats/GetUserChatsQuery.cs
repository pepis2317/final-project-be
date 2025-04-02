using Entities;
using final_project_backend.Models.Chat;
using MediatR;
using System;
using System.Collections.Generic;

namespace final_project_backend.Queries.Chat
{
    public class GetUserChatsQuery : IRequest<List<ChatResponse>>
    {
        public Guid UserId { get; set; }

        public GetUserChatsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
