using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using OSA_File_Management_System.Commands; // Refers to your RelayCommand.cs
using OSA_File_Management_System.View;     // Refers to the new CertificatePrintPreview window

namespace OSA_File_Management_System.ViewModel
{
    public class CertificateViewModel : INotifyPropertyChanged
    {
        // 1. INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // 2. Properties
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _position;
        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private string _office;
        public string Office
        {
            get { return _office; }
            set
            {
                _office = value;
                OnPropertyChanged(nameof(Office));
            }
        }

        private DateTime _appearanceDate = DateTime.Now;
        public DateTime AppearanceDate
        {
            get { return _appearanceDate; }
            set
            {
                _appearanceDate = value;
                OnPropertyChanged(nameof(AppearanceDate));
                OnPropertyChanged(nameof(AppearanceDateString)); // Updates the display text when date changes
            }
        }

        private string _purpose;
        public string Purpose
        {
            get { return _purpose; }
            set
            {
                _purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }

        // 3. Computed Properties (For the Certificate Text)
        public string AppearanceDateString
        {
            get { return AppearanceDate.ToString("MMMM dd, yyyy"); }
        }

        public string DayString
        {
            get { return DateTime.Now.Day.ToString() + GetDaySuffix(DateTime.Now.Day); }
        }

        public string MonthYearString
        {
            get { return DateTime.Now.ToString("MMMM yyyy"); }
        }

        // 4. Commands
        // We use ICommand for the property type, and RelayCommand for the initialization
        public ICommand GenerateCertificateCommand { get; set; }

        // 5. Constructor
        public CertificateViewModel()
        {
            // Initialize the command using your RelayCommand class
            GenerateCertificateCommand = new RelayCommand(GenerateCertificate);
        }

        // 6. Methods
        private void GenerateCertificate(object obj)
        {
            // Simple Validation
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Purpose))
            {
                MessageBox.Show("Please fill in the Name and Purpose fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Open the Preview Window
            // We pass 'this' (the current ViewModel) so the window can bind to our data
            var previewWindow = new CertificatePrintPreview(this);
            previewWindow.ShowDialog();
        }

        // Helper for "st", "nd", "rd", "th"
        private string GetDaySuffix(int day)
        {
            if (day % 100 >= 11 && day % 100 <= 13)
                return "th";
            switch (day % 10)
            {
                case 1: return "st";
                case 2: return "nd";
                case 3: return "rd";
                default: return "th";
            }
        }
    }
}