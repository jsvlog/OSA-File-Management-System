using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace OSA_File_Management_System
{
    public partial class MainWindow : Window
    {
        private const double DrawerWidth = 220;
        private bool _isDrawerOpen = true; // drawer visible at startup

        public MainWindow()
        {
            InitializeComponent();
            _isDrawerOpen = true;
        }

        private void ToggleDrawer_Click(object sender, RoutedEventArgs e)
        {
            if (_isDrawerOpen) CloseDrawer();
            else OpenDrawer();
        }

        private void OpenDrawer()
        {
            // ensure start off-screen then slide in
            DrawerTranslate.X = -DrawerWidth;
            var showAnim = new DoubleAnimation(-DrawerWidth, 0, TimeSpan.FromMilliseconds(260))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            DrawerTranslate.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, showAnim);
            _isDrawerOpen = true;
        }

        private void CloseDrawer()
        {
            var hideAnim = new DoubleAnimation(0, -DrawerWidth, TimeSpan.FromMilliseconds(220))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            DrawerTranslate.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, hideAnim);
            _isDrawerOpen = false;
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // close drawer when user selects an item
            if (e.AddedItems.Count > 0)
            {
                CloseDrawer();
            }

            // keep any existing selection handling/navigation here
        }

        // Optional: close drawer with Escape key
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _isDrawerOpen)
            {
                CloseDrawer();
                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }
    }
}