using Dapper;
using MySql.Data.MySqlClient;
using Product_api.Model;

namespace Product_api.Repository
{
    
        public class ProductRepository : IProductRepository
        {
            private readonly string _connectionString;

            public ProductRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("MySQLConnection");
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                using var connection = new MySqlConnection(_connectionString);
                return await connection.QueryAsync<Product>("SELECT * FROM Products");
            }

            public async Task<Product> GetByIdAsync(int id)
            {
                using var connection = new MySqlConnection(_connectionString);
                return await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
            }

            public async Task<int> InsertAsync(Product product)
            {
                using var connection = new MySqlConnection(_connectionString);
                var sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT LAST_INSERT_ID();";
                return await connection.ExecuteScalarAsync<int>(sql, product);
            }

            public async Task<bool> UpdateAsync(Product product)
            {
                using var connection = new MySqlConnection(_connectionString);
                var sql = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
                return await connection.ExecuteAsync(sql, product) > 0;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                using var connection = new MySqlConnection(_connectionString);
                var sql = "DELETE FROM Products WHERE Id = @Id";
                return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
            }
        }

    }

