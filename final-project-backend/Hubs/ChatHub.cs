using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessage(Guid chatId, string senderId, string messageText)
    {
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", senderId, messageText, DateTime.UtcNow);
    }

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}
