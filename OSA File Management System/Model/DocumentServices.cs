using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using OSA_File_Management_System.View;
using System.Windows;

namespace OSA_File_Management_System.Model
{
    class DocumentServices
    {
        private MySqlConnection connection;

        public DocumentServices() 
        {
            

        }

        private string connectionString = "SERVER=localhost;DATABASE=osasystem;UID=root;PASSWORD=12345;";



        public ObservableCollection<Document> GetAllDocuments()
        {
            var inventoryList = new ObservableCollection<Document>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM inventoryDocs";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        inventoryList.Add(new Document
                        {
                            Date = reader["date"].ToString(),
                            Type = reader["type"].ToString(),
                            Description = reader["description"].ToString(),
                            Status = reader["status"].ToString(),
                            Location = reader["location"].ToString(),
                            Remarks = reader["remarks"].ToString(),
                            ScannedCopy = reader["scannedCopy"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return inventoryList;
        }





    }
}
