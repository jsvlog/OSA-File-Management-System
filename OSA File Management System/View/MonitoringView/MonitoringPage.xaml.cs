using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.ViewModel;

namespace OSA_File_Management_System.View.MonitoringView
{
    public partial class MonitoringPage : UserControl
    {
        public MonitoringPage()
        {
            InitializeComponent();
            this.DataContextChanged += MonitoringPage_DataContextChanged;
        }

        private void MonitoringPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MonitoringViewModel vm)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
                GenerateColumns(vm);
            }

            if (e.OldValue is MonitoringViewModel oldVm)
            {
                oldVm.PropertyChanged -= ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GridData" || e.PropertyName == "YearColumns")
            {
                if (DataContext is MonitoringViewModel vm)
                {
                    GenerateColumns(vm);
                    MonitoringGrid.Items.Refresh();
                }
            }
        }

        private void GenerateColumns(MonitoringViewModel vm)
        {
            MonitoringGrid.Columns.Clear();

            var municipalityCol = new DataGridTextColumn
            {
                Header = "Municipality",
                Binding = new Binding("Municipality"),
                Width = new DataGridLength(180),
                FontWeight = FontWeights.SemiBold
            };
            MonitoringGrid.Columns.Add(municipalityCol);

            if (vm.YearColumns == null) return;

            foreach (var year in vm.YearColumns)
            {
                var yearCol = new DataGridTemplateColumn
                {
                    Header = year.ToString(),
                    Width = new DataGridLength(50),
                    SortMemberPath = $"YearStatuses[{year}].Status"
                };

                var cellTemplate = CreateCellTemplate(year);
                yearCol.CellTemplate = cellTemplate;

                MonitoringGrid.Columns.Add(yearCol);
            }
        }

        private DataTemplate CreateCellTemplate(int year)
        {
            var factory = new FrameworkElementFactory(typeof(Border));
            factory.SetValue(Border.PaddingProperty, new Thickness(2));
            factory.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
            factory.SetValue(Border.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(Border.VerticalAlignmentProperty, VerticalAlignment.Center);

            factory.SetBinding(Border.BackgroundProperty, new Binding($"YearStatuses[{year}].Status")
            {
                Converter = new StatusToBrushConverter()
            });

            var iconFactory = new FrameworkElementFactory(typeof(TextBlock));
            iconFactory.SetValue(TextBlock.FontSizeProperty, 14.0);
            iconFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            iconFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            iconFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            iconFactory.SetBinding(TextBlock.TextProperty, new Binding($"YearStatuses[{year}].Status")
            {
                Converter = new StatusToIconConverter()
            });
            iconFactory.SetBinding(TextBlock.ForegroundProperty, new Binding($"YearStatuses[{year}].Status")
            {
                Converter = new StatusToForegroundConverter()
            });

            factory.AppendChild(iconFactory);

            return new DataTemplate { VisualTree = factory };
        }

        private void DocumentTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                var panel = clickedButton.Parent as Panel;
                if (panel == null) return;

                foreach (var child in panel.Children.OfType<Button>())
                {
                    if (child.Tag is string)
                    {
                        child.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6c757d"));
                    }
                }
                clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#257180"));

                if (DataContext is MonitoringViewModel vm)
                {
                    vm.SelectedDocumentType = clickedButton.Tag.ToString();
                }
            }
        }

        private YearStatus GetClickedYearStatus()
        {
            if (MonitoringGrid.CurrentItem is MonitoringGridRow row && MonitoringGrid.CurrentColumn != null)
            {
                var header = MonitoringGrid.CurrentColumn.Header?.ToString();
                if (int.TryParse(header, out int year))
                {
                    if (row.YearStatuses.ContainsKey(year))
                    {
                        return row.YearStatuses[year];
                    }
                }
            }
            return null;
        }

        private void ContextMenu_Edit(object sender, RoutedEventArgs e)
        {
            var yearStatus = GetClickedYearStatus();
            if (yearStatus != null && DataContext is MonitoringViewModel vm)
            {
                vm.OpenEditForm(yearStatus);
            }
        }

        private void ContextMenu_Delete(object sender, RoutedEventArgs e)
        {
            var yearStatus = GetClickedYearStatus();
            if (yearStatus != null && DataContext is MonitoringViewModel vm)
            {
                vm.DeleteMonitoring(yearStatus);
            }
        }

        private void ContextMenu_ViewDetails(object sender, RoutedEventArgs e)
        {
            var yearStatus = GetClickedYearStatus();
            if (yearStatus != null)
            {
            var detailsWindow = new ViewMonitoringDetails(yearStatus);
            detailsWindow.ShowDialog();
            }
        }
    }

    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "Submitted":
                        return new SolidColorBrush(Color.FromRgb(187, 247, 208));
                    default:
                        return new SolidColorBrush(Color.FromRgb(243, 244, 246));
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "Submitted":
                        return "✓";
                    default:
                        return "✗";
                }
            }
            return "✗";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "Submitted":
                        return new SolidColorBrush(Color.FromRgb(22, 163, 74));
                    default:
                        return new SolidColorBrush(Color.FromRgb(156, 163, 175));
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}