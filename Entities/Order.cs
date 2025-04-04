using System;
using System.Collections.Generic;

namespace Entities;

public partial class Order
{
    public Guid OrderId { get; set; }

    public string? OrderDetails { get; set; }

    public DateTime? OrderDate { get; set; }

    public Guid BuyerId { get; set; }

    public Guid ItemId { get; set; }

    public int? Quantity { get; set; }

    public int? TotalHarga { get; set; }

    public string? Confirmed { get; set; }

    public virtual User Buyer { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
