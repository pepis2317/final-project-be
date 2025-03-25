using System;
using System.Collections.Generic;

namespace Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public int? UserBalance { get; set; }

    public string? UserProfile { get; set; }

    public string? UserPhoneNumber { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserAddress { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
