using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSA_File_Management_System.ViewModel
{
    class MainViewModel
    {
		private DocumentViewModel documentViewModel;

		public DocumentViewModel DocumentViewModel
		{
			get { return documentViewModel; }
			set { documentViewModel = value; }
		}

		private RegionComViewModel regionComViewModel;

		public RegionComViewModel RegionComViewModel
		{
			get { return regionComViewModel; }
			set { regionComViewModel = value; }
		}

		public MainViewModel()
		{
			regionComViewModel = new RegionComViewModel();
			documentViewModel = new DocumentViewModel();
		}




	}
}
