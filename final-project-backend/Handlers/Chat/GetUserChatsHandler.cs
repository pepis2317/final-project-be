using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using final_project_backend.Queries.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Handlers.Chat
{
    public class GetUserChatsHandler : IRequestHandler<GetUserChatsQuery, List<Entities.Chat>>
    {
        private readonly FinalProjectTrainingDbContext _context;

        public GetUserChatsHandler(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Chat>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _context.Chats
                .Include(c => c.User)
                .Include(c => c.Seller)
                .Where(c => c.UserId == request.UserId || c.SellerId == request.UserId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync(cancellationToken);

            return chats;
        }
    }
}
