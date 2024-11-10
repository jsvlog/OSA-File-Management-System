using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
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
                        TypeOfDocs = reader["typeOfDocs"] is DBNull ? null : reader["typeOfDocs"].ToString(),
                        Addressee = reader["addressee"] is DBNull ? null : reader["addressee"].ToString(),
                        SubjectParticulars = reader["subjectParticulars"] is DBNull ? null : reader["subjectParticulars"].ToString(),
                        Details = reader["details"] is DBNull ? null : reader["details"].ToString(),
                        RefNumber = reader["refNumber"] is DBNull ? null : reader["refNumber"].ToString(),
                        Municipality = reader["municipality"] is DBNull ? null : reader["municipality"].ToString(),
                        Barangay = reader["barangay"] is DBNull ? null : reader["barangay"].ToString(),
                        ReceivedFrom = reader["receivedFrom"] is DBNull ? null : reader["receivedFrom"].ToString(),
                        DateSentOutToTeam = reader["dateSentOutToTeam"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSentOutToTeam"]),
                        Receiver = reader["receiver"] is DBNull ? null : reader["receiver"].ToString(),
                        Location = reader["location"] is DBNull ? null : reader["location"].ToString(),
                        ActionableDoc = reader["actionableDoc"] is DBNull ? (bool?)null : Convert.ToBoolean(reader["actionableDoc"]),
                        DateDeadline = reader["dateDeadline"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateDeadline"]),
                        Remarks = reader["remarks"] is DBNull ? null : reader["remarks"].ToString(),
                        TrackingCode = reader["trackingCode"] is DBNull ? null : reader["trackingCode"].ToString(),
                        Direction = reader["direction"] is DBNull ? null : reader["direction"].ToString(),
                        NumberOfCopies = reader["numberOfCopies"] is DBNull ? null : reader["numberOfCopies"].ToString(),
                        DateSignBySA = reader["dateSignBySA"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSignBySA"]),
                        DateReceiveByRegion = reader["dateReceiveByRegion"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateReceiveByRegion"]),
                        DateSentOutToRegion = reader["dateSentOutToRegion"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["dateSentOutToRegion"]),
                        LbcRefNumber = reader["lbcRefNumber"] is DBNull ? null : reader["lbcRefNumber"].ToString(),
                        ScannedCopy = reader["scannedCopy"] is DBNull ? null : reader["scannedCopy"].ToString(),

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

        #region Add Data To Region
        public bool addToRegionCom(RegionComModel objRegionCom)
        {
            objRegionCom.Direction = "To Region";
            if (string.IsNullOrEmpty(objRegionCom.TrackingCode))
            {
                objRegionCom.TrackingCode = Guid.NewGuid().ToString();
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "INSERT INTO regioncom (dateReceived, documentDate, typeOfDocs, refNumber, receivedFrom, numberOfCopies, dateSignBySA, dateSentOutToRegion, subjectParticulars, dateSentOutToTeam, receiver, dateReceiveByRegion, location, lbcRefNumber, remarks, scannedCopy, direction, trackingCode) " +
                                              "VALUES (@dateReceived, @documentDate, @typeOfDocs, @refNumber, @receivedFrom, @numberOfCopies, @dateSignBySA, @dateSentOutToRegion, @subjectParticulars, @dateSentOutToTeam, @receiver, @dateReceiveByRegion, @location, @lbcRefNumber, @remarks, @scannedCopy, @direction, @trackingCode)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@dateReceived", objRegionCom.DateReceived);
                cmd.Parameters.AddWithValue("@documentDate", objRegionCom.DocumentDate);
                cmd.Parameters.AddWithValue("@typeOfDocs", objRegionCom.TypeOfDocs);
                cmd.Parameters.AddWithValue("@refNumber", objRegionCom.RefNumber);
                cmd.Parameters.AddWithValue("@receivedFrom", objRegionCom.ReceivedFrom);
                cmd.Parameters.AddWithValue("@numberOfCopies", objRegionCom.NumberOfCopies);
                cmd.Parameters.AddWithValue("@dateSignBySA", objRegionCom.DateSignBySA);
                cmd.Parameters.AddWithValue("@dateSentOutToRegion", objRegionCom.DateSentOutToRegion);
                cmd.Parameters.AddWithValue("@subjectParticulars", objRegionCom.SubjectParticulars);
                cmd.Parameters.AddWithValue("@dateSentOutToTeam", objRegionCom.DateSentOutToTeam);
                cmd.Parameters.AddWithValue("@receiver", objRegionCom.Receiver);
                cmd.Parameters.AddWithValue("@dateReceiveByRegion", objRegionCom.DateReceiveByRegion);
                cmd.Parameters.AddWithValue("@location", objRegionCom.Location);
                cmd.Parameters.AddWithValue("@remarks", objRegionCom.Remarks);
                cmd.Parameters.AddWithValue("@scannedCopy", objRegionCom.ScannedCopy);
                cmd.Parameters.AddWithValue("@lbcRefNumber", objRegionCom.LbcRefNumber);
                cmd.Parameters.AddWithValue("@direction", objRegionCom.Direction);
                cmd.Parameters.AddWithValue("@trackingCode", objRegionCom.TrackingCode);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return true;

        }
        #endregion

        #region Add Data from Region
        public bool addFromRegionCom(RegionComModel objRegionCom)
        {
            objRegionCom.Direction = "From Region";
            if (string.IsNullOrEmpty(objRegionCom.TrackingCode))
            {
                objRegionCom.TrackingCode = Guid.NewGuid().ToString();
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "INSERT INTO regioncom (dateReceived, documentDate, typeOfDocs, refNumber, receivedFrom, addressee, details, municipality, subjectParticulars, barangay, dateSentOutToTeam, receiver, location, actionableDoc, dateDeadline, remarks, scannedCopy, direction, trackingCode) " +
                                              "VALUES (@dateReceived, @documentDate, @typeOfDocs, @refNumber, @receivedFrom, @addressee, @details, @municipality, @subjectParticulars, @barangay, @dateSentOutToTeam, @receiver, @location, @actionableDoc, @dateDeadline, @remarks, @scannedCopy, @direction, @trackingCode)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@dateReceived", objRegionCom.DateReceived);
                cmd.Parameters.AddWithValue("@documentDate", objRegionCom.DocumentDate);
                cmd.Parameters.AddWithValue("@typeOfDocs", objRegionCom.TypeOfDocs);
                cmd.Parameters.AddWithValue("@refNumber", objRegionCom.RefNumber);
                cmd.Parameters.AddWithValue("@receivedFrom", objRegionCom.ReceivedFrom);
                cmd.Parameters.AddWithValue("@addressee", objRegionCom.Addressee);
                cmd.Parameters.AddWithValue("@details", objRegionCom.Details);
                cmd.Parameters.AddWithValue("@municipality", objRegionCom.Municipality);
                cmd.Parameters.AddWithValue("@subjectParticulars", objRegionCom.SubjectParticulars);
                cmd.Parameters.AddWithValue("@barangay", objRegionCom.Barangay);
                cmd.Parameters.AddWithValue("@dateSentOutToTeam", objRegionCom.DateSentOutToTeam);
                cmd.Parameters.AddWithValue("@receiver", objRegionCom.Receiver);
                cmd.Parameters.AddWithValue("@location", objRegionCom.Location);
                cmd.Parameters.AddWithValue("@actionableDoc", objRegionCom.ActionableDoc);
                cmd.Parameters.AddWithValue("@dateDeadline", objRegionCom.DateDeadline);
                cmd.Parameters.AddWithValue("@remarks", objRegionCom.Remarks);
                cmd.Parameters.AddWithValue("@scannedCopy", objRegionCom.ScannedCopy);
                cmd.Parameters.AddWithValue("@direction", objRegionCom.Direction);
                cmd.Parameters.AddWithValue("@trackingCode", objRegionCom.TrackingCode);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return true;

        }
        #endregion

        #region Delete Document to RegionCom
        public bool DeleteData(RegionComModel rmodel)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "DELETE FROM regioncom WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", rmodel.Id); // Assuming 'Id' is the primary key
                int rowsAffected = cmd.ExecuteNonQuery();

                connection.Close();
                return rowsAffected > 0; // Returns true if deletion is successful

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Save Edited ToRegion
        public bool SaveEditedToRegion(RegionComModel ToRegionData)
        {
            if (string.IsNullOrEmpty(ToRegionData.TrackingCode))
            {
                ToRegionData.TrackingCode = Guid.NewGuid().ToString();
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string query = "UPDATE regioncom SET dateReceived = @dateReceived, documentDate = @documentDate, typeOfDocs = @typeOfDocs, refNumber = @refNumber, receivedFrom = @receivedFrom, numberOfCopies = @numberOfCopies, dateSignBySA = @dateSignBySA, dateSentOutToRegion = @dateSentOutToRegion, subjectParticulars = @subjectParticulars, dateSentOutToTeam = @dateSentOutToTeam, receiver = @receiver, dateReceiveByRegion = @dateReceiveByRegion, location = @location, lbcRefNumber = @lbcRefNumber, remarks = @remarks, scannedCopy = @scannedCopy, direction = @direction, trackingCode = @trackingCode  WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", ToRegionData.Id);
                cmd.Parameters.AddWithValue("@dateReceived", ToRegionData.DateReceived);
                cmd.Parameters.AddWithValue("@documentDate", ToRegionData.DocumentDate);
                cmd.Parameters.AddWithValue("@typeOfDocs", ToRegionData.TypeOfDocs);
                cmd.Parameters.AddWithValue("@refNumber", ToRegionData.RefNumber);
                cmd.Parameters.AddWithValue("@receivedFrom", ToRegionData.ReceivedFrom);
                cmd.Parameters.AddWithValue("@numberOfCopies", ToRegionData.NumberOfCopies);
                cmd.Parameters.AddWithValue("@dateSignBySA", ToRegionData.DateSignBySA);
                cmd.Parameters.AddWithValue("@dateSentOutToRegion", ToRegionData.DateSentOutToRegion);
                cmd.Parameters.AddWithValue("@subjectParticulars", ToRegionData.SubjectParticulars);
                cmd.Parameters.AddWithValue("@dateSentOutToTeam", ToRegionData.DateSentOutToTeam);
                cmd.Parameters.AddWithValue("@receiver", ToRegionData.Receiver);
                cmd.Parameters.AddWithValue("@dateReceiveByRegion", ToRegionData.DateReceiveByRegion);
                cmd.Parameters.AddWithValue("@location", ToRegionData.Location);
                cmd.Parameters.AddWithValue("@remarks", ToRegionData.Remarks);
                cmd.Parameters.AddWithValue("@scannedCopy", ToRegionData.ScannedCopy);
                cmd.Parameters.AddWithValue("@lbcRefNumber", ToRegionData.LbcRefNumber);
                cmd.Parameters.AddWithValue("@direction", ToRegionData.Direction);
                cmd.Parameters.AddWithValue("@trackingCode", ToRegionData.TrackingCode);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return true;
        }


        #endregion

        #region Save Edited FromRegion
        public bool SaveEditedFromRegion(RegionComModel FromRegionData)
        {
            if (string.IsNullOrEmpty(FromRegionData.TrackingCode))
            {
                FromRegionData.TrackingCode = Guid.NewGuid().ToString();
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string query = "UPDATE regioncom SET dateReceived = @dateReceived, documentDate = @documentDate, typeOfDocs = @typeOfDocs, refNumber = @refNumber, receivedFrom = @receivedFrom, addressee = @addressee, details = @details, municipality = @municipality, subjectParticulars = @subjectParticulars, barangay = @barangay, dateSentOutToTeam = @dateSentOutToTeam, receiver = @receiver, location = @location, actionableDoc = @actionableDoc, dateDeadline = @dateDeadline, remarks = @remarks, scannedCopy = @scannedCopy, direction = @direction, trackingCode = @trackingCode  WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", FromRegionData.Id);
                cmd.Parameters.AddWithValue("@dateReceived", FromRegionData.DateReceived);
                cmd.Parameters.AddWithValue("@documentDate", FromRegionData.DocumentDate);
                cmd.Parameters.AddWithValue("@typeOfDocs", FromRegionData.TypeOfDocs);
                cmd.Parameters.AddWithValue("@refNumber", FromRegionData.RefNumber);
                cmd.Parameters.AddWithValue("@receivedFrom", FromRegionData.ReceivedFrom);
                cmd.Parameters.AddWithValue("@addressee", FromRegionData.Addressee);
                cmd.Parameters.AddWithValue("@details", FromRegionData.Details);
                cmd.Parameters.AddWithValue("@municipality", FromRegionData.Municipality);
                cmd.Parameters.AddWithValue("@subjectParticulars", FromRegionData.SubjectParticulars);
                cmd.Parameters.AddWithValue("@barangay", FromRegionData.Barangay);
                cmd.Parameters.AddWithValue("@dateSentOutToTeam", FromRegionData.DateSentOutToTeam);
                cmd.Parameters.AddWithValue("@receiver", FromRegionData.Receiver);
                cmd.Parameters.AddWithValue("@location", FromRegionData.Location);
                cmd.Parameters.AddWithValue("@actionableDoc", FromRegionData.ActionableDoc);
                cmd.Parameters.AddWithValue("@dateDeadline", FromRegionData.DateDeadline);
                cmd.Parameters.AddWithValue("@remarks", FromRegionData.Remarks);
                cmd.Parameters.AddWithValue("@scannedCopy", FromRegionData.ScannedCopy);
                cmd.Parameters.AddWithValue("@direction", FromRegionData.Direction);
                cmd.Parameters.AddWithValue("@trackingCode", FromRegionData.TrackingCode);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return true;
        }
        #endregion


    }
}

