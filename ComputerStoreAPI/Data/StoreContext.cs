using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Data {
    public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options) {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
        public required DbSet<Product> Products { get; set; }
    }
}
