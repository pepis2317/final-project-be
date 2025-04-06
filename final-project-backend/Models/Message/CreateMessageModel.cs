using System;

namespace final_project_backend.Models.Message
{
    public class CreateMessageModel
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; } = string.Empty;
    }
}
