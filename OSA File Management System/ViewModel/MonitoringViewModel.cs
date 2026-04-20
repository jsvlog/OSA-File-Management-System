using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;

namespace OSA_File_Management_System.ViewModel
{
    class MonitoringViewModel : INotifyPropertyChanged
    {
        #region Notify Property Change
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private MonitoringServices monitoringServices;

        public MonitoringViewModel()
        {
            monitoringServices = new MonitoringServices();
            monitoringServices.EnsureTableExists();

            DocumentTypes = new ObservableCollection<string>
            {
                "Financial Statement",
                "Barangay Financial Statement",
                "AOM",
                "Barangay AOM",
                "AAR",
                "Barangay AAR"
            };

            SelectedDocumentType = DocumentTypes.FirstOrDefault() ?? "";
            SelectedYear = DateTime.Now.Year;

            YearList = new ObservableCollection<int>();
            for (int y = DateTime.Now.Year; y >= 2018; y--)
            {
                YearList.Add(y);
            }

            MonitoringList = new ObservableCollection<MonitoringModel>();
            Municipalities = new ObservableCollection<string>();

            loadMonitoringData = new RelayCommand(LoadMonitoringData);
            showAddForm = new RelayCommand(OpenAddForm);
            addMonitoring = new RelayCommand(AddMonitoringCommand);
            closeAddForm = new RelayCommand(CloseAddFormCommand);
            updateMonitoring = new RelayCommand(UpdateMonitoringCommand);
            deleteMonitoring = new RelayCommand(DeleteMonitoringCommand);
            showEditForm = new RelayCommand(ShowEditFormCommand);
            closeEditForm = new RelayCommand(CloseEditFormCommand);

            LoadMonitoringData();
        }

        #region Properties
        private ObservableCollection<string> documentTypes;
        public ObservableCollection<string> DocumentTypes
        {
            get { return documentTypes; }
            set { documentTypes = value; OnPropertyChanged("DocumentTypes"); }
        }

        private string selectedDocumentType;
        public string SelectedDocumentType
        {
            get { return selectedDocumentType; }
            set { selectedDocumentType = value; OnPropertyChanged("SelectedDocumentType"); }
        }

        private ObservableCollection<int> yearList;
        public ObservableCollection<int> YearList
        {
            get { return yearList; }
            set { yearList = value; OnPropertyChanged("YearList"); }
        }

