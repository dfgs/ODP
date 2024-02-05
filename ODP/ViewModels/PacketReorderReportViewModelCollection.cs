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
	public class PacketReorderReportViewModelCollection : ListViewModel<PacketReorderReport, PacketReorderReportViewModel>
	{
		public PacketReorderReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		protected override PacketReorderReportViewModel OnCreateItem()
		{
			return new PacketReorderReportViewModel(Logger);
		}
	}
}
