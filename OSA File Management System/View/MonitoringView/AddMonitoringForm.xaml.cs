using System.Windows;
using System.Windows.Controls;
using OSA_File_Management_System.ViewModel;

namespace OSA_File_Management_System.View.MonitoringView
{
    public partial class AddMonitoringForm : Window
    {
        public AddMonitoringForm()
        {
            InitializeComponent();
        }

        private void MunicipalityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MonitoringViewModel vm && MunicipalityComboBox.SelectedItem != null)
            {
                vm.OnMunicipalityChangedForBarangay();
            }
        }
    }
}