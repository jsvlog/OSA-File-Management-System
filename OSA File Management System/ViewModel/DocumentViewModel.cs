using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;

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
            set { documentList = value; OnPropertyChanged("DocumentList"); }
        }


        public DocumentViewModel()
        {
            
            addFormData = new Document();
            documentServices = new DocumentServices();
            showAddForm = new RelayCommand(OpenAddDocumentForm);
            showEditForm = new RelayCommand(OpenEditDocumentForm);
            selectFile = new RelayCommand(OpenSelectFile);
            closeAddForm = new RelayCommand(CloseAddForms);
            addDocument = new RelayCommand(AddDocumentMethod);
            deleteDocument = new RelayCommand(DeleteDocumentMethod);
            viewPdf = new RelayCommand(OpenPdfFile);
            LoadData();
        }

        public void LoadData()
        {
            DocumentList = documentServices.GetAllDocuments();
        }



        #region Show Add Form
        private RelayCommand showAddForm;

        public RelayCommand ShowAddForm
        {
            get { return showAddForm; }
        }

        private AddFormInventory popup;
        private void OpenAddDocumentForm()
        {
            // Create the popup window
            popup = new AddFormInventory();

            // Bind ViewModel to the popup window
            popup.DataContext = this;

            // Show the popup window
            popup.ShowDialog();
        }
        #endregion

        #region Add Document
        private Document addFormData;

        public Document AddFormData
        {
            get { return addFormData; }
            set { addFormData = value; OnPropertyChanged("AddFormData"); }
        }

        private RelayCommand addDocument;

        public RelayCommand AddDocument
        {
            get { return addDocument; }
        }

        private void AddDocumentMethod()
        {
            try
            {
                var IsSaved = documentServices.addDocument(AddFormData);
                if (IsSaved)
                {
                    
                    MessageBox.Show("Saving Successfull");
                    popup.Close();
                    LoadData();
                    AddFormData = new Document(); //to clear the fields after saving
                }
                else { MessageBox.Show("error saving"); }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }





        #endregion

        #region Select pdf Copy
        private RelayCommand selectFile;

        public RelayCommand SelectFile
        {
            get { return selectFile; }
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
                AddFormData.ScannedCopy = openFileDialog.FileName.ToString();
            }
        }
        #endregion

        #region Close Form
        private RelayCommand closeAddForm;

        public RelayCommand CloseAddForm
        {
            get { return closeAddForm; }
        }

        private void CloseAddForms()
        {
            popup.Close();
        }
        #endregion

        #region Delete Document
        private RelayCommand deleteDocument;

        public RelayCommand DeleteDocument
        {
            get { return deleteDocument; }
            set { deleteDocument = value; }
        }

        private void DeleteDocumentMethod(object parameter)
        {
            if (parameter is Document documentToDelete)
            {
                // Confirm the deletion 
                var result = MessageBox.Show($"Are you sure you want to delete this document?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Call your documentServices to delete the document from the database
                    bool isDeleted = documentServices.DeleteDocument(documentToDelete);

                    // If deletion was successful, remove it from the ObservableCollection
                    if (isDeleted)
                    {
                        LoadData();
                        MessageBox.Show("Document deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Error deleting document.");
                    }
                }
            }

        }
        #endregion

        #region View PDF Copy
        private RelayCommand viewPdf;

        public RelayCommand ViewPdf
        {
            get { return viewPdf; }
        }

        private void OpenPdfFile(object parameter)
        {

            if (parameter is Document documentObj)
            {
                try
                {
                    // Use Process.Start to open the PDF file
                    string pdfPath = documentObj.ScannedCopy.ToString();
                    Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    string pdfPath = documentObj.ScannedCopy.ToString();
                    MessageBox.Show($"Failed to open PDF: {ex.Message} {pdfPath}");
                }
            }
            else
            {
                MessageBox.Show("No valid PDF file path provided.");
            }
        }

        #endregion

        #region Show edit Form
        private RelayCommand showEditForm;

        public RelayCommand ShowEditForm
        {
            get { return showEditForm; }
        }

        private EditFormInventory popupEditForm;

        private void OpenEditDocumentForm()
        {
            // Create the popup window
            popupEditForm = new EditFormInventory();

            // Bind ViewModel to the popup window
            popupEditForm.DataContext = this;

            // Show the popup window
            popupEditForm.ShowDialog();
        }

        #endregion







    }
}
