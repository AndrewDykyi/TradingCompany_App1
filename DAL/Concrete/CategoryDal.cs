using DAL.Interface;
using System.Data.SqlClient;

namespace DAL.Concrete
{
    public class CategoryDal : ICategoryDal
    {
        private readonly SqlConnection _connection;

        public CategoryDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<CategoryDto> GetAll()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT CategoryID, CategoryName, IsDeleted FROM Categories";
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<CategoryDto> categories = new List<CategoryDto>();
                while (reader.Read())
                {
                    categories.Add(new CategoryDto
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        IsDeleted = Convert.ToBoolean(reader["IsDeleted"])
                    });
                }

                _connection.Close();
                return categories;
            }
        }
        public CategoryDto GetById(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT CategoryID, CategoryName, IsDeleted FROM Categories WHERE CategoryID = @categoryId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("categoryId", id);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                CategoryDto category = null;
                if (reader.Read())
                {
                    category = new CategoryDto
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        IsDeleted = Convert.ToBoolean(reader["IsDeleted"])
                    };
                }
                _connection.Close();

                if (category == null)
                    throw new NullReferenceException("Invalid Category ID");

                return category;
            }
        }

        public void Insert(CategoryDto category)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Categories (CategoryName, IsDeleted) OUTPUT inserted.CategoryID VALUES (@name, @isDeleted)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("name", category.CategoryName);
                command.Parameters.AddWithValue("isDeleted", category.IsDeleted);

                _connection.Open();
                category.CategoryID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
            }
        }

        public void Update(CategoryDto category)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Categories SET CategoryName = @name, IsDeleted = @isDeleted WHERE CategoryID = @categoryId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("name", category.CategoryName);
                command.Parameters.AddWithValue("isDeleted", category.IsDeleted);
                command.Parameters.AddWithValue("categoryId", category.CategoryID);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Categories WHERE CategoryID = @categoryId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("categoryId", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}