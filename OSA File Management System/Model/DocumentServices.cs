using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using OSA_File_Management_System.View;
using System.Windows;
using System.Data;

namespace OSA_File_Management_System.Model
{
    class DocumentServices
    {
        private MySqlConnection connection;

        public DocumentServices() 
        {
            ConnectToDatabase();

        }

        private void ConnectToDatabase()
        {
            string connectionString = "SERVER=localhost;DATABASE=osasystem;UID=root;PASSWORD=12345;";
            connection = new MySqlConnection(connectionString);
        }


        #region Get All Documents
        public ObservableCollection<Document> GetAllDocuments()
        {
            var inventoryList = new ObservableCollection<Document>();
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM inventoryDocs";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        inventoryList.Add(new Document
                        {
                            Date = Convert.ToDateTime(reader["date"]),
                            Type = reader["type"].ToString(),
                            Description = reader["description"].ToString(),
                            Status = reader["status"].ToString(),
                            Location = reader["location"].ToString(),
                            Remarks = reader["remarks"].ToString(),
                            ScannedCopy = reader["scannedCopy"].ToString()
                        });
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
            
            return inventoryList;
        }
        #endregion

        #region Add Document to Inventory
        public bool addDocument(Document objDocument)
        {

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = "INSERT INTO inventorydocs (date, type, description, status, location, remarks, scannedCopy) VALUES (@date, @type, @description, @status, @location, @remarks, scannedCopy)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                if (objDocument.Date.HasValue)
                {
                    // Use .Value to access the underlying DateTime
                    cmd.Parameters.AddWithValue("@date", objDocument.Date.Value.Date);
                }
                else
                {
                    // Handle case where Date is null (you might want to throw an error or set a default)
                    cmd.Parameters.AddWithValue("@date", DBNull.Value); // Or a default DateTime value
                }
                cmd.Parameters.AddWithValue("@type", objDocument.Type);
                cmd.Parameters.AddWithValue("@description", objDocument.Description);
                cmd.Parameters.AddWithValue("@status", objDocument.Status);
                cmd.Parameters.AddWithValue("@location", objDocument.Location);
                cmd.Parameters.AddWithValue("@remarks", objDocument.Remarks);
                cmd.Parameters.AddWithValue("@scannedCopy", objDocument.ScannedCopy);
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
