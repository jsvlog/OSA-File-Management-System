using MySql.Data.MySqlClient;
using OSAWebAPI.Models;

namespace OSAWebAPI.Services
{
    public class AuthService
    {
        private readonly string _connectionString;

        public AuthService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public UserModel? ValidateUser(string username, string password)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT id, username, passwordHash, fullName FROM users WHERE username = @username";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var storedHash = reader["passwordHash"]?.ToString();
                            if (storedHash == password)
                            {
                                return new UserModel
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Username = reader["username"]?.ToString(),
                                    FullName = reader["fullName"]?.ToString()
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void EnsureUsersTableExists()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string createTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        username VARCHAR(100) NOT NULL UNIQUE,
                        passwordHash VARCHAR(255) NOT NULL,
                        fullName VARCHAR(255),
                        createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                using (var command = new MySqlCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                string checkAdmin = "SELECT COUNT(*) FROM users WHERE username = 'admin'";
                using (var command = new MySqlCommand(checkAdmin, connection))
                {
                    long count = (long)command.ExecuteScalar();
                    if (count == 0)
                    {
                        string insertAdmin = "INSERT INTO users (username, passwordHash, fullName) VALUES ('admin', 'admin123', 'Administrator')";
                        using (var cmd = new MySqlCommand(insertAdmin, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}