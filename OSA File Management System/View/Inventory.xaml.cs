using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OSA_File_Management_System.ViewModel;

namespace OSA_File_Management_System.View
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : UserControl
    {

        public Inventory()
        {
            InitializeComponent();

        }

        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            YearPopup.IsOpen = !YearPopup.IsOpen;  // Toggle the Popup visibility
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            // Get the ContextMenu and its PlacementTarget (the DataGridRow)
            var contextMenu = sender as ContextMenu;
            if (contextMenu != null)
            {
                // Set the DataContext of the ContextMenu to the DataContext of the DataGridRow
                var dataGridRow = contextMenu.PlacementTarget as DataGridRow;
                if (dataGridRow != null)
                {
                    contextMenu.DataContext = dataGridRow.DataContext;
                }
            }
        }
    }
}
