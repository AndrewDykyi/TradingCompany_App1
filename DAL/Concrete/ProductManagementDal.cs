using System.Data.SqlClient;
using DAL.Interface;

namespace DAL.Concrete
{
    public class ProductManagementDal : IProductManagementDal
    {
        private readonly SqlConnection _connection;

        public ProductManagementDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<ProductManagementDto> GetAll()
        {
            var managements = new List<ProductManagementDto>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ManagementID, UserID, ProductID, ActionID FROM ProductManagement";
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        managements.Add(new ProductManagementDto
                        {
                            ManagementID = Convert.ToInt32(reader["ManagementID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ActionID = Convert.ToInt32(reader["ActionID"])
                        });
                    }
                }
                _connection.Close();
            }
            return managements;
        }

        public ProductManagementDto GetById(int id)
        {
            ProductManagementDto management = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ManagementID, UserID, ProductID, ActionID FROM ProductManagement WHERE ManagementID = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("id", id);

                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        management = new ProductManagementDto
                        {
                            ManagementID = Convert.ToInt32(reader["ManagementID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ActionID = Convert.ToInt32(reader["ActionID"])
                        };
                    }
                }
                _connection.Close();
            }

            if (management == null)
                throw new NullReferenceException("Invalid Management ID");

            return management;
        }

        public void Insert(ProductManagementDto management)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO ProductManagement (UserID, ProductID, ActionID) OUTPUT inserted.ManagementID VALUES (@UserID, @ProductID, @ActionID)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("UserID", management.UserID);
                command.Parameters.AddWithValue("ProductID", management.ProductID);
                command.Parameters.AddWithValue("ActionID", management.ActionID);

                _connection.Open();
                management.ManagementID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
            }
        }

        public void Update(ProductManagementDto management)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE ProductManagement SET UserID = @UserID, ProductID = @ProductID, ActionID = @ActionID WHERE ManagementID = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("UserID", management.UserID);
                command.Parameters.AddWithValue("ProductID", management.ProductID);
                command.Parameters.AddWithValue("ActionID", management.ActionID);
                command.Parameters.AddWithValue("id", management.ManagementID);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM ProductManagement WHERE ManagementID = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("id", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}