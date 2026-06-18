using MySql.Data.MySqlClient;
using VisitorLogAPI.Models;

namespace VisitorLogAPI.Services
{
    public class VisitorLogService
    {
        private readonly string _connectionString;

        public VisitorLogService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void EnsureTableExists()
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"
                CREATE TABLE IF NOT EXISTS visitors_log (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(255),
                    purpose TEXT,
                    office VARCHAR(255),
                    municipality VARCHAR(255),
                    barangay VARCHAR(255),
                    entry_time DATETIME DEFAULT CURRENT_TIMESTAMP
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
            using var command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public List<VisitorLog> GetAll(string? search = null, string? office = null, string? municipality = null, string? barangay = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var result = new List<VisitorLog>();
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT * FROM visitors_log WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " AND (name LIKE @search OR purpose LIKE @search)";
            }
            if (!string.IsNullOrWhiteSpace(office))
            {
                query += " AND office LIKE @office";
            }
            if (!string.IsNullOrWhiteSpace(municipality))
            {
                query += " AND municipality LIKE @municipality";
            }
            if (!string.IsNullOrWhiteSpace(barangay))
            {
                query += " AND barangay LIKE @barangay";
            }
            if (dateFrom.HasValue)
            {
                query += " AND DATE(entry_time) >= @dateFrom";
            }
            if (dateTo.HasValue)
            {
                query += " AND DATE(entry_time) <= @dateTo";
            }
            query += " ORDER BY entry_time DESC";
            using var command = new MySqlCommand(query, connection);
            if (!string.IsNullOrWhiteSpace(search))
                command.Parameters.AddWithValue("@search", "%" + search + "%");
            if (!string.IsNullOrWhiteSpace(office))
                command.Parameters.AddWithValue("@office", "%" + office + "%");
            if (!string.IsNullOrWhiteSpace(municipality))
                command.Parameters.AddWithValue("@municipality", "%" + municipality + "%");
            if (!string.IsNullOrWhiteSpace(barangay))
                command.Parameters.AddWithValue("@barangay", "%" + barangay + "%");
            if (dateFrom.HasValue)
                command.Parameters.AddWithValue("@dateFrom", dateFrom.Value.Date);
            if (dateTo.HasValue)
                command.Parameters.AddWithValue("@dateTo", dateTo.Value.Date);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(MapFromReader(reader));
            }
            return result;
        }

        public VisitorLog? GetById(int id)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT * FROM visitors_log WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapFromReader(reader);
            }
            return null;
        }

        public bool Create(VisitorLog log)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"
                INSERT INTO visitors_log (name, purpose, office, municipality, barangay, entry_time)
                VALUES (@name, @purpose, @office, @municipality, @barangay, @entry_time)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", log.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@purpose", log.Purpose ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@office", log.Office ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@municipality", log.Municipality ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@barangay", log.Barangay ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@entry_time", log.EntryTime ?? DateTime.Now);
            return command.ExecuteNonQuery() > 0;
        }

        public bool Update(VisitorLog log)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"
                UPDATE visitors_log
                SET name = @name,
                    purpose = @purpose,
                    office = @office,
                    municipality = @municipality,
                    barangay = @barangay
                WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", log.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@purpose", log.Purpose ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@office", log.Office ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@municipality", log.Municipality ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@barangay", log.Barangay ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@id", log.Id);
            return command.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "DELETE FROM visitors_log WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }

        private VisitorLog MapFromReader(MySqlDataReader reader)
        {
            return new VisitorLog
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                Name = reader["name"]?.ToString(),
                Purpose = reader["purpose"]?.ToString(),
                Office = reader["office"]?.ToString(),
                Municipality = reader["municipality"]?.ToString(),
                Barangay = reader["barangay"]?.ToString(),
                EntryTime = reader["entry_time"] != DBNull.Value ? Convert.ToDateTime(reader["entry_time"]) : null
            };
        }
    }
}
