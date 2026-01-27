using System.Text;
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
using OSA_File_Management_System.View;

namespace OSA_File_Management_System
{
    public partial class MainWindow : Window
    {
        DocumentViewModel ViewModel;
        MainViewModel MainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // 1. Set the default DataContext
            ViewModel = new DocumentViewModel();
            MainViewModel = new MainViewModel();
            this.DataContext = MainViewModel;

            // 2. Set the default View to RegionComm
            ContentArea.Content = new View.RegionComView.RegionComm();

            // 3. IMPORTANT: Tell the ListBox to visually highlight the first item (RegionCom)
            // This ensures the sidebar matches the content when you open the app.
            CategoryListBox.SelectedIndex = 0;
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if an item is actually selected
            if (CategoryListBox.SelectedItem is ListBoxItem selectedItem)
            {
                // Read the TAG from the XAML
                if (selectedItem.Tag != null)
                {
                    string navigationTag = selectedItem.Tag.ToString();

                    // Switch based on the Tag
                    switch (navigationTag)
                    {
                        case "RegionCom":
                            ContentArea.Content = new View.RegionComView.RegionComm();
                            MainViewModel.RegionComViewModel.LoadAllRegionCom();
                            break;

                        case "Inventory":
                            // I uncommented this line. 
                            // Make sure "View.Inventory" matches your actual file name!
                            // If your Inventory file is inside a folder named InventoryView, keep it as View.InventoryView.Inventory
                            // If it's directly in the View folder, change it to View.Inventory

                            ContentArea.Content = new View.Inventory(); // <--- Check this path
                            break;

                        case "Certificates":
                            // I uncommented this line too.
                            // Check if your file is named "CertificateOfAppearance"

                            ContentArea.Content = new View.CertificateOfAppearance(); // <--- Check this path
                            break;
                    }
                }
            }
        }
    }
}