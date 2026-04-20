using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;
using OSA_File_Management_System.ViewModel;

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

private CertificateViewModel certificateViewModel;

        public CertificateViewModel CertificateViewModel
        {
            get { return certificateViewModel; }
            set { certificateViewModel = value; }
        }

        private MonitoringViewModel monitoringViewModel;

        public MonitoringViewModel MonitoringViewModel
        {
            get { return monitoringViewModel; }
            set { monitoringViewModel = value; }
        }



public MainViewModel()
		{
			regionComViewModel = new RegionComViewModel();
			documentViewModel = new DocumentViewModel();
            certificateViewModel = new CertificateViewModel();
            monitoringViewModel = new MonitoringViewModel();
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
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Database Backup",
                    Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
                    FileName = $"osasystem_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql",
                    InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string outputFilePath = saveFileDialog.FileName;

                    string mysqldumpPath = FindMySqlDumpPath();

                    if (string.IsNullOrEmpty(mysqldumpPath))
                    {
                        MessageBox.Show("Could not find mysqldump.exe. Please ensure MySQL is installed and mysqldump is in your PATH or MySQL installation folder.", 
                            "MySQL Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string arguments = $"--host=localhost --user=root --password=12345 osasystem --routines --triggers";

                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = mysqldumpPath,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process process = Process.Start(processInfo))
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            throw new Exception($"mysqldump failed (exit code {process.ExitCode}): {error}");
                        }

                        File.WriteAllText(outputFilePath, output);
                    }

                    MessageBox.Show($"Database backup saved successfully!\n\nLocation: {outputFilePath}", 
                        "Backup Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during backup:\n{ex.Message}", 
                    "Backup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FindMySqlDumpPath()
        {
            string[] commonPaths = new[]
            {
                @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysqldump.exe",
                @"C:\Program Files (x86)\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                @"C:\Program Files (x86)\MySQL\MySQL Server 5.7\bin\mysqldump.exe"
            };

            foreach (string path in commonPaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "where",
                    Arguments = "mysqldump",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                    {
                        return output.Trim().Split('\n')[0].Trim();
                    }
                }
            }
            catch { }

            return string.Empty;
        }


		#endregion


	}
}
