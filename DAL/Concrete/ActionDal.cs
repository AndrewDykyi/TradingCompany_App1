using System.Data.SqlClient;
using DAL.Interface;

namespace DAL.Concrete
{
    public class ActionDal : IActionDal
    {
        private readonly SqlConnection _connection;

        public ActionDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<ActionDto> GetAll()
        {
            var actions = new List<ActionDto>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ActionID, ActionName FROM Actions";
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actions.Add(new ActionDto
                        {
                            ActionID = Convert.ToInt32(reader["ActionID"]),
                            ActionName = reader["ActionName"].ToString()
                        });
                    }
                }
                _connection.Close();
            }
            return actions;
        }

        public ActionDto GetById(int id)
        {
            ActionDto action = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ActionID, ActionName FROM Actions WHERE ActionID = @id";
                command.Parameters.AddWithValue("id", id);

                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        action = new ActionDto
                        {
                            ActionID = Convert.ToInt32(reader["ActionID"]),
                            ActionName = reader["ActionName"].ToString()
                        };
                    }
                }
                _connection.Close();
            }

            if (action == null)
                throw new NullReferenceException("Invalid Action ID");

            return action;
        }

        public void Insert(ActionDto action)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Actions (ActionName) VALUES (@actionName)";
                command.Parameters.AddWithValue("actionName", action.ActionName);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Update(ActionDto action)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Actions SET ActionName = @actionName WHERE ActionID = @id";
                command.Parameters.AddWithValue("actionName", action.ActionName);
                command.Parameters.AddWithValue("id", action.ActionID);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Actions WHERE ActionID = @id";
                command.Parameters.AddWithValue("id", id);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}