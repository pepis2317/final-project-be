using MediatR;
using System;

namespace final_project_backend.Models.Item
{
    public class InitiateChatCommand : IRequest<Guid>
    {
        public Guid InitiatorUserId { get; set; }
        public Guid TargetUserId { get; set; }
    }

}
