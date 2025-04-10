﻿using System;
using System.Collections.Generic;

namespace Entities;

public partial class Item
{
    public Guid ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemDesc { get; set; }

    public Guid ShopId { get; set; }

    public int? Quantity { get; set; }

    public int? HargaPerItem { get; set; }

    public string? Thumbnail { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual Shop Shop { get; set; } = null!;
}
