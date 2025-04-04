using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessage(string chatId, string senderId, string senderName, string message)
    {
        var messageId = Guid.NewGuid().ToString(); 
        var timestamp = DateTime.UtcNow;

        var messageObject = new
        {
            id = messageId,
            sender = new { id = senderId, name = senderName },
            messageText = message,
            createdAt = timestamp
        };

        await Clients.Group(chatId).SendAsync("ReceiveMessage", messageObject);
    }

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }
}
