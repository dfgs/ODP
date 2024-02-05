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
	public class CDRSBCReportViewModelCollection : ListViewModel<CDRSBCReport, CDRSBCReportViewModel>
	{
		public CDRSBCReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}
		protected override CDRSBCReportViewModel OnCreateItem()
		{
			return new CDRSBCReportViewModel(Logger);
		}
	}
}
