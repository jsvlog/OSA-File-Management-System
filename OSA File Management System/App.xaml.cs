using System.Configuration;
using System.Data;
using System.Windows;

namespace OSA_File_Management_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show the LoginWindow first
            Login loginWindow = new Login();
            loginWindow.Show();
        }

    }

}
