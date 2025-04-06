using System;

namespace final_project_backend.Models.Chat
{
    public class CreateChatModel
    {
        public Guid UserId { get; set; }
        public Guid SellerId { get; set; }
    }
}
