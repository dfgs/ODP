using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.ViewModels
{
	public class PacketReorderReportViewModelCollection : ODPViewModelCollection<PacketReorderReport, PacketReorderReportViewModel>
	{
		public PacketReorderReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}
	}
}
