using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace OSA_File_Management_System.Model
{
    class MonitoringServices
    {
        private MySqlConnection connection;

        public MonitoringServices()
        {
            ConnectToDatabase();
        }

        private void ConnectToDatabase()
        {
            connection = new MySqlConnection(AppConfig.GetConnectionString());
        }

        #region Ensure Table Exists
        public void EnsureTableExists()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string createTable = @"
                    CREATE TABLE IF NOT EXISTS monitoring_submissions (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        documentType VARCHAR(100) NOT NULL,
                        municipality VARCHAR(255) NOT NULL,
                        year INT NOT NULL,
                        dateSubmitted DATE,
                        status VARCHAR(50) DEFAULT 'Not Submitted',
                        remarks TEXT,
                        createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        UNIQUE KEY uq_submission (documentType, municipality, year)
                    )";
                MySqlCommand cmd = new MySqlCommand(createTable, connection);
                cmd.ExecuteNonQuery();

                string checkColumn = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'monitoring_submissions' AND COLUMN_NAME = 'month'";
                MySqlCommand checkCmd = new MySqlCommand(checkColumn, connection);
                var result = checkCmd.ExecuteScalar();
                if (result != null && Convert.ToInt32(result) > 0)
                {
                    string alterTable = "ALTER TABLE monitoring_submissions DROP COLUMN month";
                    MySqlCommand alterCmd = new MySqlCommand(alterTable, connection);
                    alterCmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Get All Monitoring Submissions
        public ObservableCollection<MonitoringModel> GetAllMonitoring(string documentType, int year)
        {
            var monitoringList = new ObservableCollection<MonitoringModel>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query;
                MySqlCommand cmd;
                if (year == 0)
                {
                    query = "SELECT * FROM monitoring_submissions WHERE documentType = @documentType";
                    cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@documentType", documentType);
                }
                else
                {
                    query = "SELECT * FROM monitoring_submissions WHERE documentType = @documentType AND year = @year";
                    cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@documentType", documentType);
                    cmd.Parameters.AddWithValue("@year", year);
                }
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    monitoringList.Add(new MonitoringModel
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        DocumentType = reader["documentType"]?.ToString(),
                        Municipality = reader["municipality"]?.ToString(),
                        Year = Convert.ToInt32(reader["year"]),
                        DateSubmitted = reader["dateSubmitted"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSubmitted"]),
                        Status = reader["status"]?.ToString(),
                        Remarks = reader["remarks"]?.ToString()
                    });
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return monitoringList;
        }
        #endregion

        #region Add Monitoring Submission
        public bool AddMonitoring(MonitoringModel monitoring)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "INSERT INTO monitoring_submissions (documentType, municipality, year, dateSubmitted, status, remarks) " +
                               "VALUES (@documentType, @municipality, @year, @dateSubmitted, @status, @remarks) " +
                               "ON DUPLICATE KEY UPDATE dateSubmitted = @dateSubmitted, status = @status, remarks = @remarks, updatedAt = CURRENT_TIMESTAMP";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@documentType", monitoring.DocumentType);
                cmd.Parameters.AddWithValue("@municipality", monitoring.Municipality);
                cmd.Parameters.AddWithValue("@year", monitoring.Year);
                cmd.Parameters.AddWithValue("@dateSubmitted", monitoring.DateSubmitted.HasValue ? monitoring.DateSubmitted.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", monitoring.Status);
                cmd.Parameters.AddWithValue("@remarks", monitoring.Remarks ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region Update Monitoring Submission
        public bool UpdateMonitoring(MonitoringModel monitoring)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "UPDATE monitoring_submissions SET dateSubmitted = @dateSubmitted, status = @status, remarks = @remarks WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", monitoring.Id);
                cmd.Parameters.AddWithValue("@dateSubmitted", monitoring.DateSubmitted.HasValue ? monitoring.DateSubmitted.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", monitoring.Status);
                cmd.Parameters.AddWithValue("@remarks", monitoring.Remarks ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region Delete Monitoring Submission
        public bool DeleteMonitoring(MonitoringModel monitoring)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "DELETE FROM monitoring_submissions WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", monitoring.Id);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region Get Municipalities
        public ObservableCollection<string> GetMunicipalities()
        {
            var municipalities = new ObservableCollection<string>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "SELECT DISTINCT municipality FROM regioncom WHERE municipality IS NOT NULL AND municipality != '' ORDER BY municipality";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    municipalities.Add(reader["municipality"]?.ToString() ?? "");
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (municipalities.Count == 0)
            {
                municipalities = new ObservableCollection<string>
                {
                    "Buenavista", "Butuan City", "Cabadbaran City", "Carmen", "Jabonga",
                    "Kitcharao", "Las Nieves", "Magallanes", "Nasipit", "Remedios T. Romualdez",
                    "Santiago", "Tubay"
                };
            }
            return municipalities;
        }
        #endregion
    }
}