using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid SellerId { get; set; }
    public string? LastMessage { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ChatUser User { get; set; }
    public ChatUser Seller { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>(); // Tambahkan ini
}