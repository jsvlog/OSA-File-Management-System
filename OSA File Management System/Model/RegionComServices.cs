using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace OSA_File_Management_System.Model
{
    class RegionComServices
    {

        private MySqlConnection connection;

        public RegionComServices()
        {
            ConnectToDatabase();
        }


        private void ConnectToDatabase()
        {
            string connectionString = "SERVER=localhost;DATABASE=osasystem;UID=root;PASSWORD=12345;";
            connection = new MySqlConnection(connectionString);
        }


        #region Get All RegionCom
        public ObservableCollection<RegionComModel> GetAllRegionCom()
        {
            var regionComList = new ObservableCollection<RegionComModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM regioncom";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    regionComList.Add(new RegionComModel
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        DateReceived = reader["dateReceived"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateReceived"]),
                        DocumentDate = reader["documentDate"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["documentDate"]),
                        TypeOfDocs = reader["typeOfDocs"]?.ToString() ?? string.Empty,
                        Addressee = reader["addressee"]?.ToString() ?? string.Empty,
                        SubjectParticulars = reader["subjectParticulars"]?.ToString() ?? string.Empty,
                        Details = reader["details"]?.ToString() ?? string.Empty,
                        RefNumber = reader["refnumber"]?.ToString() ?? string.Empty,
                        Municipality = reader["municipality"]?.ToString() ?? string.Empty,
                        Barangay = reader["barangay"]?.ToString() ?? string.Empty,
                        ReceivedFrom = reader["receivedFrom"]?.ToString() ?? string.Empty,
                        DateSentOutToTeam = reader["dateSentOutToTeam"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSentOutToTeam"]),
                        Receiver = reader["receiver"]?.ToString() ?? string.Empty,
                        Location = reader["location"]?.ToString() ?? string.Empty,
                        ActionableDoc = reader["actionableDoc"] is DBNull ? false : Convert.ToBoolean(reader["actionableDoc"]),
                        DateDeadline = reader["dateDeadline"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateDeadline"]),
                        Remarks = reader["remarks"]?.ToString() ?? string.Empty,
                        TrackingCode = reader["trackingCode"]?.ToString() ?? string.Empty,
                        Direction = reader["direction"]?.ToString() ?? string.Empty,
                        NumberOfCopies = reader["numberOfCopies"]?.ToString() ?? string.Empty,
                        DateSignBySA = reader["dateSignBySA"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSignBySA"]),
                        DateReceiveByRegion = reader["dateReceiveByRegion"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateReceiveByRegion"]),
                        DateSentOutToRegion = reader["dateSentOutToRegion"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSentOutToRegion"]),
                        LbcRefNumber = reader["lbcRefNumber"]?.ToString() ?? string.Empty,
                        ScannedCopy = reader["scannedCopy"]?.ToString() ?? string.Empty

                    });
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return regionComList;
        }
        #endregion








    }
}
