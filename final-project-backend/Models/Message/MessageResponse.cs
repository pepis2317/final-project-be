namespace final_project_backend.Models.Message
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public MessageSender Sender { get; set; }
        public string MessageText {  get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
