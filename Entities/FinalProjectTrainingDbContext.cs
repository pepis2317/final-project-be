using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public partial class FinalProjectTrainingDbContext : DbContext
    {
        public FinalProjectTrainingDbContext() { }

        public FinalProjectTrainingDbContext(DbContextOptions<FinalProjectTrainingDbContext> options)
            : base(options) { }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<ChatChat> ChatChats { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<ChatUser> ChatUsers { get; set; }

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
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__carts__2EF52A27B24DA9E9");

            entity.ToTable("carts");

            entity.Property(e => e.CartId)
                .ValueGeneratedNever()
                .HasColumnName("cart_id");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("completed_at");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__carts__buyer_id__540C7B00");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__cart_ite__5D9A6C6EEBB29208");

            entity.ToTable("cart_items");

            entity.Property(e => e.CartItemId)
                .ValueGeneratedNever()
                .HasColumnName("cart_item_id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__cart___5CA1C101");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__item___5D95E53A");
        });

        modelBuilder.Entity<ChatChat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat_Cha__3214EC07583CD0A7");

            entity.ToTable("Chat_Chats");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Seller).WithMany(p => p.ChatChatSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_Seller");

            entity.HasOne(d => d.User).WithMany(p => p.ChatChatUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_User");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat_Mes__3214EC07A39D0A11");

            entity.ToTable("Chat_Messages");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_Chat");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_Sender");
        });

        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat_Use__3214EC0788B434D5");

            entity.ToTable("Chat_Users");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__item__52020FDDA5F1086C");

            entity.ToTable("item");

                entity.Property(e => e.ItemId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ItemName).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.ItemDesc).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.Thumbnail).HasMaxLength(255).IsUnicode(false);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_item_shop_id");
            });

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("Order_id");
            entity.Property(e => e.BuyerId).HasColumnName("Buyer_id");
            entity.Property(e => e.Confirmed)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("unconfirmed")
                .HasColumnName("confirmed");
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

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("FK__orders__Buyer_id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__orders__Item_id");
            });

            // ProductImage
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);
                entity.ToTable("productImages");

                entity.Property(e => e.ImageId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Image).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.IsPrimary).HasMaxLength(50).IsUnicode(false).HasDefaultValue("false");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__productIm__Item");
            });

            // Shop
            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ShopId);
                entity.ToTable("Shop");

                entity.Property(e => e.ShopId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ShopName).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.Address).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.Description).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shop_owner_id");
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.UserName).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.UserPassword).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.UserEmail).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.UserPhoneNumber).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.UserProfile).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.UserAddress).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.RefreshToken).HasMaxLength(255).IsUnicode(false);
            });

            // ChatUser ↔ User (Many-to-One)
            modelBuilder.Entity<ChatUser>()
                .HasOne<User>()
                .WithMany(u => u.ChatUsers)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chat ↔ ChatUser (User and Seller)
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany(cu => cu.UserChats)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Seller)
                .WithMany(cu => cu.SellerChats)
                .HasForeignKey(c => c.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat ↔ ChatUsers (One Chat has many ChatUsers)
            modelBuilder.Entity<Chat>()
                .HasMany(c => c.ChatUsers)
                .WithOne()
                .HasForeignKey(cu => cu.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message ↔ Chat
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message ↔ ChatUser (Sender)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(cu => cu.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
