using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;

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










    }
}
