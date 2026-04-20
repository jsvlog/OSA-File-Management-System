using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using OSA_File_Management_System.ViewModel;
using OSA_File_Management_System.Model;

namespace OSA_File_Management_System.View.RegionComView
{
    public partial class RegionComm : UserControl
    {
        private CollectionViewSource? _cvs;
        private MainViewModel? _mainVm;

        public RegionComm()
        {
            InitializeComponent();
            Loaded += RegionComm_Loaded;
        }

        private void RegionComm_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainVm = DataContext as MainViewModel;
            if (_mainVm?.RegionComViewModel != null)
            {
                _mainVm.RegionComViewModel.PropertyChanged += RegionComViewModel_PropertyChanged;
            }

            DataContextChanged += (s, args) =>
            {
                _mainVm = DataContext as MainViewModel;
                if (_mainVm?.RegionComViewModel != null)
                {
                    _mainVm.RegionComViewModel.PropertyChanged += RegionComViewModel_PropertyChanged;
                }
            };
        }

        private void RegionComViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegionComViewModel.SelectedFilterYear) ||
                e.PropertyName == nameof(RegionComViewModel.IsToRegionChecked) ||
                e.PropertyName == nameof(RegionComViewModel.IsFromRegionChecked) ||
                e.PropertyName == nameof(RegionComViewModel.IsAllChecked) ||
                e.PropertyName == nameof(RegionComViewModel.SearchTextRegionCom))
            {
                RefreshFilter();
            }
        }

        private void RefreshFilter()
        {
            if (FindResource("RegionComViewSource") is CollectionViewSource cvs)
            {
                cvs.View?.Refresh();
            }
        }

        private void RegionComViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (_mainVm?.RegionComViewModel == null)
            {
                e.Accepted = true;
                return;
            }

            var vm = _mainVm.RegionComViewModel;
            var doc = e.Item as RegionComModel;
            if (doc == null)
            {
                e.Accepted = true;
                return;
            }

            bool matches = true;

            if (vm.SelectedFilterYear > 0)
            {
                matches = matches && doc.DateReceived.HasValue && doc.DateReceived.Value.Year == vm.SelectedFilterYear;
            }

            if (vm.IsToRegionChecked)
            {
                matches = matches && doc.Direction == "To Region";
            }
            else if (vm.IsFromRegionChecked)
            {
                matches = matches && doc.Direction == "From Region";
            }

            if (!string.IsNullOrEmpty(vm.SearchTextRegionCom))
            {
                var search = vm.SearchTextRegionCom;
                matches = matches && (
                    (doc.SubjectParticulars != null && doc.SubjectParticulars.Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                    (doc.RefNumber != null && doc.RefNumber.Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                    (doc.ReceivedFrom != null && doc.ReceivedFrom.Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                    (doc.TypeOfDocs != null && doc.TypeOfDocs.Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                    (doc.DateReceived.HasValue && doc.DateReceived.Value.ToString("MM-dd-yyyy").Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                    (doc.DocumentDate.HasValue && doc.DocumentDate.Value.ToString("MM-dd-yyyy").Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                    (doc.Remarks != null && doc.Remarks.Contains(search, System.StringComparison.OrdinalIgnoreCase))
                );
            }

            e.Accepted = matches;
        }
    }
}