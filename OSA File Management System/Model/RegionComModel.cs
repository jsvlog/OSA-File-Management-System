using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSA_File_Management_System.Model
{
    class RegionComModel : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private int? id;

        public int? Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private DateTime? dateReceived;

        public DateTime? DateReceived
        {
            get { return dateReceived; }
            set { dateReceived = value;  }
        }

        private DateTime? documentDate;

        public DateTime? DocumentDate
        {
            get { return documentDate; }
            set { documentDate = value;}
        }

        private string? typeOfDocs;

        public string? TypeOfDocs
        {
            get { return typeOfDocs; }
            set { typeOfDocs = value;  }
        }

        private string? addressee;

        public string? Addressee
        {
            get { return addressee; }
            set { addressee = value; }
        }

        private string? subjectParticulars;

        public string? SubjectParticulars
        {
            get { return subjectParticulars; }
            set { subjectParticulars = value; }
        }

        private string? details;

        public string? Details
        {
            get { return details; }
            set { details = value; }
        }

        private string? refNumber;

        public string? RefNumber
        {
            get { return refNumber; }
            set { refNumber = value; }
        }

        private string? municipality;

        public string? Municipality
        {
            get { return municipality; }
            set { municipality = value; }
        }

        private string? barangay;

        public string? Barangay
        {
            get { return barangay; }
            set { barangay = value; }
        }

        private string? receivedFrom;

        public string? ReceivedFrom
        {
            get { return receivedFrom; }
            set { receivedFrom = value;  }
        }

        private DateTime? dateSentOutToTeam;

        public DateTime? DateSentOutToTeam
        {
            get { return dateSentOutToTeam; }
            set { dateSentOutToTeam = value;  }
        }

        private string? receiver;

        public string? Receiver
        {
            get { return receiver; }
            set { receiver = value;  }
        }

        private string? location;

        public string? Location
        {
            get { return location; }
            set { location = value;  }
        }

        private bool? actionableDoc;

        public bool? ActionableDoc
        {
            get { return actionableDoc; }
            set { actionableDoc = value; }
        }

        private DateTime? dateDeadline;

        public DateTime? DateDeadline
        {
            get { return dateDeadline; }
            set { dateDeadline = value;  }
        }

        private string? remarks;

        public string? Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        private string? trackingCode;

        public string? TrackingCode
        {
            get { return trackingCode; }
            set { trackingCode = value;  }
        }

        private string? direction;

        public string? Direction
        {
            get { return direction; }
            set { direction = value;  }
        }

        private string? scannedCopy;

        public string? ScannedCopy
        {
            get { return scannedCopy; }
            set { scannedCopy = value;  }
        }



        //this part is for Document to region
        private string? numberOfCopies;

        public string? NumberOfCopies
        {
            get { return numberOfCopies; }
            set { numberOfCopies = value; }
        }

        private DateTime? dateSignBySA;

        public DateTime? DateSignBySA
        {
            get { return dateSignBySA; }
            set { dateSignBySA = value;  }
        }

        private DateTime? dateReceiveByRegion;

        public DateTime? DateReceiveByRegion
        {
            get { return dateReceiveByRegion; }
            set { dateReceiveByRegion = value; }
        }

        private DateTime? dateSentOutToRegion;

        public DateTime? DateSentOutToRegion
        {
            get { return dateSentOutToRegion; }
            set { dateSentOutToRegion = value; }
        }

        private string? lbcRefNumber;

        public string? LbcRefNumber
        {
            get { return lbcRefNumber; }
            set { lbcRefNumber = value;}
        }

        private bool? isHighlighted;

        public bool? IsHighlighted
        {
            get { return isHighlighted; }
            set { isHighlighted = value; }
        }

        private bool? visib;

        public bool? Visib
        {
            get { return visib; }
            set { visib = value;  }
        }






    }
}
