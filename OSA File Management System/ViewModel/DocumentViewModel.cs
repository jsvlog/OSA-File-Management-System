using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using OSA_File_Management_System.View;
using OSA_File_Management_System.Model;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using OSA_File_Management_System.Commands;
using Mysqlx.Crud;
using Microsoft.Win32;
using System.ComponentModel;

namespace OSA_File_Management_System.ViewModel
{
    class DocumentViewModel : INotifyPropertyChanged
    {
        #region Notify Property Change
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion



        private DocumentServices documentServices;


        private ObservableCollection<Document> documentList;
        public ObservableCollection<Document> DocumentList 
        {
            get { return documentList; }
            set { documentList = value; OnPropertyChanged("FilePath"); }
        }

        public DocumentViewModel()
        {
            documentServices = new DocumentServices();
            DocumentList = documentServices.GetAllDocuments();
            showAddForm = new RelayCommand(OpenAddDocumentForm);
            selectFile = new RelayCommand(OpenSelectFile);
        }



        #region Show Add Form
        private RelayCommand showAddForm;

        public RelayCommand ShowAddForm
        {
            get { return showAddForm; }
        }

        private void OpenAddDocumentForm()
        {
            // Create the popup window
            AddFormInventory popup = new AddFormInventory();

            // Bind ViewModel to the popup window
            popup.DataContext = this;

            // Show the popup window
            popup.ShowDialog();
        }
        #endregion


        #region Select pdf Copy
        private RelayCommand selectFile;

        public RelayCommand SelectFile
        {
            get { return selectFile; }
        }


        private string filePath = "ito nga";

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; OnPropertyChanged("FilePath"); }
        }

        private void OpenSelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the selected file path
                FilePath = openFileDialog.FileName.ToString();
            }
        }
        #endregion





    }
}
