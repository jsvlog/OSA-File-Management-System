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

// 1. FIX: The namespace MUST now include the "CertificateView" folder name.
namespace OSA_File_Management_System.View
{
    /// <summary>
    /// Interaction logic for CertificateOfAppearance.xaml
    /// </summary>
    public partial class CertificateOfAppearance : UserControl
    {
        public CertificateOfAppearance()
        {
            // 2. This should now compile correctly!
            InitializeComponent();
        }
    }
}