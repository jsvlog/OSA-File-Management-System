using OSA_File_Management_System.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace OSA_File_Management_System.ViewModel
{
    public class CertificateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _position;
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private string _purpose;
        public string Purpose
        {
            get => _purpose;
            set
            {
                _purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }

        public ICommand GenerateCertificateCommand { get; }

        public CertificateViewModel()
        {
            GenerateCertificateCommand = new RelayCommand(GenerateCertificate, CanGenerate);
        }

        private bool CanGenerate()
        {
            // Enable the button only if all fields have text
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Position) &&
                   !string.IsNullOrWhiteSpace(Purpose);
        }

        private void GenerateCertificate()
        {
            // Placeholder for your certificate generation logic (e.g., creating a PDF or a printable document)
            string certificateText = $"Certificate of Appearance\n\n" +
                                     $"This is to certify that {Name}, {Position}, " +
                                     $"has appeared at this office on {System.DateTime.Now:MMMM dd, yyyy} " +
                                     $"for the purpose of: {Purpose}.";

            MessageBox.Show(certificateText, "Certificate Generated");

            // You would typically call a service here to create a document
            // Example:
            // var document = _certificateService.CreateAppearanceCertificate(Name, Position, Purpose);
            // _printService.Print(document);
        }
    }
}