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

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<ChatUser> ChatUsers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:final-project-training.database.windows.net,1433;Initial Catalog=final-project-training-db;Persist Security Info=False;User ID=admin_user;Password=Password11!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Custom table names
            modelBuilder.Entity<Chat>().ToTable("Chat_Chats");
            modelBuilder.Entity<ChatUser>().ToTable("Chat_Users");
            modelBuilder.Entity<Message>().ToTable("Chat_Messages");

            // Item
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItemId);
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

                entity.Property(e => e.OrderId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.OrderDetails).HasMaxLength(255).IsUnicode(false);

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
