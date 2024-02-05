using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class CDRSBCReportViewModelCollection : ODPViewModelCollection<CDRSBCReportViewModel, CDRSBCReportViewModel>
	{
		public CDRSBCReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}
	}
}
