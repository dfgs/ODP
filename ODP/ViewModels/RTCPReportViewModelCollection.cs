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
	public class RTCPReportViewModelCollection : GenericViewModelList<RTCPReport, RTCPReportViewModel>
	{
		public RTCPReportViewModelCollection(IList<RTCPReport> Source) : base(Source, (SourceItem) => new RTCPReportViewModel(SourceItem))
		{
		}

		
	}
}
