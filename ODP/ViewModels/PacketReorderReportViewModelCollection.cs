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
	public class PacketReorderReportViewModelCollection : GenericViewModelList<PacketReorderReport, PacketReorderReportViewModel>
	{
		public PacketReorderReportViewModelCollection(IList<PacketReorderReport> Source) : base(Source, (SourceItem) => new PacketReorderReportViewModel(SourceItem))
		{
		}

		
	}
}
