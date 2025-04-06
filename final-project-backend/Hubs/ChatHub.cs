using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace final_project_backend.Hubs;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> _connections = new();

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[SignalR] Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"[SignalR] Client disconnected: {Context.ConnectionId}");

        var chatId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (chatId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
            _connections.TryRemove(chatId, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string chatId, string senderId, string senderName, string message)
    {
        if (string.IsNullOrWhiteSpace(chatId) || string.IsNullOrWhiteSpace(message)) return;

        Console.WriteLine($"[SignalR] Message sent in Chat {chatId}: {senderName}: {message}");

        var messageObject = new
        {
            id = Guid.NewGuid().ToString(),
            sender = new { id = senderId, name = senderName },
            messageText = message,
            createdAt = DateTime.UtcNow
        };

        await Clients.Group(chatId).SendAsync("ReceiveMessage", messageObject);
    }

    public async Task JoinChat(string chatId)
    {
        if (string.IsNullOrWhiteSpace(chatId)) return;

        Console.WriteLine($"[SignalR] {Context.ConnectionId} joining Chat {chatId}");

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        _connections[chatId] = Context.ConnectionId;
    }
}
