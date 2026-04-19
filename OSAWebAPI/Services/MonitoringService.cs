using MySql.Data.MySqlClient;
using OSAWebAPI.Models;

namespace OSAWebAPI.Services
{
    public class MonitoringService
    {
        private readonly string _connectionString;

        public MonitoringService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void EnsureTableExists()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string createTable = @"
                    CREATE TABLE IF NOT EXISTS monitoring_submissions (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        documentType VARCHAR(100) NOT NULL,
                        municipality VARCHAR(255) NOT NULL,
                        year INT NOT NULL,
                        month INT NOT NULL,
                        dateSubmitted DATE,
                        status VARCHAR(50) DEFAULT 'Pending',
                        remarks TEXT,
                        createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        UNIQUE KEY uq_submission (documentType, municipality, year, month)
                    )";
                using (var command = new MySqlCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetMunicipalities()
        {
            var municipalities = new List<string>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT DISTINCT municipality FROM regioncom WHERE municipality IS NOT NULL AND municipality != '' ORDER BY municipality";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        municipalities.Add(reader["municipality"].ToString() ?? "");
                    }
                }
            }
            if (municipalities.Count == 0)
            {
                municipalities = new List<string>
                {
                    "Buenavista", "Butuan City", "Cabadbaran City", "Carmen", "Jabonga",
                    "Kitcharao", "Las Nieves", "Magallanes", "Nasipit", "Remedios T. Romualdez",
                    "Santiago", "Tubay"
                };
            }
            return municipalities;
        }

        public MonitoringStatusGrid GetStatusGrid(string documentType, int year)
        {
            var grid = new MonitoringStatusGrid
            {
                DocumentType = documentType,
                Year = year
            };

            var municipalities = GetMunicipalities();
            var monthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            var submissions = new List<MonitoringSubmission>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM monitoring_submissions WHERE documentType = @docType AND year = @year";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@docType", documentType);
                    command.Parameters.AddWithValue("@year", year);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submissions.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            foreach (var muni in municipalities)
            {
                var muniStatus = new MunicipalityMonthStatus { Municipality = muni };
                for (int m = 1; m <= 12; m++)
                {
                    var submission = submissions.FirstOrDefault(s =>
                        s.Municipality == muni && s.Month == m);
                    muniStatus.Months.Add(new MonthStatus
                    {
                        Month = m,
                        MonthName = monthNames[m - 1],
                        Status = submission?.Status ?? "Not Submitted",
                        DateSubmitted = submission?.DateSubmitted,
                        Remarks = submission?.Remarks
                    });
                }
                grid.Municipalities.Add(muniStatus);
            }

            return grid;
        }

        public List<MonitoringDocType> GetDocTypes()
        {
            return new List<MonitoringDocType>
            {
                new MonitoringDocType { Key = "Financial Statement", DisplayName = "Financial Statement", Icon = "financial", Color = "blue" },
                new MonitoringDocType { Key = "Barangay Financial Statement", DisplayName = "Barangay Financial Statement", Icon = "barangay-financial", Color = "green" },
                new MonitoringDocType { Key = "AOM", DisplayName = "Audit Observation Memorandum", Icon = "aom", Color = "purple" },
                new MonitoringDocType { Key = "Barangay AOM", DisplayName = "Barangay Audit Observation Memorandum", Icon = "barangay-aom", Color = "orange" },
                new MonitoringDocType { Key = "AAR", DisplayName = "Annual Audit Report", Icon = "aar", Color = "red" },
                new MonitoringDocType { Key = "Barangay AAR", DisplayName = "Barangay AAR", Icon = "barangay-aar", Color = "pink" }
            };
        }

        private MonitoringSubmission MapFromReader(MySqlDataReader reader)
        {
            return new MonitoringSubmission
            {
                Id = Convert.ToInt32(reader["id"]),
                DocumentType = reader["documentType"]?.ToString(),
                Municipality = reader["municipality"]?.ToString(),
                Year = Convert.ToInt32(reader["year"]),
                Month = Convert.ToInt32(reader["month"]),
                DateSubmitted = reader["dateSubmitted"] != DBNull.Value ? Convert.ToDateTime(reader["dateSubmitted"]) : null,
                Status = reader["status"]?.ToString(),
                Remarks = reader["remarks"]?.ToString(),
                CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                UpdatedAt = Convert.ToDateTime(reader["updatedAt"])
            };
        }
    }
}