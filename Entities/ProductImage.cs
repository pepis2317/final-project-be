using System;
using System.Collections.Generic;

namespace Entities;

public partial class ProductImage
{
    public Guid ImageId { get; set; }

    public Guid ItemId { get; set; }

    public string Image { get; set; } = null!;

    public string IsPrimary { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}