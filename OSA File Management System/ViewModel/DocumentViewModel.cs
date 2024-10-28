using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        //for sub ViewModel
        public RegionComViewModel regionComViewModel;

        private DocumentServices documentServices;


        private ObservableCollection<Document> documentList;
        public ObservableCollection<Document> DocumentList
        {
            get { return documentList; }
            set { documentList = value; OnPropertyChanged("DocumentList"); }
        }


        public DocumentViewModel()
        {
            regionComViewModel = new RegionComViewModel();
            addFormData = new Document();
            editFormData = new Document();
            documentServices = new DocumentServices();
            showAddForm = new RelayCommand(OpenAddDocumentForm);
            showEditForm = new RelayCommand(OpenEditDocumentForm);
            selectFile = new RelayCommand(OpenSelectFile);
            closeAddForm = new RelayCommand(CloseAddForms);
            addDocument = new RelayCommand(AddDocumentMethod);
            deleteDocument = new RelayCommand(DeleteDocumentMethod);
            viewPdf = new RelayCommand(OpenPdfFile);
            saveEditedDocument = new RelayCommand(SaveEditedCommand);
            closeEditForm = new RelayCommand(CloseEditForms); 
            btnSearchInventory = new RelayCommand(ExecuteSearch);
            printPreview = new RelayCommand(ShowPrintPreview);
            printDocumentInventory = new RelayCommand(PrintDocument);
            LoadData();
        }

        public void LoadData()
        {
            DocumentList = documentServices.GetAllDocuments();
        }



        #region Inventory

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

        #region Close Add Form
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
        private Document editFormData;

        public Document EditFormData
        {
            get { return editFormData; }
            set { editFormData = value; OnPropertyChanged("EditFormData"); }
        }


        private RelayCommand showEditForm;

        public RelayCommand ShowEditForm
        {
            get { return showEditForm; }
        }

        private EditFormInventory popupEditForm;

        private void OpenEditDocumentForm(object parameter)
        {
            // Create the popup window
            popupEditForm = new EditFormInventory();

            // Bind ViewModel to the popup window
            popupEditForm.DataContext = this;



            if (parameter is Document documentToDelete)
            {
                EditFormData = documentToDelete;
            }
            // Show the popup window
            popupEditForm.ShowDialog();
            LoadData();

        }

        #endregion

        #region save Edit Document
        private RelayCommand saveEditedDocument;

        public RelayCommand SaveEditedDocument
        {
            get { return saveEditedDocument; }
        }

        private void SaveEditedCommand()
        {
            try
            {
                var isSaved = documentServices.SaveEditedDocument(EditFormData);
                if (isSaved)
                {
                    MessageBox.Show("Edited Successfull");
                    popupEditForm.Close();
                    LoadData();
                    EditFormData = new Document(); //to clear the fields after saving
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        #endregion

        #region Close Edit Form
        private RelayCommand closeEditForm;

        public RelayCommand CloseEditForm
        {
            get { return closeEditForm; }
        }

        private void CloseEditForms()
        {
            popupEditForm.Close();
        }

        #endregion

        #region Search 
        private string searchTextInventory;

        public string SearchTextInventory
        {
            get { return searchTextInventory; }
            set { searchTextInventory = value; OnPropertyChanged("SearchText"); }
        }


        private RelayCommand btnSearchInventory;

        public RelayCommand BtnSearchInventory
        {
            get { return btnSearchInventory; }
        }

        private void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchTextInventory))
            {
                DocumentList = documentServices.GetAllDocuments();
            }
            else
            {
                LoadData();
                var filteredDocuments = DocumentList.Where(d =>
                d.Date.HasValue && d.Date.Value.ToString("yyyy-MM-dd").Contains(SearchTextInventory, StringComparison.OrdinalIgnoreCase) ||
                d.Type.Contains(SearchTextInventory, StringComparison.OrdinalIgnoreCase) ||
                d.Description.Contains(SearchTextInventory, StringComparison.OrdinalIgnoreCase) ||
                d.Status.Contains(SearchTextInventory, StringComparison.OrdinalIgnoreCase) ||
                d.Location.Contains(SearchTextInventory, StringComparison.OrdinalIgnoreCase) ||
                d.Remarks.Contains(SearchTextInventory, StringComparison.OrdinalIgnoreCase))
                .ToList();

                // Bind the filtered list to the DataGrid
                DocumentList = new ObservableCollection<Document>(filteredDocuments);
            }
        }


        #endregion

        #region print
        private RelayCommand printPreview;

        public RelayCommand PrintPreview
        {
            get { return printPreview; }
        }

        public void ShowPrintPreview()
        {
            // Create the FlowDocument
            FlowDocument doc = CreateDocumentForPrint();

            // Show the preview window
            InventoryPrintPreview previewWindow = new InventoryPrintPreview();
            previewWindow.DataContext = this;
            previewWindow.DocumentReader.Document = doc;
            previewWindow.ShowDialog();
        }

        private FlowDocument CreateDocumentForPrint()
        {
            FlowDocument doc = new FlowDocument();

            // Set page dimensions (adjust as needed)
            doc.PageWidth = 800;  // Width in device-independent units (1/96 inch)
            doc.PageHeight = 1100; // Height in device-independent units
            doc.ColumnWidth = 600; // Width of a single column


            // Create an image for the header
            Image headerImage = new Image();
            headerImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/header.png"));
            headerImage.Width = 700;
            headerImage.Height = 200;
            headerImage.HorizontalAlignment = HorizontalAlignment.Center;

            BlockUIContainer imageContainer = new BlockUIContainer(headerImage);
            doc.Blocks.Add(imageContainer);

            Table table = new Table();
            table.CellSpacing = 5;
            table.BorderBrush = Brushes.Black;
            table.BorderThickness = new Thickness(1);

            int numberOfColumns = 5;
            for (int i = 0; i < numberOfColumns; i++)
            {
                table.Columns.Add(new TableColumn());
            }

            TableRow headerRow = new TableRow();
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Date")))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Type")))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Description")))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Location")))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Remarks")))));
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(headerRow);

            foreach (var docItem in DocumentList)
            {
                TableRow dataRow = new TableRow();
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(docItem.Date.ToString()))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(docItem.Type))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(docItem.Description))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(docItem.Location))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(docItem.Remarks))));
                table.RowGroups[0].Rows.Add(dataRow);
            }

            doc.Blocks.Add(table);
            doc.PagePadding = new Thickness(50);
            return doc;
        }


        //this is the part in final printing
        private RelayCommand printDocumentInventory;

        public RelayCommand PrintDocumentInventory
        {
            get { return printDocumentInventory; }
        }

        private void PrintDocument()
        {
            PrintDialog printDlg = new PrintDialog();
            FlowDocument doc = CreateDocumentForPrint();
            if (printDlg.ShowDialog() == true)
            {
                IDocumentPaginatorSource idpSource = doc;
                printDlg.PrintDocument(idpSource.DocumentPaginator, "Document Report");
            }
        }

        #endregion

        #endregion

    }
}
