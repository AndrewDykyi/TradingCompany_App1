using System.Data.SqlClient;
using DAL.Interface;

namespace DAL.Concrete
{
    public class CategoryManagementDal : ICategoryManagementDal
    {
        private readonly SqlConnection _connection;

        public CategoryManagementDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<CategoryManagementDto> GetAll()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ManagementID, UserID, CategoryID, ActionID, ActionDate FROM CategoryManagement";
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<CategoryManagementDto> managements = new List<CategoryManagementDto>();
                while (reader.Read())
                {
                    managements.Add(new CategoryManagementDto
                    {
                        ManagementID = Convert.ToInt32(reader["ManagementID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        ActionID = reader["ActionID"] != DBNull.Value
                                   ? Convert.ToInt32(reader["ActionID"])
                                   : 0,
                        ActionDate = reader["ActionDate"] != DBNull.Value
                                     ? Convert.ToDateTime(reader["ActionDate"])
                                     : DateTime.MinValue
                    });
                }

                _connection.Close();
                return managements;
            }
        }

        public CategoryManagementDto GetById(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ManagementID, UserID, CategoryID, ActionID, ActionDate FROM CategoryManagement WHERE ManagementID = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("id", id);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                CategoryManagementDto management = null;
                if (reader.Read())
                {
                    management = new CategoryManagementDto
                    {
                        ManagementID = Convert.ToInt32(reader["ManagementID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        ActionID = reader["ActionID"] != DBNull.Value
                                   ? Convert.ToInt32(reader["ActionID"])
                                   : 0,
                        ActionDate = reader["ActionDate"] != DBNull.Value
                                     ? Convert.ToDateTime(reader["ActionDate"])
                                     : DateTime.MinValue
                    };
                }

                _connection.Close();

                if (management == null)
                    throw new NullReferenceException("Invalid Management ID");

                return management;
            }
        }

        public void Insert(CategoryManagementDto management)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO CategoryManagement (UserID, CategoryID, ActionID, ActionDate) OUTPUT inserted.ManagementID VALUES (@UserID, @CategoryID, @ActionID, @ActionDate)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("UserID", management.UserID);
                command.Parameters.AddWithValue("CategoryID", management.CategoryID);
                command.Parameters.AddWithValue("ActionID", (object?)management.ActionID ?? DBNull.Value);
                command.Parameters.AddWithValue("ActionDate", (object?)management.ActionDate ?? DBNull.Value);

                _connection.Open();
                management.ManagementID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
            }
        }

        public void Update(CategoryManagementDto management)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE CategoryManagement SET UserID = @UserID, CategoryID = @CategoryID, ActionID = @ActionID, ActionDate = @ActionDate WHERE ManagementID = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("UserID", management.UserID);
                command.Parameters.AddWithValue("CategoryID", management.CategoryID);
                command.Parameters.AddWithValue("ActionID", (object?)management.ActionID ?? DBNull.Value);
                command.Parameters.AddWithValue("ActionDate", (object?)management.ActionDate ?? DBNull.Value);
                command.Parameters.AddWithValue("id", management.ManagementID);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CategoryManagement WHERE ManagementID = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("id", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}