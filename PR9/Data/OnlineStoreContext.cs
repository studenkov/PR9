using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Data;

public partial class OnlineStoreContext : DbContext
{
    public OnlineStoreContext()
    {
    }

    public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReservation> ProductReservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carts__701794908D4F608E");

            entity.HasIndex(e => e.UserId, "UQ__Carts__B9BF3306CB970098").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID_cart");
            entity.Property(e => e.UserId).HasColumnName("user_ID");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__2D2131968DB0536B");

            entity.Property(e => e.Id).HasColumnName("ID_cart_item");
            entity.Property(e => e.CartId).HasColumnName("cart_ID");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__AC5CA40E8BDD1F50");

            entity.HasIndex(e => e.Title, "UQ__Categori__E52A1BB39E60FB43").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID_category");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__89BFCD5992DC2904");

            entity.Property(e => e.Id).HasColumnName("ID_order");
            entity.Property(e => e.DateOrder).HasColumnName("date_order");
            entity.Property(e => e.UserId).HasColumnName("user_ID");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__5B1127FF2D9F7554");

            entity.Property(e => e.Id).HasColumnName("ID_order_item");
            entity.Property(e => e.OrderId).HasColumnName("order_ID");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__FD7FEC2320C32751");

            entity.Property(e => e.Id).HasColumnName("ID_product");
            entity.Property(e => e.CategoryId).HasColumnName("category_ID");
            entity.Property(e => e.StockQuantity).HasDefaultValue(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_path");
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        modelBuilder.Entity<ProductReservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User).WithMany(u => u.ProductReservations).HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__45DFFBDBD9D08645");

            entity.HasIndex(e => e.Title, "UQ__Roles__E52A1BB3848F6567").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID_role");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__D7B4671ED39BD889");

            entity.HasIndex(e => e.LoginUser, "UQ__Users__12D0D37FB4B53D8F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID_user");
            entity.Property(e => e.LoginUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("login_user");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_ID");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
