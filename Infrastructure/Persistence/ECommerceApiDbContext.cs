using System.Net.NetworkInformation;
using System.Security.Cryptography;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure;

public class ECommerceApiDbContext : DbContext {
    public ECommerceApiDbContext(DbContextOptions<ECommerceApiDbContext> options) : base(options) {

    }
    public DbSet<Cart> CartTable {get; set;}
    public DbSet<CartItem> CartItemTable {get; set;}
    public DbSet<Category> CategoryTable {get; set;}
    public DbSet<Order> OrderTable {get; set;}
    public DbSet<OrderItem> OrderItemTable {get; set;}
    public DbSet<Product> ProductTable {get; set;}
    public DbSet<User> UserTable {get; set;}
    public DbSet<RefreshTokenModel> RefreshTokenTable {get; set;}

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>()
            .HavePrecision(10, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Product>().HasIndex(p => p.Name);
        modelBuilder.Entity<Product>().HasIndex(p => p.Description);

        modelBuilder.Entity<Cart>().HasIndex(c => c.UserId).IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.OrderRisuto)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.CartItemRisuto)
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderItemRisuto)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .HasMany(p => p.CategoryRisuto)
            .WithMany(c => c.ProductRisuto)
            .UsingEntity<Dictionary<string, object>>(
                "ProductCategory",
                j => j
                    .HasOne<Category>()
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade),
                    
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade),
                    
                j => {
                    j.HasKey("ProductId", "CategoryId");
                    j.ToTable("ProductCategoryTable");
                }
            );

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.CartItemRisuto)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .IsRequired();

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .IsRequired();

        modelBuilder.Entity<Order>().Property(o => o.OrderStatus).IsRequired();

        modelBuilder.Entity<Order>()
            .ToTable(t => t.HasCheckConstraint("CK_OrderTable_OrderStatus", "OrderStatus IN (1,2,3,4,5)"));
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItemRisuto)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>().HasKey(ci => new {ci.ProductId, ci.CartId});

        modelBuilder.Entity<OrderItem>().HasKey(oi => new {oi.ProductId, oi.OrderId});


    }
}