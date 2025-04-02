using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using final_project_backend.Queries.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services;
using final_project_backend.Models.Chat;

namespace Handlers.Chat
{
    public class GetUserChatsHandler : IRequestHandler<GetUserChatsQuery, List<ChatResponse>>
    {
        private readonly ChatService _service;

        public GetUserChatsHandler(ChatService service)
        {
            _service = service;
        }

        public async Task<List<ChatResponse>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            var data = await _service.GetChatsByUserId(request.UserId);

            return data;
        }
    }
}
