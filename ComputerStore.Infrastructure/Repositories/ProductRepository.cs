using System.Data;
using ComputerStore.Application.Repositories;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;
using Dapper;

namespace ComputerStore.Infrastructure.Repositories {
    public class ProductRepository : IProductRepository {
        private readonly IDapperContext _context;
        public ProductRepository(IDapperContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetAllAsync() {
            const string sql = "SELECT * FROM core.Product";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<Product>(sql);
        }

        public async Task<Product?> GetByIdAsync(int id) {
            const string sql = "SELECT * FROM core.Product WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Product product) {
            const string sql = @"
                INSERT INTO core.Product (Name, Description, Price, Stock, CategoryId)
                VALUES (@Name, @Description, @Price, @Stock, @CategoryId);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            using var conn = _context.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, product);
        }

        public async Task UpdateAsync(Product product) {
            const string sql = @"
                UPDATE core.Product
                SET Name=@Name, Description=@Description,
                    Price=@Price, Stock=@Stock, CategoryId=@CategoryId
                WHERE Id=@Id";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, product);
        }

        public async Task DeleteAsync(int id) {
            const string sql = "DELETE FROM core.Product WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
