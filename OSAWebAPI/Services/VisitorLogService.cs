using MySql.Data.MySqlClient;
using OSAWebAPI.Models;

namespace OSAWebAPI.Services
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
