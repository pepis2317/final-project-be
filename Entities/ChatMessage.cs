using System;
using System.Collections.Generic;

namespace Entities;

public partial class ChatMessage
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid SenderId { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ChatChat Chat { get; set; } = null!;

    public virtual ChatUser Sender { get; set; } = null!;
}
