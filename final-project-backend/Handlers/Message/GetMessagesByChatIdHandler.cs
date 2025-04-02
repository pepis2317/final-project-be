using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using final_project_backend.Queries.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Alias untuk menghindari bentrok dengan namespace
using MessageEntity = Entities.ChatMessage;
using Services;
using final_project_backend.Models.Message;

namespace final_project_backend.Handlers.Message
{
    public class GetMessagesByChatIdHandler : IRequestHandler<GetMessagesByChatIdQuery, List<MessageResponse>>
    {
        private readonly MessageService _service;

        public GetMessagesByChatIdHandler(MessageService service)
        {
            _service = service;
        }

        public async Task<List<MessageResponse>> Handle(GetMessagesByChatIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _service.GetMessagesByChatIdAsync(request.ChatId);
            return data;
        }
    }
}
