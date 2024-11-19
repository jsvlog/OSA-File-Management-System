using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

using OSA_File_Management_System.Model;

namespace OSA_File_Management_System.View.RegionComView
{
    /// <summary>
    /// Interaction logic for ArrangePrintToRegion.xaml
    /// </summary>
    public partial class ArrangePrintToRegion : Window
    {
        public ArrangePrintToRegion()
        {
            InitializeComponent();
        }


        private void DataGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            if (e.LeftButton == MouseButtonState.Pressed && dataGrid.SelectedItem != null)
            {
                DragDrop.DoDragDrop(dataGrid, dataGrid.SelectedItem, DragDropEffects.Move);
            }
        }

        private void DataGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RegionComModel)))
            {
                var droppedData = e.Data.GetData(typeof(RegionComModel)) as RegionComModel;
                var dataGrid = sender as DataGrid;
                var items = dataGrid?.ItemsSource as ObservableCollection<RegionComModel>;

                if (items != null && droppedData != null)
                {
                    // Remove the dropped item from the collection
                    items.Remove(droppedData);

                    // Determine the target item and index
                    var hitTestResult = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));
                    if (hitTestResult?.VisualHit is FrameworkElement targetElement)
                    {
                        // Try to get the target item safely
                        var targetData = targetElement.DataContext as RegionComModel;
                        if (targetData != null)
                        {
                            var targetIndex = items.IndexOf(targetData);

                            // Insert the dropped item at the new position
                            items.Insert(targetIndex >= 0 ? targetIndex : items.Count, droppedData);
                        }
                        else
                        {
                            // If targetData is null, add the item to the end
                            items.Add(droppedData);
                        }
                    }
                    else
                    {
                        // If no target is found, append the item at the end
                        items.Add(droppedData);
                    }
                }
            }
        }








    }
}
