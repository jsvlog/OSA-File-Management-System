using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OSA_File_Management_System
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            btnLogin.IsEnabled = false;
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                bool isValid = await Task.Run(() => ValidateLogin(username, password));

                if (isValid)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                btnLogin.IsEnabled = true;
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            return username == "admin" && password == "password";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
