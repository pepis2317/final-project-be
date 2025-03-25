using System;
using System.Collections.Generic;

namespace Entities;

public partial class ChatChat
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid SellerId { get; set; }

    public string? LastMessage { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public virtual ChatUser Seller { get; set; } = null!;

    public virtual ChatUser User { get; set; } = null!;
}
