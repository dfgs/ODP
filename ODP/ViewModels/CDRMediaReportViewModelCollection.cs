using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class CDRMediaReportViewModelCollection : GenericViewModelList<CDRMediaReport, CDRMediaReportViewModel>
    {
		public CDRMediaReportViewModelCollection(IList<CDRMediaReport> Source) : base(Source)
		{
		}

		protected override CDRMediaReportViewModel OnCreateItem(CDRMediaReport Model)
		{
			return new CDRMediaReportViewModel(Model);
		}
	}
}
