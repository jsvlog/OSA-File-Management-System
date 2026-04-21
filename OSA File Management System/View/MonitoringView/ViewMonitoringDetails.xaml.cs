using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using OSA_File_Management_System.Model;

namespace OSA_File_Management_System.View.MonitoringView
{
    public partial class ViewMonitoringDetails : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewMonitoringDetails(YearStatus yearStatus)
        {
            InitializeComponent();
            DataContext = this;

            DocumentType = yearStatus.DocumentType ?? "";
            Municipality = yearStatus.Municipality ?? "";
            Barangay = yearStatus.Barangay ?? "";
            HasBarangay = !string.IsNullOrEmpty(Barangay);
            Year = yearStatus.Year;
            Status = yearStatus.Status ?? "Not Submitted";
            DateSubmitted = yearStatus.DateSubmitted;
            Remarks = yearStatus.Remarks ?? "";
            PdfLink = yearStatus.PdfLink ?? "";
            HasPdfLink = !string.IsNullOrEmpty(PdfLink);
        }

        public string DocumentType { get; set; }
        public string Municipality { get; set; }
        public string Barangay { get; set; }
        public bool HasBarangay { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string Remarks { get; set; }
        public string PdfLink { get; set; }
        public bool HasPdfLink { get; set; }

        private void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PdfLink))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = PdfLink,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open link: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}