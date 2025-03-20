using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public partial class FinalProjectTrainingDbContext : DbContext
{
    public FinalProjectTrainingDbContext()
    {
    }

    public FinalProjectTrainingDbContext(DbContextOptions<FinalProjectTrainingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=tcp:final-project-training.database.windows.net,1433;Initial Catalog=final-project-training-db;Persist Security Info=False;User ID=admin_user;Password=Password11!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__item__52020FDDA5F1086C");

            entity.ToTable("item");

            entity.HasIndex(e => e.ItemName, "UQ__item__ACA52A97D059E7C4").IsUnique();

            entity.Property(e => e.ItemId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("item_id");
            entity.Property(e => e.HargaPerItem)
                .HasDefaultValue(0)
                .HasColumnName("harga_per_item");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("item_desc");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("item_name");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.ShopId).HasColumnName("shop_id");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("thumbnail");

            entity.HasOne(d => d.Shop).WithMany(p => p.Items)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK_item_shop_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__F1FF845341271034");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("Order_id");
            entity.Property(e => e.BuyerId).HasColumnName("Buyer_id");
            entity.Property(e => e.ItemId).HasColumnName("Item_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Order_date");
            entity.Property(e => e.OrderDetails)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Order_details");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.TotalHarga)
                .HasDefaultValue(0)
                .HasColumnName("total_harga");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__orders__Buyer_id__0D7A0286");

            entity.HasOne(d => d.Item).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__orders__Item_id__0E6E26BF");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__productI__3CAC59117B8AA3BA");

            entity.ToTable("productImages");

            entity.Property(e => e.ImageId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("Image_id");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsPrimary)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("false")
                .IsFixedLength()
                .HasColumnName("Is_primary");
            entity.Property(e => e.ItemId).HasColumnName("Item_id");

            entity.HasOne(d => d.Item).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__productIm__Item___1BC821DD");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shop__5C0F82B8317FE30A");

            entity.ToTable("Shop");

            entity.HasIndex(e => e.ShopName, "UQ__Shop__F265FA7AA61DB603").IsUnique();

            entity.Property(e => e.ShopId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("Shop_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OwnerId).HasColumnName("Owner_id");
            entity.Property(e => e.Rating).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShopName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Shop_name");

            entity.HasOne(d => d.Owner).WithMany(p => p.Shops)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_shop_owner_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F0EA657DD");

            entity.ToTable("users");

            entity.HasIndex(e => e.UserEmail, "UQ__users__B0FBA212E391237E").IsUnique();

            entity.HasIndex(e => e.UserPhoneNumber, "UQ__users__D5D775E81DFBDDF5").IsUnique();

            entity.HasIndex(e => e.UserName, "unique_user_name").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpiryTime)
                .HasColumnType("datetime")
                .HasColumnName("refresh_token_expiry_time");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_address");
            entity.Property(e => e.UserBalance)
                .HasDefaultValue(0)
                .HasColumnName("user_balance");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_email");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_name");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_password");
            entity.Property(e => e.UserPhoneNumber)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("user_phone_number");
            entity.Property(e => e.UserProfile)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_profile");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
