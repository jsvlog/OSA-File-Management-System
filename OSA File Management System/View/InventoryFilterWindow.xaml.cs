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
using System.Windows.Shapes;

namespace OSA_File_Management_System.View
{
    /// <summary>
    /// Interaction logic for InventoryFilterWindow.xaml
    /// </summary>
    public partial class InventoryFilterWindow : Window
    {
        public InventoryFilterWindow()
        {
            InitializeComponent();
            // Allow the user to drag the borderless window (modern design requirement)
            this.MouseLeftButtonDown += (s, e) => { if (e.ButtonState == MouseButtonState.Pressed) this.DragMove(); };
        }

        // Logic for the custom "X" button (used in the modern header)
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
