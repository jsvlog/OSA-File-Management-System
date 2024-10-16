using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

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







    }
}
