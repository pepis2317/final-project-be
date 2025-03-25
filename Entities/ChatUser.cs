using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entities;

public class ChatUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public Guid SellerId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; }
    public User User { get; set; }

    public ICollection<Chat> UserChats { get; set; }
    public ICollection<Chat> SellerChats { get; set; }
    public ICollection<Message> Messages { get; set; }
}
