using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OSA_File_Management_System.Model
{
    public class Document : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged("Status"); }
        }

        private string location;

        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged("Location"); }
        }

        private string remarks;

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; OnPropertyChanged("Remarks"); }
        }

        private string scannedCopy;

        public string ScannedCopy
        {
            get { return scannedCopy; }
            set { scannedCopy = value; OnPropertyChanged("ScannedCopy"); }
        }






    }
}
