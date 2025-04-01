using System;
using System.Collections.Generic;

namespace Entities;

public partial class Shop
{
    public Guid ShopId { get; set; }

    public string ShopName { get; set; } = null!;

    public Guid OwnerId { get; set; }

    public string? Description { get; set; }

    public decimal Rating { get; set; }

    public string Address { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual User Owner { get; set; } = null!;
}