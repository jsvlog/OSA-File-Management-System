using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;
using OSA_File_Management_System.View.RegionCom;
using OSA_File_Management_System.View.RegionComView;

namespace OSA_File_Management_System.ViewModel
{
    class RegionComViewModel : INotifyPropertyChanged
    {
        #region Notify Property Change
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private RegionComServices regionComServices;


        public RegionComViewModel()
        {
            regionComServices = new RegionComServices();
            btnLoadData = new RelayCommand(LoadAllRegionCom);
            showAddToRegion = new RelayCommand(OpenAddDocumentForm);
            addToRegion = new RelayCommand(AddToRegionCommand);
            closeAddToRegion = new RelayCommand(CloseAddToRegionCommand);
            addFromRegion = new RelayCommand(AddFromRegionCommand);
            closeAddFromRegion = new RelayCommand(CloseAddFromRegionCommand);
            addToRegionData = new RegionComModel();
            addFromRegionData = new RegionComModel();
            editFromRegionData = new RegionComModel();
            selectFile = new RelayCommand(OpenSelectFile);
            showAddFromRegion = new RelayCommand(OpenAddFromRegionForm);
            selectFileFromRegion = new RelayCommand(OpenSelectFileFromRegion);
            deleteData = new RelayCommand(DeleteDataMethod);
            showEditFromOrTo = new RelayCommand(OpenEditFromOrTo);
            saveEditedToRegion = new RelayCommand(SaveEditedToRegionCommand);
            saveEditedFromRegion = new RelayCommand(SaveEditedFromRegionCommand);
            closeEditToRegion = new RelayCommand(CloseEditToRegionCommand);
            closeEditFromRegion = new RelayCommand(CloseEditFromRegionCommand);
            viewPdfToRegion = new RelayCommand(OpenPdfToRegion);
            btnSearchRegionCom = new RelayCommand(ExecuteSearch);
            fromRegionFullData = new RegionComModel();
            toRegionFullData = new RegionComModel();
            showFullData = new RelayCommand(OpenFullDetailsForm);
            viewPdfFullData = new RelayCommand(ViewPdfFullDataCommand);
            showEditFromFullData = new RelayCommand(ShowEditFromFullDataCommand);
            deleteFromFullData = new RelayCommand(DeleteFromFullDataCommand);
            showFullDatailsFromFullData = new RelayCommand(OpenFullDetailsFromFullData);
            LoadAllRegionCom();
        }




        #region Load All Region Com List
        private ObservableCollection<RegionComModel> regionComList;

        public ObservableCollection<RegionComModel> RegionComList
        {
            get { return regionComList; }
            set { regionComList = value; OnPropertyChanged("RegionComList"); }
        }

        public void LoadAllRegionCom()
        {
            RegionComList = regionComServices.GetAllRegionCom();
        }
        #endregion

        #region Refresh Data of Gid 
        private RelayCommand btnLoadData;

        public RelayCommand BtnLoadData
        {
            get { return btnLoadData; }
            set { btnLoadData = value; OnPropertyChanged("BtnLoadData"); }
        }


        #endregion

        #region Filter if To Region or From Region or All is to be displayed
        // RadioButton IsChecked properties
        private bool _isAllChecked;
        public bool IsAllChecked
        {
            get { return _isAllChecked; }
            set
            {
                _isAllChecked = value;
                FilterDocs();
                OnPropertyChanged("IsAllChecked");
            }
        }

        private bool _isToRegionChecked;
        public bool IsToRegionChecked
        {
            get { return _isToRegionChecked; }
            set
            {
                _isToRegionChecked = value;
                FilterDocs();
                OnPropertyChanged("IsToRegionChecked");
            }
        }

        private bool _isFromRegionChecked;
        public bool IsFromRegionChecked
        {
            get { return _isFromRegionChecked; }
            set
            {
                _isFromRegionChecked = value;
                FilterDocs();
                OnPropertyChanged("IsFromRegionChecked");
            }
        }

        private bool isHidden;

        public bool IsHidden
        {
            get { return isHidden; }
            set { isHidden = value; OnPropertyChanged("IsHidden"); }
        }

