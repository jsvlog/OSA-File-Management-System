using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;
using OSA_File_Management_System.View.RegionCom;

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
            showAddToRegion = new RelayCommand(OpenAddDocumentForm);
            addToRegion = new RelayCommand(AddToRegionCommand);
            addToRegionData = new RegionComModel();
            selectFile = new RelayCommand(OpenSelectFile);
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

            // Show the popup window
            popup.ShowDialog();
        }

        #endregion

        #region Add RegionCom Data
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
                AddToRegionData.ScannedCopy = openFileDialog.FileName.ToString();
            }
        }
        #endregion











    }
}
