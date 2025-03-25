using System;
using System.Collections.Generic;

namespace Entities;

public partial class ChatUser
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ChatChat> ChatChatSellers { get; set; } = new List<ChatChat>();

    public virtual ICollection<ChatChat> ChatChatUsers { get; set; } = new List<ChatChat>();

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
