using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;

namespace OSA_File_Management_System.ViewModel
{
    class MainViewModel
    {
		private DocumentViewModel documentViewModel;

		public DocumentViewModel DocumentViewModel
		{
			get { return documentViewModel; }
			set { documentViewModel = value; }
		}

		private RegionComViewModel regionComViewModel;

		public RegionComViewModel RegionComViewModel
		{
			get { return regionComViewModel; }
			set { regionComViewModel = value; }
		}



		public MainViewModel()
		{
			regionComViewModel = new RegionComViewModel();
			documentViewModel = new DocumentViewModel();
			backupDatabaseBtn = new RelayCommand(BackupDatabaseCommand);
        }

		#region Backup Database
		private RelayCommand backupDatabaseBtn;

		public RelayCommand BackupDatabaseBtn
		{
			get { return backupDatabaseBtn; }
		}

		private void BackupDatabaseCommand()
		{
            try
            {
                string downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                // Create a unique backup file name
                string backupFileName = $"osasystem_backup_{DateTime.Now:yyyyMMddHHmmss}.sql";

                // Combine folder path and file name
                string outputFilePath = Path.Combine(downloadsFolder, backupFileName);

                // Create the mysqldump command
                string command = $"mysqldump --host=localhost --user=root --password=12345 osasystem > \"{outputFilePath}\"";

                // Configure the process
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe", // Execute in command prompt
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Run the command
                using (Process process = Process.Start(processInfo))
                {
                    using (var writer = process.StandardInput)
                    {
                        if (writer.BaseStream.CanWrite)
                        {
                            writer.WriteLine(command); // Send the command to cmd
                        }
                    }

                    process.WaitForExit();

                    // Check for errors
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception("Backup failed: " + error);
                    }
                }

                MessageBox.Show("Backup successful: " + outputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during backup: " + ex.Message);
            }
        }


		#endregion


	}
}
