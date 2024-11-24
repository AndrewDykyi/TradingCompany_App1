using DAL.Interface;
using System.Data.SqlClient;

namespace DAL.Concrete
{
    public class ProductDal : IProductDal
    {
        private readonly SqlConnection _connection;

        public ProductDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ProductID, ProductName, CategoryID, Price, Blocked FROM Products";
                await _connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                List<ProductDto> products = new List<ProductDto>();
                while (await reader.ReadAsync())
                {
                    products.Add(new ProductDto
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryID = reader["CategoryID"] as int?,
                        Price = Convert.ToDecimal(reader["Price"]),
                        Blocked = Convert.ToBoolean(reader["Blocked"])
                    });
                }

                _connection.Close();
                return products;
            }
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ProductID, ProductName, CategoryID, Price, Blocked FROM Products WHERE ProductID = @productId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("productId", id);

                await _connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                ProductDto product = null;
                if (await reader.ReadAsync())
                {
                    product = new ProductDto
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryID = reader["CategoryID"] as int?,
                        Price = Convert.ToDecimal(reader["Price"]),
                        Blocked = Convert.ToBoolean(reader["Blocked"])
                    };
                }
                _connection.Close();

                if (product == null)
                    throw new NullReferenceException("Invalid Product ID");

                return product;
            }
        }

        public async Task InsertAsync(ProductDto product)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Products (ProductName, CategoryID, Price, Blocked) OUTPUT inserted.ProductID VALUES (@name, @categoryId, @price, @blocked)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("name", product.ProductName);
                command.Parameters.AddWithValue("categoryId", (object)product.CategoryID ?? DBNull.Value);
                command.Parameters.AddWithValue("price", product.Price);
                command.Parameters.AddWithValue("blocked", product.Blocked);

                await _connection.OpenAsync();
                product.ProductID = Convert.ToInt32(await command.ExecuteScalarAsync());
                _connection.Close();
            }
        }

        public async Task UpdateAsync(ProductDto product)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Products SET ProductName = @name, CategoryID = @categoryId, Price = @price, Blocked = @blocked WHERE ProductID = @productId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("name", product.ProductName);
                command.Parameters.AddWithValue("categoryId", (object)product.CategoryID ?? DBNull.Value);
                command.Parameters.AddWithValue("price", product.Price);
                command.Parameters.AddWithValue("blocked", product.Blocked);
                command.Parameters.AddWithValue("productId", product.ProductID);

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                _connection.Close();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Products WHERE ProductID = @productId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("productId", id);

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                _connection.Close();
            }
        }
    }
}