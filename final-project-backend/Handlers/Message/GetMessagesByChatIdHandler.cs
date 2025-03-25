using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using final_project_backend.Queries.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Alias untuk menghindari bentrok dengan namespace
using MessageEntity = Entities.Message;

namespace final_project_backend.Handlers.Message
{
    public class GetMessagesByChatIdHandler : IRequestHandler<GetMessagesByChatIdQuery, List<MessageEntity>>
    {
        private readonly FinalProjectTrainingDbContext _context;

        public GetMessagesByChatIdHandler(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<MessageEntity>> Handle(GetMessagesByChatIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatId == request.ChatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
