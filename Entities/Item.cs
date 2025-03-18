using System;
using System.Collections.Generic;

namespace Entities;

public partial class Item
{
    public Guid ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemDesc { get; set; }

    public Guid ShopId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Shop Shop { get; set; } = null!;
}
