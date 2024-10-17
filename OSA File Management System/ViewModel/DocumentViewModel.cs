using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using OSA_File_Management_System.View;
using OSA_File_Management_System.Model;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using OSA_File_Management_System.Commands;
using Mysqlx.Crud;

namespace OSA_File_Management_System.ViewModel
{
    class DocumentViewModel
    {
        private DocumentServices documentServices;

        public ObservableCollection<Document> DocumentList { get; set; }

        public DocumentViewModel()
        {
            documentServices = new DocumentServices();
            DocumentList = documentServices.GetAllDocuments();
            showAddForm = new RelayCommand(OpenAddDocumentForm);
        }


        private RelayCommand showAddForm;

        public RelayCommand ShowAddForm
        {
            get { return showAddForm; }
        }

        private void OpenAddDocumentForm()
        {
            // Create the popup window
            AddFormInventory popup = new AddFormInventory();

            // Bind ViewModel to the popup window
            popup.DataContext = new DocumentViewModel();

            // Show the popup window
            popup.ShowDialog();
        }






    }
}
