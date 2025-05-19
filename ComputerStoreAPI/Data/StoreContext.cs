using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Data {
    public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options) {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
        }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
    }
}
