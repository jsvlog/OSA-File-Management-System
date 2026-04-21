using System;
using System.Collections.Generic;
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
        #region INotify
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

            DocumentTypes = new List<string>
            {
                "Financial Statement",
                "Barangay Financial Statement",
                "AOM",
                "Barangay AOM",
                "AAR",
                "Barangay AAR"
            };

            GridData = new ObservableCollection<MonitoringGridRow>();
            BarangayGridData = new ObservableCollection<BarangayMonitoringGridRow>();
            MunicipalityList = new ObservableCollection<string>();
            GenerateYearColumns();

            loadMonitoringData = new RelayCommand(LoadMonitoringData);
            showAddForm = new RelayCommand(OpenAddForm);
            addMonitoring = new RelayCommand(AddMonitoringCommand);
            closeAddForm = new RelayCommand(CloseAddFormCommand);
            updateMonitoring = new RelayCommand(UpdateMonitoringCommand);
            closeEditForm = new RelayCommand(CloseEditFormCommand);

            selectedDocumentType = DocumentTypes[0];
            LoadMonitoringData();
        }

        #region Properties
        private List<string> documentTypes;
        public List<string> DocumentTypes
        {
            get { return documentTypes; }
            set { documentTypes = value; OnPropertyChanged("DocumentTypes"); }
        }

        private string selectedDocumentType;
        public string SelectedDocumentType
        {
            get { return selectedDocumentType; }
            set
            {
                selectedDocumentType = value;
                OnPropertyChanged("SelectedDocumentType");
                OnPropertyChanged("IsBarangayDocumentType");
                if (YearColumns != null && YearColumns.Count > 0)
                {
                    LoadMonitoringData();
                }
            }
        }

        private List<int> yearColumns;
        public List<int> YearColumns
        {
            get { return yearColumns; }
            set { yearColumns = value; OnPropertyChanged("YearColumns"); }
        }

        private ObservableCollection<MonitoringGridRow> gridData;
        public ObservableCollection<MonitoringGridRow> GridData
        {
            get { return gridData; }
            set { gridData = value; OnPropertyChanged("GridData"); }
        }

        private ObservableCollection<BarangayMonitoringGridRow> barangayGridData;
        public ObservableCollection<BarangayMonitoringGridRow> BarangayGridData
        {
            get { return barangayGridData; }
            set { barangayGridData = value; OnPropertyChanged("BarangayGridData"); }
        }

        private ObservableCollection<string> municipalityList;
        public ObservableCollection<string> MunicipalityList
        {
            get { return municipalityList; }
            set { municipalityList = value; OnPropertyChanged("MunicipalityList"); }
        }

        private ObservableCollection<string> barangayList;
        public ObservableCollection<string> BarangayList
        {
            get { return barangayList; }
            set { barangayList = value; OnPropertyChanged("BarangayList"); }
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

        private int addFormYear;
        public int AddFormYear
        {
            get { return addFormYear; }
            set { addFormYear = value; OnPropertyChanged("AddFormYear"); }
        }

        private List<int> addFormYearList;
        public List<int> AddFormYearList
        {
            get { return addFormYearList; }
            set { addFormYearList = value; OnPropertyChanged("AddFormYearList"); }
        }

        public bool IsBarangayDocumentType => BarangayData.IsBarangayDocumentType(SelectedDocumentType);
        #endregion

        #region Year Columns
        private void GenerateYearColumns()
        {
            var years = new List<int>();
            for (int y = 2016; y <= DateTime.Now.Year; y++)
            {
                years.Add(y);
            }
            YearColumns = years;
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
                MunicipalityList = monitoringServices.GetMunicipalities();
                var submissions = monitoringServices.GetAllMonitoring(SelectedDocumentType, 0);

                if (IsBarangayDocumentType)
                {
                    LoadBarangayGrid(submissions);
                }
                else
                {
                    LoadMunicipalityGrid(submissions);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadMunicipalityGrid(ObservableCollection<MonitoringModel> submissions)
        {
            var rows = new ObservableCollection<MonitoringGridRow>();
            foreach (var muni in MunicipalityList)
            {
                var row = new MonitoringGridRow { Municipality = muni };
                foreach (var year in YearColumns)
                {
                    var sub = submissions.FirstOrDefault(s => s.Municipality == muni && s.Year == year);
                    row.YearStatuses[year] = new YearStatus
                    {
                        SubmissionId = sub?.Id ?? 0,
                        Municipality = muni,
                        Year = year,
                        DocumentType = SelectedDocumentType,
                        Status = sub?.Status ?? "Not Submitted",
                        DateSubmitted = sub?.DateSubmitted,
                        Remarks = sub?.Remarks,
                        PdfLink = sub?.PdfLink
                    };
                }
                rows.Add(row);
            }
            GridData = rows;
        }

        private void LoadBarangayGrid(ObservableCollection<MonitoringModel> submissions)
        {
            var rows = new ObservableCollection<BarangayMonitoringGridRow>();
            foreach (var muni in MunicipalityList)
            {
                var barangays = BarangayData.GetBarangaysForMunicipality(muni);
                foreach (var barangay in barangays)
                {
                    var row = new BarangayMonitoringGridRow { Municipality = muni, Barangay = barangay };
                    foreach (var year in YearColumns)
                    {
                        var sub = submissions.FirstOrDefault(s => s.Municipality == muni && s.Barangay == barangay && s.Year == year);
                        row.YearStatuses[year] = new YearStatus
                        {
                            SubmissionId = sub?.Id ?? 0,
                            Municipality = muni,
                            Barangay = barangay,
                            Year = year,
                            DocumentType = SelectedDocumentType,
                            Status = sub?.Status ?? "Not Submitted",
                            DateSubmitted = sub?.DateSubmitted,
                            Remarks = sub?.Remarks,
                            PdfLink = sub?.PdfLink
                        };
                    }
                    rows.Add(row);
                }
            }
            BarangayGridData = rows;
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
            var yearList = new List<int>();
            for (int y = 2016; y <= DateTime.Now.Year + 1; y++)
            {
                yearList.Add(y);
            }
            AddFormYearList = yearList;
            AddFormYear = DateTime.Now.Year;

            if (IsBarangayDocumentType)
            {
                var firstMuni = MunicipalityList.FirstOrDefault() ?? "";
                var barangays = BarangayData.GetBarangaysForMunicipality(firstMuni);
                BarangayList = new ObservableCollection<string>(barangays);

                addFormData = new MonitoringModel
                {
                    DocumentType = SelectedDocumentType,
                    Year = DateTime.Now.Year,
                    Municipality = firstMuni,
                    Barangay = barangays.FirstOrDefault() ?? "",
                    Status = "Submitted",
                    DateSubmitted = DateTime.Now,
                    Remarks = ""
                };
            }
            else
            {
                addFormData = new MonitoringModel
                {
                    DocumentType = SelectedDocumentType,
                    Year = DateTime.Now.Year,
                    Municipality = MunicipalityList.FirstOrDefault() ?? "",
                    Status = "Submitted",
                    DateSubmitted = DateTime.Now,
                    Remarks = ""
                };
            }
            OnPropertyChanged("AddFormData");
            OnPropertyChanged("IsBarangayDocumentType");
            AddFormStatus = "Submitted";
            OnPropertyChanged("AddFormStatus");

            addFormWindow = new View.MonitoringView.AddMonitoringForm();
            addFormWindow.DataContext = this;
            addFormWindow.ShowDialog();
        }

        public void OnMunicipalityChangedForBarangay()
        {
            if (IsBarangayDocumentType && addFormData != null)
            {
                var barangays = BarangayData.GetBarangaysForMunicipality(addFormData.Municipality ?? "");
                BarangayList = new ObservableCollection<string>(barangays);
                addFormData.Barangay = barangays.FirstOrDefault() ?? "";
                OnPropertyChanged("AddFormData");
            }
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

                if (IsBarangayDocumentType && string.IsNullOrEmpty(AddFormData?.Barangay))
                {
                    MessageBox.Show("Please select a barangay.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                AddFormData.DocumentType = SelectedDocumentType;
                AddFormData.Year = AddFormYear;
                AddFormData.Status = AddFormStatus ?? "Not Submitted";
                if (AddFormData.Status == "Not Submitted")
                {
                    AddFormData.DateSubmitted = null;
                }

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

        #region Edit Monitoring (from context menu)
        private RelayCommand updateMonitoring;
        public RelayCommand UpdateMonitoring
        {
            get { return updateMonitoring; }
        }

        private Window editFormWindow;

        public void OpenEditForm(YearStatus yearStatus)
        {
            if (yearStatus.SubmissionId == 0)
            {
                var locationInfo = IsBarangayDocumentType
                    ? $"{yearStatus.Barangay} ({yearStatus.Municipality}) - {yearStatus.Year}"
                    : $"{yearStatus.Municipality} - {yearStatus.Year}";
                var result = MessageBox.Show(
                    $"No submission exists for {locationInfo}.\nWould you like to create one?",
                    "Create Entry", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OpenAddFormWith(yearStatus);
                }
                return;
            }

            editFormData = new MonitoringModel
            {
                Id = yearStatus.SubmissionId,
                DocumentType = yearStatus.DocumentType,
                Municipality = yearStatus.Municipality,
                Barangay = yearStatus.Barangay,
                Year = yearStatus.Year,
                DateSubmitted = yearStatus.DateSubmitted,
                Status = yearStatus.Status,
                Remarks = yearStatus.Remarks,
                PdfLink = yearStatus.PdfLink
            };
            OnPropertyChanged("EditFormData");
            OnPropertyChanged("IsBarangayDocumentType");
            EditFormStatus = yearStatus.Status ?? "Not Submitted";
            OnPropertyChanged("EditFormStatus");

            editFormWindow = new View.MonitoringView.EditMonitoringForm();
            editFormWindow.DataContext = this;
            editFormWindow.ShowDialog();
        }

        private void OpenAddFormWith(YearStatus yearStatus)
        {
            var yearList = new List<int>();
            for (int y = 2016; y <= DateTime.Now.Year + 1; y++)
            {
                yearList.Add(y);
            }
            AddFormYearList = yearList;
            AddFormYear = yearStatus.Year;

            if (IsBarangayDocumentType)
            {
                var barangays = BarangayData.GetBarangaysForMunicipality(yearStatus.Municipality ?? "");
                BarangayList = new ObservableCollection<string>(barangays);

                addFormData = new MonitoringModel
                {
                    DocumentType = yearStatus.DocumentType,
                    Year = yearStatus.Year,
                    Municipality = yearStatus.Municipality,
                    Barangay = yearStatus.Barangay ?? barangays.FirstOrDefault() ?? "",
                    Status = "Submitted",
                    DateSubmitted = DateTime.Now,
                    Remarks = ""
                };
            }
            else
            {
                addFormData = new MonitoringModel
                {
                    DocumentType = yearStatus.DocumentType,
                    Year = yearStatus.Year,
                    Municipality = yearStatus.Municipality,
                    Status = "Submitted",
                    DateSubmitted = DateTime.Now,
                    Remarks = ""
                };
            }
            OnPropertyChanged("AddFormData");
            AddFormStatus = "Submitted";
            OnPropertyChanged("AddFormStatus");

            addFormWindow = new View.MonitoringView.AddMonitoringForm();
            addFormWindow.DataContext = this;
            addFormWindow.ShowDialog();
        }

        private void UpdateMonitoringCommand()
        {
            try
            {
                if (EditFormData == null) return;

                EditFormData.Status = EditFormStatus ?? "Not Submitted";
                if (EditFormData.Status == "Not Submitted")
                {
                    EditFormData.DateSubmitted = null;
                }

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

        #region Delete Monitoring (from context menu)
        public void DeleteMonitoring(YearStatus yearStatus)
        {
            if (yearStatus.SubmissionId == 0)
            {
                MessageBox.Show("No submission exists for this entry.", "Cannot Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var locationInfo = IsBarangayDocumentType
                ? $"{yearStatus.Barangay} ({yearStatus.Municipality}) - {yearStatus.Year}"
                : $"{yearStatus.Municipality} - {yearStatus.Year}";

            var result = MessageBox.Show($"Are you sure you want to delete the monitoring entry for {locationInfo}?",
                "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var monitoring = new MonitoringModel { Id = yearStatus.SubmissionId };
                bool isDeleted = monitoringServices.DeleteMonitoring(monitoring);
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