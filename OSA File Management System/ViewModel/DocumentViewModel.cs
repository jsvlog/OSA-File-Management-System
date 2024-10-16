using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using OSA_File_Management_System.View;
using OSA_File_Management_System.Model;

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
        }


    }
}
