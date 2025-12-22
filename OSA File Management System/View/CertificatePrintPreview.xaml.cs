using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OSA_File_Management_System.View
{
    public partial class CertificatePrintPreview : Window
    {
        public CertificatePrintPreview(object dataContext)
        {
            InitializeComponent();
            // Set the DataContext (The ViewModel passed from the previous screen)
            this.DataContext = dataContext;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintCertificate();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // In WPF, "Saving as PDF" is typically done via the Print Dialog by selecting "Microsoft Print to PDF"
            MessageBox.Show("To save as PDF, please select 'Microsoft Print to PDF' in the printer selection dialog.", "Save Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
            PrintCertificate();
        }

        private void PrintCertificate()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // We need to access the FlowDocument inside the viewer
                FlowDocumentScrollViewer viewer = this.Content as Grid != null ?
                    (this.Content as Grid).Children[1] as FlowDocumentScrollViewer : null;

                if (viewer != null && viewer.Document != null)
                {
                    // Create a copy of the document to avoid "Document is in use" errors or layout issues during print
                    // For simple cases, printing directly might work, but standard practice often involves paginators.
                    // Here we print the IDocumentPaginatorSource directly.

                    IDocumentPaginatorSource idpSource = viewer.Document as IDocumentPaginatorSource;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Certificate of Appearance");
                }
            }
        }
    }
}