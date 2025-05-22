using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Data {
    public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options) {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // default schema is core
            modelBuilder.HasDefaultSchema("core");
            // we set schemas (sales) for the tables
            modelBuilder.Entity<Product>().ToTable("Product", "core");
            modelBuilder.Entity<Category>().ToTable("Category", "core");
            modelBuilder.Entity<Customer>().ToTable("Customer", "core");
            modelBuilder.Entity<Order>().ToTable("Order", "sales");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem", "sales");

            modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });

            // setting precision for decimal properties
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            // adding indexes for performance
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CategoryId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CustomerId);
        }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
    }
}