        //to show if you want to show To region, From Region or All
        private void FilterDocs()
        {
            if (IsAllChecked)
            {
                foreach (var doc in RegionComList)
                {
                    doc.Visib = true;
                }

            }
            else if (IsToRegionChecked)
            {
                foreach (var doc in RegionComList)
                {
                    if (doc.Direction == "To Region")
                    {
                        doc.Visib = true;
                    }
                    else if (doc.Direction == "From Region")
                    {
                        doc.Visib = false;
                    }
                }

            }
            else if (IsFromRegionChecked)
            {
                foreach (var doc in RegionComList)
                {
                    if (doc.Direction == "From Region")
                    {
                        doc.Visib = true;
                    }
                    else if (doc.Direction == "To Region")
                    {
                        doc.Visib = false;
                    }

                }

            }



        }


        #endregion

        #region Show Add To Region Form
        private RelayCommand showAddToRegion;

        public RelayCommand ShowAddToRegion
        {
            get { return showAddToRegion; }
        }
        private AddToRegion popup;
        private void OpenAddDocumentForm()
        {
            // Create the popup window
            popup = new AddToRegion();

            // Bind ViewModel to the popup window
            popup.DataContext = this;
            popup.Top = 10;
            popup.Left = 100;

            // Show the popup window
            popup.ShowDialog();
        }

        #endregion

        #region Close Add to Region Form
        private RelayCommand closeAddToRegion;

        public RelayCommand CloseAddToRegion
        {
            get { return closeAddToRegion; }
        }

        private void CloseAddToRegionCommand()
        {
            popup.Close();
        }
        #endregion

        #region Add To Region Data
        private RegionComModel addToRegionData;

        public RegionComModel AddToRegionData
        {
            get { return addToRegionData; }
            set { addToRegionData = value; OnPropertyChanged("AddToRegionData"); }

        }

        private RelayCommand addToRegion;

        public RelayCommand AddToRegion
        {
            get { return addToRegion; }
        }

        private void AddToRegionCommand()
        {
            try
            {
                var IsSaved = regionComServices.addToRegionCom(AddToRegionData);
                if (IsSaved)
                {

                    MessageBox.Show("Saving Successfull");
                    popup.Close();
                    LoadAllRegionCom();
                    AddToRegionData = new RegionComModel(); //to clear the fields after saving
                }
                else { MessageBox.Show("error saving"); }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Select pdf Copy To Region
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
                AddToRegionData.ScannedCopy = openFileDialog.FileName.ToString();
            }
        }
        #endregion

        #region Show Add From Region Form
        private RelayCommand showAddFromRegion;

        public RelayCommand ShowAddFromRegion
        {
            get { return showAddFromRegion; }
        }
        private AddFromRegion popupAddFrom;
        private void OpenAddFromRegionForm()
        {
            // Create the popup window
            popupAddFrom = new AddFromRegion();

            // Bind ViewModel to the popup window
            popupAddFrom.DataContext = this;

            popupAddFrom.Top = 10;
            popupAddFrom.Left = 100;

            // Show the popup window
            popupAddFrom.ShowDialog();
        }

        #endregion

        #region Close Add from Region Form
        private RelayCommand closeAddFromRegion;

        public RelayCommand CloseAddFromRegion
        {
            get { return closeAddFromRegion; }
        }

        private void CloseAddFromRegionCommand()
        {
            popupAddFrom.Close();
        }
        #endregion

        #region Add From Region Data
        private RegionComModel addFromRegionData;

        public RegionComModel AddFromRegionData
        {
            get { return addFromRegionData; }
            set { addFromRegionData = value; OnPropertyChanged("AddFromRegionData"); }

        }

        private RelayCommand addFromRegion;

        public RelayCommand AddFromRegion
        {
            get { return addFromRegion; }
        }

