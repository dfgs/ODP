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
	public class RTCPReportViewModelCollection : ListViewModel<RTCPReport, RTCPReportViewModel>
	{
		public RTCPReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		protected override RTCPReportViewModel OnCreateItem()
		{
			return new RTCPReportViewModel(Logger);
		}
	}
}
