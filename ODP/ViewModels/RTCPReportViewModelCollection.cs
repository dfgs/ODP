using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.ViewModels
{
	public class RTCPReportViewModelCollection : ODPViewModelCollection<RTCPReport, RTCPReportViewModel>
	{
		public RTCPReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}
	}
}
