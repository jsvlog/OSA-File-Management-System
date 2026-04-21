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

                string checkBarangayColumn = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'monitoring_submissions' AND COLUMN_NAME = 'barangay'";
                using (var checkBarangayCmd = new MySqlCommand(checkBarangayColumn, connection))
                {
                    var result = checkBarangayCmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) == 0)
                    {
                        string alterBarangayTable = "ALTER TABLE monitoring_submissions ADD COLUMN barangay VARCHAR(255)";
                        using (var alterBarangayCmd = new MySqlCommand(alterBarangayTable, connection))
                        {
                            alterBarangayCmd.ExecuteNonQuery();
                        }
                    }
                }

                string checkOldKey = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'monitoring_submissions' AND INDEX_NAME = 'uq_submission'";
                using (var checkKeyCmd = new MySqlCommand(checkOldKey, connection))
                {
                    var keyResult = checkKeyCmd.ExecuteScalar();
                    if (keyResult != null && Convert.ToInt32(keyResult) > 0)
                    {
                        string dropKey = "ALTER TABLE monitoring_submissions DROP INDEX uq_submission";
                        using (var dropKeyCmd = new MySqlCommand(dropKey, connection))
                        {
                            dropKeyCmd.ExecuteNonQuery();
                        }
                    }
                }

                string updateBarangayNull = "UPDATE monitoring_submissions SET barangay = '' WHERE barangay IS NULL";
                using (var updateCmd = new MySqlCommand(updateBarangayNull, connection))
                {
                    updateCmd.ExecuteNonQuery();
                }

                string createTable = @"
                    CREATE TABLE IF NOT EXISTS monitoring_submissions (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        documentType VARCHAR(100) NOT NULL,
                        municipality VARCHAR(255) NOT NULL,
                        barangay VARCHAR(255) NOT NULL DEFAULT '',
                        year INT NOT NULL,
                        dateSubmitted DATE,
                        status VARCHAR(50) DEFAULT 'Not Submitted',
                        remarks TEXT,
                        pdfLink TEXT,
                        createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        UNIQUE KEY uq_submission (documentType, municipality, year, barangay)
                    )";
                using (var command = new MySqlCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                string checkColumn = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'monitoring_submissions' AND COLUMN_NAME = 'month'";
                using (var checkCmd = new MySqlCommand(checkColumn, connection))
                {
                    var result = checkCmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) > 0)
                    {
                        string alterTable = "ALTER TABLE monitoring_submissions DROP COLUMN month";
                        using (var alterCmd = new MySqlCommand(alterTable, connection))
                        {
                            alterCmd.ExecuteNonQuery();
                        }
                    }
                }

                string checkPdfColumn = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'monitoring_submissions' AND COLUMN_NAME = 'pdfLink'";
                using (var checkPdfCmd = new MySqlCommand(checkPdfColumn, connection))
                {
                    var result = checkPdfCmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) == 0)
                    {
                        string alterPdfTable = "ALTER TABLE monitoring_submissions ADD COLUMN pdfLink TEXT";
                        using (var alterPdfCmd = new MySqlCommand(alterPdfTable, connection))
                        {
                            alterPdfCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public List<int> GetYearColumns()
        {
            var years = new List<int>();
            for (int y = 2016; y <= DateTime.Now.Year; y++)
            {
                years.Add(y);
            }
            return years;
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
                Year = year,
                IsBarangayDocumentType = IsBarangayDocumentType(documentType)
            };

            var municipalities = GetMunicipalities();
            var yearColumns = GetYearColumns();
            grid.YearColumns = yearColumns;

            var submissions = new List<MonitoringSubmission>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM monitoring_submissions WHERE documentType = @docType";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@docType", documentType);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submissions.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            if (IsBarangayDocumentType(documentType))
            {
                foreach (var muni in municipalities)
                {
                    var barangays = BarangayData.GetBarangaysForMunicipality(muni);
                    foreach (var barangay in barangays)
                    {
                        var row = new MunicipalityStatusRow { Municipality = muni, Barangay = barangay };
                        foreach (var yr in yearColumns)
                        {
                            var submission = submissions.FirstOrDefault(s => s.Municipality == muni && s.Barangay == barangay && s.Year == yr);
                            row.YearStatuses[yr] = new MunicipalityStatus
                            {
                                SubmissionId = submission?.Id ?? 0,
                                Municipality = muni,
                                Barangay = barangay,
                                Year = yr,
                                Status = submission?.Status ?? "Not Submitted",
                                DateSubmitted = submission?.DateSubmitted,
                                Remarks = submission?.Remarks,
                                PdfLink = submission?.PdfLink
                            };
                        }
                        grid.Municipalities.Add(row);
                    }
                }
            }
            else
            {
                foreach (var muni in municipalities)
                {
                    var row = new MunicipalityStatusRow { Municipality = muni };
                    foreach (var yr in yearColumns)
                    {
                        var submission = submissions.FirstOrDefault(s => s.Municipality == muni && s.Year == yr);
                        row.YearStatuses[yr] = new MunicipalityStatus
                        {
                            SubmissionId = submission?.Id ?? 0,
                            Municipality = muni,
                            Year = yr,
                            Status = submission?.Status ?? "Not Submitted",
                            DateSubmitted = submission?.DateSubmitted,
                            Remarks = submission?.Remarks,
                            PdfLink = submission?.PdfLink
                        };
                    }
                    grid.Municipalities.Add(row);
                }
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
                Barangay = reader["barangay"] != DBNull.Value && !string.IsNullOrEmpty(reader["barangay"]?.ToString()) ? reader["barangay"]?.ToString() : null,
                Year = Convert.ToInt32(reader["year"]),
                DateSubmitted = reader["dateSubmitted"] != DBNull.Value ? Convert.ToDateTime(reader["dateSubmitted"]) : null,
                Status = reader["status"]?.ToString(),
                Remarks = reader["remarks"]?.ToString(),
                PdfLink = reader["pdfLink"] != DBNull.Value ? reader["pdfLink"]?.ToString() : null,
                CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                UpdatedAt = Convert.ToDateTime(reader["updatedAt"])
            };
        }

        public bool IsBarangayDocumentType(string docType)
        {
            return docType == "Barangay Financial Statement" ||
                   docType == "Barangay AOM" ||
                   docType == "Barangay AAR";
        }
    }
}