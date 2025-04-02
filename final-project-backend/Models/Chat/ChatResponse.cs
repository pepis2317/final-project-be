namespace final_project_backend.Models.Chat
{
    public class ChatResponse
    {
        public Guid Id {  get; set; }
        public ChatUserModel User {  get; set; }
        public string LastMessage { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
