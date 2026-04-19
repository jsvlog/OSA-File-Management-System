using MySql.Data.MySqlClient;
using OSAWebAPI.Models;

namespace OSAWebAPI.Services
{
    public class InventoryService
    {
        private readonly string _connectionString;

        public InventoryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public List<InventoryModel> GetAll()
        {
            var list = new List<InventoryModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM inventoryDocs ORDER BY date DESC";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapFromReader(reader));
                    }
                }
            }
            return list;
        }

        public InventoryModel? GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM inventoryDocs WHERE id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapFromReader(reader);
                    }
                }
            }
            return null;
        }

        public List<InventoryModel> Filter(int? year, string? type, string? searchTerm)
        {
            var list = new List<InventoryModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM inventoryDocs WHERE 1=1";

                if (year.HasValue)
                    query += " AND YEAR(date) = @year";

                if (!string.IsNullOrEmpty(type))
                    query += " AND type = @type";

                if (!string.IsNullOrEmpty(searchTerm))
                    query += " AND (description LIKE @search OR status LIKE @search OR location LIKE @search OR remarks LIKE @search)";

                query += " ORDER BY date DESC";

                using (var command = new MySqlCommand(query, connection))
                {
                    if (year.HasValue)
                        command.Parameters.AddWithValue("@year", year.Value);

                    if (!string.IsNullOrEmpty(type))
                        command.Parameters.AddWithValue("@type", type);

                    if (!string.IsNullOrEmpty(searchTerm))
                        command.Parameters.AddWithValue("@search", $"%{searchTerm}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapFromReader(reader));
                        }
                    }
                }
            }
            return list;
        }

        public List<string> GetDistinctTypes()
        {
            var types = new List<string>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT DISTINCT type FROM inventoryDocs WHERE type IS NOT NULL AND type != '' ORDER BY type";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(reader["type"].ToString() ?? "");
                    }
                }
            }
            return types;
        }

        public string GetYearRange()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT MIN(YEAR(date)) as minYr, MAX(YEAR(date)) as maxYr FROM inventoryDocs WHERE date IS NOT NULL";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var min = reader["minYr"] != DBNull.Value ? Convert.ToInt32(reader["minYr"]) : (int?)null;
                        var max = reader["maxYr"] != DBNull.Value ? Convert.ToInt32(reader["maxYr"]) : (int?)null;
                        if (min.HasValue && max.HasValue)
                        {
                            return min == max ? min.Value.ToString() : $"{min} - {max}";
                        }
                    }
                }
            }
            return "No data";
        }

        public List<int> GetDistinctYears()
        {
            var years = new List<int>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT DISTINCT YEAR(date) as yr FROM inventoryDocs WHERE date IS NOT NULL ORDER BY yr DESC";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        years.Add(Convert.ToInt32(reader["yr"]));
                    }
                }
            }
            if (years.Count == 0)
            {
                years = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Reverse().ToList();
            }
            return years;
        }

        private InventoryModel MapFromReader(MySqlDataReader reader)
        {
            return new InventoryModel
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                Date = reader["date"] != DBNull.Value ? Convert.ToDateTime(reader["date"]) : null,
                Type = reader["type"]?.ToString(),
                Description = reader["description"]?.ToString(),
                Status = reader["status"]?.ToString(),
                Location = reader["location"]?.ToString(),
                Remarks = reader["remarks"]?.ToString(),
                ScannedCopy = reader["scannedCopy"]?.ToString()
            };
        }
    }
}