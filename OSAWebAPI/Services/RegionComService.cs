using MySql.Data.MySqlClient;
using OSAWebAPI.Models;

namespace OSAWebAPI.Services
{
    public class RegionComService
    {
        private readonly string _connectionString;
        
        public RegionComService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
        
        public List<RegionComModel> GetAll()
        {
            var list = new List<RegionComModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM regioncom ORDER BY dateReceived DESC";
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
        
        public RegionComModel? GetById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM regioncom WHERE id = @id";
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
        
        public List<RegionComModel> Filter(int? year, string? type, string? searchTerm)
        {
            var list = new List<RegionComModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM regioncom WHERE 1=1";
                
                if (year.HasValue)
                    query += " AND YEAR(dateReceived) = @year";
                
                if (!string.IsNullOrEmpty(type))
                    query += " AND typeOfDocs = @type";
                
                if (!string.IsNullOrEmpty(searchTerm))
                    query += " AND (subjectParticulars LIKE @search OR refNumber LIKE @search OR remarks LIKE @search)";
                
                query += " ORDER BY dateReceived DESC";
                
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
        
        public DashboardStats GetStatistics()
        {
            var stats = new DashboardStats();
            using (var connection = GetConnection())
            {
                connection.Open();
                
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM regioncom", connection))
                {
                    stats.TotalRecords = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM regioncom WHERE direction = 'To Region'", connection))
                {
                    stats.ToRegionCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM regioncom WHERE direction = 'From Region'", connection))
                {
                    stats.FromRegionCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM regioncom WHERE actionableDoc = 1", connection))
                {
                    stats.ActionableCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM regioncom WHERE MONTH(dateReceived) = MONTH(CURDATE()) AND YEAR(dateReceived) = YEAR(CURDATE())", connection))
                {
                    stats.ThisMonthCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                stats.ByType = new List<TypeCount>();
                using (var cmd = new MySqlCommand("SELECT typeOfDocs, COUNT(*) as count FROM regioncom WHERE typeOfDocs IS NOT NULL GROUP BY typeOfDocs ORDER BY count DESC", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stats.ByType.Add(new TypeCount
                        {
                            Type = reader["typeOfDocs"].ToString(),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
                
                stats.ByMunicipality = new List<MunicipalityCount>();
                using (var cmd = new MySqlCommand("SELECT municipality, COUNT(*) as count FROM regioncom WHERE municipality IS NOT NULL GROUP BY municipality ORDER BY count DESC LIMIT 10", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stats.ByMunicipality.Add(new MunicipalityCount
                        {
                            Municipality = reader["municipality"].ToString(),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
                
                stats.MonthlyTrend = new List<MonthlyCount>();
                using (var cmd = new MySqlCommand(@"
                    SELECT YEAR(dateReceived) as year, MONTH(dateReceived) as month, COUNT(*) as count
                    FROM regioncom
                    WHERE dateReceived >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH)
                    GROUP BY YEAR(dateReceived), MONTH(dateReceived)
                    ORDER BY year, month", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stats.MonthlyTrend.Add(new MonthlyCount
                        {
                            Year = Convert.ToInt32(reader["year"]),
                            Month = Convert.ToInt32(reader["month"]),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
            }
            return stats;
        }
        
        private RegionComModel MapFromReader(MySqlDataReader reader)
        {
            return new RegionComModel
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : null,
                DateReceived = reader["dateReceived"] != DBNull.Value ? Convert.ToDateTime(reader["dateReceived"]) : null,
                DocumentDate = reader["documentDate"] != DBNull.Value ? Convert.ToDateTime(reader["documentDate"]) : null,
                TypeOfDocs = reader["typeOfDocs"]?.ToString(),
                Addressee = reader["addressee"]?.ToString(),
                SubjectParticulars = reader["subjectParticulars"]?.ToString(),
                Details = reader["details"]?.ToString(),
                RefNumber = reader["refNumber"]?.ToString(),
                Municipality = reader["municipality"]?.ToString(),
                Barangay = reader["barangay"]?.ToString(),
                ReceivedFrom = reader["receivedFrom"]?.ToString(),
                DateSentOutToTeam = reader["dateSentOutToTeam"] != DBNull.Value ? Convert.ToDateTime(reader["dateSentOutToTeam"]) : null,
                Receiver = reader["receiver"]?.ToString(),
                Location = reader["location"]?.ToString(),
                ActionableDoc = reader["actionableDoc"] != DBNull.Value ? Convert.ToBoolean(reader["actionableDoc"]) : null,
                DateDeadline = reader["dateDeadline"] != DBNull.Value ? Convert.ToDateTime(reader["dateDeadline"]) : null,
                Remarks = reader["remarks"]?.ToString(),
                TrackingCode = reader["trackingCode"]?.ToString(),
                Direction = reader["direction"]?.ToString(),
                NumberOfCopies = reader["numberOfCopies"]?.ToString(),
                DateSignBySA = reader["dateSignBySA"] != DBNull.Value ? Convert.ToDateTime(reader["dateSignBySA"]) : null,
                DateReceiveByRegion = reader["dateReceiveByRegion"] != DBNull.Value ? Convert.ToDateTime(reader["dateReceiveByRegion"]) : null,
                DateSentOutToRegion = reader["dateSentOutToRegion"] != DBNull.Value ? Convert.ToDateTime(reader["dateSentOutToRegion"]) : null,
                LbcRefNumber = reader["lbcRefNumber"]?.ToString(),
                ScannedCopy = reader["scannedCopy"]?.ToString()
            };
        }
    }
}
