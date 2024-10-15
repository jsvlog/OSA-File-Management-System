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

namespace OSA_File_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentArea.Content = new View.Inventory();
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear previous content
            ContentArea.Content = null;

            // Get selected category
            if (CategoryListBox.SelectedItem is ListBoxItem selectedItem)
            {
                if (selectedItem.Content is StackPanel stackPanel)
                {
                    var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                    if (textBlock != null)
                    {
                        switch (textBlock.Text)
                        {
                            case "Inventory":
                                ContentArea.Content = new View.Inventory(); // Load your UserControl or content for Category 1
                                break;
                            case "Region Comm.":
                                ContentArea.Content = new View.RegionCom(); // Load your UserControl or content for Category 2
                                break;
                            case "Certificate of Appearance":
                                ContentArea.Content = new View.CertificateOfAppearance(); // Load your UserControl or content for Category 3
                                break;
                        }
                    }
                }
            }
        }




    }
}