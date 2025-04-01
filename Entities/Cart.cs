using System;
using System.Collections.Generic;

namespace Entities;

public partial class Cart
{
    public Guid CartId { get; set; }

    public Guid BuyerId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual User Buyer { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}