        private int selectedYear;
        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; OnPropertyChanged("SelectedYear"); }
        }

        private ObservableCollection<MonitoringModel> monitoringList;
        public ObservableCollection<MonitoringModel> MonitoringList
        {
            get { return monitoringList; }
            set { monitoringList = value; OnPropertyChanged("MonitoringList"); }
        }

        private ObservableCollection<string> municipalities;
        public ObservableCollection<string> Municipalities
        {
            get { return municipalities; }
            set { municipalities = value; OnPropertyChanged("Municipalities"); }
        }

        private MonitoringModel selectedMonitoring;
        public MonitoringModel SelectedMonitoring
        {
            get { return selectedMonitoring; }
            set { selectedMonitoring = value; OnPropertyChanged("SelectedMonitoring"); }
        }

        private MonitoringModel addFormData;
        public MonitoringModel AddFormData
        {
            get { return addFormData; }
            set { addFormData = value; OnPropertyChanged("AddFormData"); }
        }

        private MonitoringModel editFormData;
        public MonitoringModel EditFormData
        {
            get { return editFormData; }
            set { editFormData = value; OnPropertyChanged("EditFormData"); }
        }

        private string addFormStatus;
        public string AddFormStatus
        {
            get { return addFormStatus; }
            set { addFormStatus = value; OnPropertyChanged("AddFormStatus"); }
        }

        private string editFormStatus;
        public string EditFormStatus
        {
            get { return editFormStatus; }
            set { editFormStatus = value; OnPropertyChanged("EditFormStatus"); }
        }
        #endregion

        #region Load Data
        private RelayCommand loadMonitoringData;

        public RelayCommand LoadMonitoringDataCommand
        {
            get { return loadMonitoringData; }
        }

        public void LoadMonitoringData()
        {
            try
            {
                Municipalities = monitoringServices.GetMunicipalities();
                MonitoringList = monitoringServices.GetAllMonitoring(SelectedDocumentType, SelectedYear);
                FillMissingMunicipalities();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillMissingMunicipalities()
        {
            var existingMunicipalities = MonitoringList.Select(m => m.Municipality).ToList();
            var missingMunicipalities = Municipalities.Where(m => !existingMunicipalities.Contains(m)).ToList();

            foreach (var muni in missingMunicipalities)
            {
                MonitoringList.Add(new MonitoringModel
                {
                    DocumentType = SelectedDocumentType,
                    Municipality = muni,
                    Year = SelectedYear,
                    Status = "Not Submitted"
                });
            }
        }
        #endregion

        #region Show Add Form
        private RelayCommand showAddForm;
        public RelayCommand ShowAddForm
        {
            get { return showAddForm; }
        }

        private Window addFormWindow;
        private void OpenAddForm()
        {
            addFormData = new MonitoringModel
            {
                DocumentType = SelectedDocumentType,
                Year = SelectedYear,
                Municipality = Municipalities.FirstOrDefault() ?? "",
                Status = "Pending",
                DateSubmitted = DateTime.Now
            };
            OnPropertyChanged("AddFormData");
            AddFormStatus = "Pending";

            addFormWindow = new View.MonitoringView.AddMonitoringForm();
            addFormWindow.DataContext = this;
            addFormWindow.ShowDialog();
        }
        #endregion

        #region Add Monitoring
        private RelayCommand addMonitoring;
        public RelayCommand AddMonitoring
        {
            get { return addMonitoring; }
        }

        private void AddMonitoringCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(AddFormData?.Municipality))
                {
                    MessageBox.Show("Please select a municipality.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                AddFormData.DocumentType = SelectedDocumentType;
                AddFormData.Year = SelectedYear;
                AddFormData.Status = AddFormStatus ?? "Pending";

                bool isSaved = monitoringServices.AddMonitoring(AddFormData);
                if (isSaved)
                {
                    MessageBox.Show("Monitoring entry saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    addFormWindow.Close();
                    LoadMonitoringData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Close Add Form
        private RelayCommand closeAddForm;
        public RelayCommand CloseAddForm
        {
            get { return closeAddForm; }
        }

        private void CloseAddFormCommand()
        {
            addFormWindow?.Close();
        }
        #endregion

        #region Show Edit Form
        private RelayCommand showEditForm;
        public RelayCommand ShowEditForm
        {
            get { return showEditForm; }
        }

        private Window editFormWindow;
        private void ShowEditFormCommand(object parameter)
        {
            if (parameter is MonitoringModel monitoringToEdit)
            {
                EditFormData = new MonitoringModel
                {
                    Id = monitoringToEdit.Id,
                    DocumentType = monitoringToEdit.DocumentType,
                    Municipality = monitoringToEdit.Municipality,
                    Year = monitoringToEdit.Year,
                    DateSubmitted = monitoringToEdit.DateSubmitted,
                    Status = monitoringToEdit.Status,
                    Remarks = monitoringToEdit.Remarks
                };
                OnPropertyChanged("EditFormData");
                EditFormStatus = monitoringToEdit.Status ?? "Pending";

                editFormWindow = new View.MonitoringView.EditMonitoringForm();
                editFormWindow.DataContext = this;
                editFormWindow.ShowDialog();
            }
        }
        #endregion

        #region Update Monitoring
        private RelayCommand updateMonitoring;
        public RelayCommand UpdateMonitoring
        {
            get { return updateMonitoring; }
        }

        private void UpdateMonitoringCommand()
        {
            try
            {
                if (EditFormData == null) return;

                EditFormData.Status = EditFormStatus ?? "Pending";

                bool isSaved = monitoringServices.UpdateMonitoring(EditFormData);
                if (isSaved)
                {
                    MessageBox.Show("Monitoring entry updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    editFormWindow.Close();
                    LoadMonitoringData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Delete Monitoring
        private RelayCommand deleteMonitoring;
        public RelayCommand DeleteMonitoring
        {
            get { return deleteMonitoring; }
        }

        private void DeleteMonitoringCommand(object parameter)
        {
            if (parameter is MonitoringModel monitoringToDelete)
            {
                if (monitoringToDelete.Id == 0)
                {
                    MessageBox.Show("This entry has not been saved to the database yet.", "Cannot Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete the monitoring entry for {monitoringToDelete.Municipality}?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool isDeleted = monitoringServices.DeleteMonitoring(monitoringToDelete);
                    if (isDeleted)
                    {
                        MessageBox.Show("Monitoring entry deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMonitoringData();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting monitoring entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion

        #region Close Edit Form
        private RelayCommand closeEditForm;
        public RelayCommand CloseEditForm
        {
            get { return closeEditForm; }
        }

        private void CloseEditFormCommand()
        {
            editFormWindow?.Close();
        }
        #endregion
    }
}