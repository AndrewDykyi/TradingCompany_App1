using DAL.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Concrete
{
    public class UserDal : IUserDal
    {
        private readonly SqlConnection _connection;

        public UserDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = new List<UserDto>();

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT UserID, Username, PasswordHash, PasswordSalt FROM Users";
                    await _connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new UserDto
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"] != DBNull.Value ? (byte[])reader["PasswordHash"] : null,
                                PasswordSalt = reader["PasswordSalt"] != DBNull.Value ? (byte[])reader["PasswordSalt"] : null
                            });
                        }
                    }
                }
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return users;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            UserDto user = null;

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT UserID, Username, PasswordHash, PasswordSalt FROM Users WHERE UserID = @userId";
                    command.Parameters.AddWithValue("userId", id);

                    await _connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new UserDto
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"] != DBNull.Value ? (byte[])reader["PasswordHash"] : null,
                                PasswordSalt = reader["PasswordSalt"] != DBNull.Value ? (byte[])reader["PasswordSalt"] : null
                            };
                        }
                    }
                }
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return user;
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            UserDto user = null;

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT UserID, Username, PasswordHash, PasswordSalt FROM Users WHERE Username = @username";
                    command.Parameters.AddWithValue("username", username);

                    await _connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new UserDto
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"] != DBNull.Value ? (byte[])reader["PasswordHash"] : null,
                                PasswordSalt = reader["PasswordSalt"] != DBNull.Value ? (byte[])reader["PasswordSalt"] : null
                            };
                        }
                    }
                }
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return user;
        }


        public async Task InsertAsync(UserDto user, string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(user.Username));
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be null or empty.", nameof(plainPassword));

            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = HashPassword(plainPassword, user.PasswordSalt);

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO Users (Username, PasswordHash, PasswordSalt)
                        OUTPUT inserted.UserID
                        VALUES (@Username, @PasswordHash, @PasswordSalt)";

                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = user.Username;
                    command.Parameters.Add("@PasswordHash", SqlDbType.VarBinary).Value = user.PasswordHash;
                    command.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary).Value = user.PasswordSalt;

                    await _connection.OpenAsync();
                    user.UserID = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        public async Task UpdateAsync(UserDto user, string plainPassword)
        {
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = HashPassword(plainPassword, user.PasswordSalt);

            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Users SET Username = @username, PasswordHash = @passwordHash, PasswordSalt = @passwordSalt WHERE UserID = @userId";
                    command.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.Username;
                    command.Parameters.Add("@passwordHash", SqlDbType.VarBinary).Value = user.PasswordHash;
                    command.Parameters.Add("@passwordSalt", SqlDbType.VarBinary).Value = user.PasswordSalt;
                    command.Parameters.Add("@userId", SqlDbType.Int).Value = user.UserID;

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Users WHERE UserID = @userId";
                    command.Parameters.Add("@userId", SqlDbType.Int).Value = id;

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
