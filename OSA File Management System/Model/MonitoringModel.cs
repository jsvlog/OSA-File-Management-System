using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSA_File_Management_System.Model
{
    public class MonitoringModel : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private string? documentType;
        public string? DocumentType
        {
            get { return documentType; }
            set { documentType = value; OnPropertyChanged("DocumentType"); }
        }

        private string? municipality;
        public string? Municipality
        {
            get { return municipality; }
            set { municipality = value; OnPropertyChanged("Municipality"); }
        }

        private int year;
        public int Year
        {
            get { return year; }
            set { year = value; OnPropertyChanged("Year"); }
        }

        private DateTime? dateSubmitted;
        public DateTime? DateSubmitted
        {
            get { return dateSubmitted; }
            set { dateSubmitted = value; OnPropertyChanged("DateSubmitted"); }
        }

        private string? status;
        public string? Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged("Status"); }
        }

        private string? remarks;
        public string? Remarks
        {
            get { return remarks; }
            set { remarks = value; OnPropertyChanged("Remarks"); }
        }

        private string? pdfLink;
        public string? PdfLink
        {
            get { return pdfLink; }
            set { pdfLink = value; OnPropertyChanged("PdfLink"); }
        }
    }

    public class MonitoringGridRow : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string? municipality;
        public string? Municipality
        {
            get { return municipality; }
            set { municipality = value; OnPropertyChanged("Municipality"); }
        }

        public Dictionary<int, YearStatus> YearStatuses { get; set; } = new Dictionary<int, YearStatus>();
    }

    public class YearStatus
    {
        public int SubmissionId { get; set; }
        public string? Municipality { get; set; }
        public int Year { get; set; }
        public string? DocumentType { get; set; }
        public string? Status { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? Remarks { get; set; }
        public string? PdfLink { get; set; }
    }
}