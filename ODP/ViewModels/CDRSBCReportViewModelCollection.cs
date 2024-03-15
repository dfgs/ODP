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
	public class CDRSBCReportViewModelCollection : GenericViewModelList<CDRSBCReport, CDRSBCReportViewModel>
	{
		public CDRSBCReportViewModelCollection(IList<CDRSBCReport> Source) : base(Source)
		{
		}
		protected override CDRSBCReportViewModel OnCreateItem(CDRSBCReport Model)
		{
			return new CDRSBCReportViewModel(Model);
		}
	}
}
