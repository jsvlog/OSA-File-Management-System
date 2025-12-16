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

namespace OSA_File_Management_System.View.RegionComView
{
    /// <summary>
    /// Interaction logic for FullDetailsToRegion.xaml
    /// </summary>
    public partial class FullDetailsToRegion : Window
    {
        public FullDetailsToRegion()
        {
            InitializeComponent();
            // 1. Make the window draggable (since we removed the title bar)
            this.MouseLeftButtonDown += (s, e) => { if (e.ButtonState == MouseButtonState.Pressed) this.DragMove(); };
        }
        // 2. The logic for the "X" button
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
