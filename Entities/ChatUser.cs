using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Entities;

public partial class ChatUser
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]

    public virtual ICollection<ChatChat> ChatChatSellers { get; set; } = new List<ChatChat>();
    public virtual ICollection<ChatChat> ChatChatUsers { get; set; } = new List<ChatChat>();
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();


}