        private void AddFromRegionCommand()
        {
            try
            {
                AddFromRegionData.Direction = "From Region";
                var IsSaved = regionComServices.addFromRegionCom(AddFromRegionData);
                if (IsSaved)
                {

                    MessageBox.Show("Saving Successfull");
                    popupAddFrom.Close();
                    LoadAllRegionCom();
                    AddFromRegionData = new RegionComModel(); //to clear the fields after saving
                }
                else { MessageBox.Show("error saving"); }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Select pdf Copy From Region
        private RelayCommand selectFileFromRegion;

        public RelayCommand SelectFileFromRegion
        {
            get { return selectFileFromRegion; }
        }

        private void OpenSelectFileFromRegion()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the selected file path
                AddFromRegionData.ScannedCopy = openFileDialog.FileName.ToString();
            }
        }
        #endregion

        #region Delete Data
        private RelayCommand deleteData;

        public RelayCommand DeleteData
        {
            get { return deleteData; }
            set { deleteData = value; }
        }

        private void DeleteDataMethod(object parameter)
        {
            if (parameter is RegionComModel documentToDelete)
            {
                // Confirm the deletion 
                var result = MessageBox.Show($"Are you sure you want to delete this document?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Call your documentServices to delete the document from the database
                    bool isDeleted = regionComServices.DeleteData(documentToDelete);

                    // If deletion was successful, remove it from the ObservableCollection
                    if (isDeleted)
                    {
                        LoadAllRegionCom();
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

        #region Show edit (From Region or To Region )


        private RegionComModel editFromRegionData;

        public RegionComModel EditFromRegionData
        {
            get { return editFromRegionData; }
            set { editFromRegionData = value; OnPropertyChanged("EditFromRegionData"); }
        }

        private RegionComModel editToRegionData;

        public RegionComModel EditToRegionData
        {
            get { return editToRegionData; }
            set { editToRegionData = value; OnPropertyChanged("EditToRegionData"); }
        }


        private RelayCommand showEditFromOrTo;

        public RelayCommand ShowEditFromOrTo
        {
            get { return showEditFromOrTo; }
        }

        private EditFromRegion popupEditFromRegion; //put it outside the method to close from another method
        private EditToRegion popupEditToRegion;//put it outside the method to close from another method

        private void OpenEditFromOrTo(object parameter)
        {
            if (parameter is RegionComModel documentToEdit)
            {
                if (documentToEdit.Direction == "To Region")
                {
                    EditToRegionData = documentToEdit;
                    // Create the popup window
                    popupEditToRegion = new EditToRegion();

                    // Bind ViewModel to the popup window
                    popupEditToRegion.DataContext = this;
                    popupEditToRegion.Top = 10;
                    popupEditToRegion.Left = 100;

                    // Show the popup window
                    popupEditToRegion.ShowDialog();

                }
                else if (documentToEdit.Direction == "From Region")
                {
                    EditFromRegionData = documentToEdit;
                    // Create the popup window
                    popupEditFromRegion = new EditFromRegion();

                    // Bind ViewModel to the popup window
                    popupEditFromRegion.DataContext = this;
                    popupEditFromRegion.Top = 10;
                    popupEditFromRegion.Left = 100;

                    // Show the popup window
                    popupEditFromRegion.ShowDialog();

                }
                else
                {
                    MessageBox.Show(" Direction not found");
                }

            }

        }

        #endregion

        #region save Edit To Region
        private RelayCommand saveEditedToRegion;

        public RelayCommand SaveEditedToRegion
        {
            get { return saveEditedToRegion; }
        }

        private void SaveEditedToRegionCommand()
        {
            try
            {
                var isSaved = regionComServices.SaveEditedToRegion(EditToRegionData);
                if (isSaved)
                {
                    MessageBox.Show("Edited Successfully");
                    popupEditToRegion.Close();
                    LoadAllRegionCom();
                    EditToRegionData = new RegionComModel(); //to clear the fields after saving
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }




        #endregion

        #region save Edit From Region
        private RelayCommand saveEditedFromRegion;

        public RelayCommand SaveEditedFromRegion
        {
            get { return saveEditedFromRegion; }
        }

        private void SaveEditedFromRegionCommand()
        {
            try
            {
                var isSaved = regionComServices.SaveEditedFromRegion(EditFromRegionData);
                if (isSaved)
                {
                    MessageBox.Show("Edited Successfully");
                    popupEditFromRegion.Close();
                    LoadAllRegionCom();
                    EditFromRegionData = new RegionComModel(); //to clear the fields after saving
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Close Edit To Region Popup
        private RelayCommand closeEditToRegion;

        public RelayCommand CloseEditToRegion
        {
            get { return closeEditToRegion; }
        }

        private void CloseEditToRegionCommand()
        {
            popupEditToRegion.Close();
        }

        #endregion

        #region Close Edit From Region Popup
        private RelayCommand closeEditFromRegion;

        public RelayCommand CloseEditFromRegion
        {
            get { return closeEditFromRegion; }
        }

        private void CloseEditFromRegionCommand()
        {
            popupEditFromRegion.Close();
        }

        #endregion

        #region View PDF (To and From) Region
        private RelayCommand viewPdfToRegion;

        public RelayCommand ViewPdfToRegion
        {
            get { return viewPdfToRegion; }
        }

        private void OpenPdfToRegion(object parameter)
        {

            if (parameter is RegionComModel documentObj)
            {
                try
                {
                    if (documentObj.ScannedCopy != null)
                    {
                        // Use Process.Start to open the PDF file
                        string pdfPath = documentObj.ScannedCopy.ToString();
                        Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                    }
                    else
                    {
                        MessageBox.Show("No Pdf File uploded");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open PDF: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No valid PDF file path provided.");
            }
        }

        #endregion

        #region Search 
        private string searchTextRegionCom;

        public string SearchTextRegionCom
        {
            get { return searchTextRegionCom; }
            set { searchTextRegionCom = value; OnPropertyChanged("SearchTextRegionCom"); }
        }


        private RelayCommand btnSearchRegionCom;

        public RelayCommand BtnSearchRegionCom
        {
            get { return btnSearchRegionCom; }
        }

        private void ExecuteSearch()
        {
            if (string.IsNullOrEmpty(SearchTextRegionCom))
            {
                LoadAllRegionCom();
            }
            else
            {
                LoadAllRegionCom();
                var filteredDocuments = RegionComList.Where(d =>
                     (d.DateReceived.HasValue && d.DateReceived.Value.ToString("MM-dd-yyyy").Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase)) ||
                     (d.DocumentDate.HasValue && d.DocumentDate.Value.ToString("MM-dd-yyyy").Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase)) ||
                     (!string.IsNullOrEmpty(d.TypeOfDocs) && d.TypeOfDocs.Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase)) ||
                     (!string.IsNullOrEmpty(d.SubjectParticulars) && d.SubjectParticulars.Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase)) ||
                     (!string.IsNullOrEmpty(d.RefNumber) && d.RefNumber.Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase)) ||
                     (!string.IsNullOrEmpty(d.ReceivedFrom) && d.ReceivedFrom.Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase)) ||
                     (!string.IsNullOrEmpty(d.Remarks) && d.Remarks.Contains(SearchTextRegionCom, StringComparison.OrdinalIgnoreCase))
                 ).ToList();

                // Bind the filtered list to the DataGrid
                RegionComList = new ObservableCollection<RegionComModel>(filteredDocuments);
            }
        }
        #endregion

        #region Show Full Details (From Region or To Region )

        private RegionComModel fromRegionFullData;

        public RegionComModel FromRegionFullData
        {
            get { return fromRegionFullData; }
            set { fromRegionFullData = value; OnPropertyChanged("FromRegionFullData"); }
        }

        private RegionComModel toRegionFullData;

        public RegionComModel ToRegionFullData
        {
            get { return toRegionFullData; }
            set { toRegionFullData = value; OnPropertyChanged("ToRegionFullData"); }
        }


        private RelayCommand showFullData;

        public RelayCommand ShowFullData
        {
            get { return showFullData; }
        }

        private FullDetailsFromRegion popupFullDetailsFromRegion;        //put it outside the method to close from another method
        private FullDetailsToRegion popupFullDetailsToRegion;            //put it outside the method to close from another method
        public string ActionableDocDisplayFrom => FromRegionFullData.ActionableDoc == true ? "Yes" : FromRegionFullData.ActionableDoc == false ? "No" : "N/A"; //This is to display Yes or No in the textbox instead of True or False
        public string ActionableDocDisplayTo => ToRegionFullData.ActionableDoc == true ? "Yes" : ToRegionFullData.ActionableDoc == false ? "No" : "N/A"; //This is to display Yes or No in the textbox instead of True or False

        //This data is for dispalying all related document to datagrid
        private ObservableCollection<RegionComModel> filteredDocs;
        public ObservableCollection<RegionComModel> FilteredDocs
        {
            get { return filteredDocs; }
            set { filteredDocs = value; OnPropertyChanged("FilteredDocs"); }
        }



        //Display the Full Details Window 
        private void OpenFullDetailsForm(object parameter)
        {
            if (parameter is RegionComModel documentToShow)
            {
                if (documentToShow.Direction == "To Region")
                {
                    ToRegionFullData = documentToShow;
                    // Create the popup window
                    popupFullDetailsToRegion = new FullDetailsToRegion();

                    // Bind ViewModel to the popup window
                    popupFullDetailsToRegion.DataContext = this;
                    popupFullDetailsToRegion.Top = 10;
                    popupFullDetailsToRegion.Left = 100;

                    // Filter RegionComList by TrackingID
                    var filteredList = RegionComList.Where(doc => doc.TrackingCode == ToRegionFullData.TrackingCode).ToList();
                    FilteredDocs = new ObservableCollection<RegionComModel>(filteredList);

                    //this is to highlight the current document from filtered docs
                    foreach (var item in FilteredDocs)
                    {
                        if (ToRegionFullData.Id == item.Id)
                        {
                            item.IsHighlighted = true;
                        }
                        else
                        {
                            item.IsHighlighted = false;
                        }
                    }

                    // Show the popup window
                    popupFullDetailsToRegion.ShowDialog();
                    ToRegionFullData = new RegionComModel(); //to refresh or empty the value of toRegionFullData after closing
                    FilteredDocs = new ObservableCollection<RegionComModel>();

                }
                else if (documentToShow.Direction == "From Region")
                {
                    FromRegionFullData = documentToShow;
                    // Create the popup window
                    popupFullDetailsFromRegion = new FullDetailsFromRegion();

                    // Bind ViewModel to the popup window
                    popupFullDetailsFromRegion.DataContext = this;
                    popupFullDetailsFromRegion.Top = 10;
                    popupFullDetailsFromRegion.Left = 100;

                    // Filter RegionComList by TrackingID
                    var filteredListFrom = RegionComList.Where(doc => doc.TrackingCode == FromRegionFullData.TrackingCode).ToList();
                    FilteredDocs = new ObservableCollection<RegionComModel>(filteredListFrom);

                    //this is to highlight the current document from filtered docs
                    foreach (var item in FilteredDocs)
                    {
                        if (FromRegionFullData.Id == item.Id)
                        {
                            item.IsHighlighted = true;
                        }
                        else
                        {
                            item.IsHighlighted = false;
                        }
                    }

                    // Show the popup window
                    popupFullDetailsFromRegion.ShowDialog();
                    FromRegionFullData = new RegionComModel(); //to refresh or empty the value of fromRegionFullData after closing
                    FilteredDocs = new ObservableCollection<RegionComModel>();


                }
                else
                {
                    MessageBox.Show(" Direction not found in viewinf full Details");
                }

            }

        }

        #endregion

        #region View PDF From Full Data Window
        private RelayCommand viewPdfFullData;

        public RelayCommand ViewPdfFullData
        {
            get { return viewPdfFullData; }
        }


        private void ViewPdfFullDataCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(ToRegionFullData.ScannedCopy) && !string.IsNullOrEmpty(FromRegionFullData.ScannedCopy))
                {
                    // Use Process.Start to open the PDF file
                    string pdfPath = FromRegionFullData.ScannedCopy;
                    Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                }
                else if (!string.IsNullOrEmpty(ToRegionFullData.ScannedCopy) && string.IsNullOrEmpty(FromRegionFullData.ScannedCopy))
                {
                    // Use Process.Start to open the PDF file
                    string pdfPath = ToRegionFullData.ScannedCopy;
                    Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("PDF file Empty");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }




        }

        #endregion

        #region Edit Data From Full Data Window

        private RelayCommand showEditFromFullData;

        public RelayCommand ShowEditFromFullData
        {
            get { return showEditFromFullData; }
            set { showEditFromFullData = value; }
        }

        private void ShowEditFromFullDataCommand()
        {
            RegionComModel parameterModel = null;

            // Check which full data is available and set the parameter model accordingly
            if (!string.IsNullOrEmpty(FromRegionFullData.Direction) && string.IsNullOrEmpty(ToRegionFullData.Direction))
            {
                parameterModel = FromRegionFullData;
            }
            else if (!string.IsNullOrEmpty(ToRegionFullData.Direction) && string.IsNullOrEmpty(FromRegionFullData.Direction))
            {
                parameterModel = ToRegionFullData;
            }

            // If parameterModel is set, proceed to open the edit window
            if (parameterModel != null)
            {
                OpenEditFromOrTo(parameterModel);
            }
            else
            {
                MessageBox.Show("Problem Showing Edit Form");
            }

        }


        #endregion

        #region Delete From Full Data Window
        private RelayCommand deleteFromFullData;

        public RelayCommand DeleteFromFullData
        {
            get { return deleteFromFullData; }
            set { deleteFromFullData = value; }
        }

        private void DeleteFromFullDataCommand()
        {
            RegionComModel parameterModel = null;

            // Check which full data is available and set the parameter model accordingly
            if (!string.IsNullOrEmpty(FromRegionFullData.Direction) && string.IsNullOrEmpty(ToRegionFullData.Direction))
            {
                parameterModel = FromRegionFullData;
                // Confirm the deletion 
                var result = MessageBox.Show($"Are you sure you want to delete this document?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Call your documentServices to delete the document from the database
                    bool isDeleted = regionComServices.DeleteData(parameterModel);

                    // If deletion was successful, remove it from the ObservableCollection
                    if (isDeleted)
                    {
                        LoadAllRegionCom();
                        MessageBox.Show("Document deleted successfully.");
                        popupFullDetailsFromRegion.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting document.");
                    }
                }

            }
            else if (!string.IsNullOrEmpty(ToRegionFullData.Direction) && string.IsNullOrEmpty(FromRegionFullData.Direction))
            {
                parameterModel = ToRegionFullData;
                // Confirm the deletion 
                var result = MessageBox.Show($"Are you sure you want to delete this document?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Call your documentServices to delete the document from the database
                    bool isDeleted = regionComServices.DeleteData(parameterModel);

                    // If deletion was successful, remove it from the ObservableCollection
                    if (isDeleted)
                    {
                        LoadAllRegionCom();
                        MessageBox.Show("Document deleted successfully.");
                        popupFullDetailsToRegion.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting document.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Problem Deleting Data");
            }

        }

        #endregion

        #region Show Full Details from Full Data Window
        private RelayCommand showFullDatailsFromFullData;

        public RelayCommand ShowFullDatailsFromFullData
        {
            get { return showFullDatailsFromFullData; }
        }

        private void OpenFullDetailsFromFullData(object parameter)
        {


            if (popupFullDetailsToRegion != null && popupFullDetailsToRegion.IsLoaded)
            {
                popupFullDetailsToRegion.Close();
            }
            if (popupFullDetailsFromRegion != null && popupFullDetailsFromRegion.IsLoaded)
            {
                popupFullDetailsFromRegion.Close();
            }
            if (parameter is RegionComModel documentToShow)
            {
                FromRegionFullData = new RegionComModel();//to still edit in full data window after it was open from full data window
                ToRegionFullData = new RegionComModel();//to still edit in full data window after it was open from full data window
                OpenFullDetailsForm(documentToShow);

            }
        }


        #endregion




    }
}
