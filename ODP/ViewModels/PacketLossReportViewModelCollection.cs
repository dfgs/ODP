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
	public class PacketLossReportViewModelCollection : GenericViewModelList<PacketLossReport, PacketLossReportViewModel>
	{
		public PacketLossReportViewModelCollection(IList<PacketLossReport> Source) : base(Source)
		{
		}

		public override int GetNewItemIndex(PacketLossReportViewModel Item)
		{
			if (Item.ReportTime == null) return Count;
			for(int t=Count-1;t>=0;t--)
			{
				if (Item.ReportTime!.Value>=(this[t].ReportTime ?? DateTime.MaxValue)) return t+1;
			}
			return 0;
		}//*/

		protected override PacketLossReportViewModel OnCreateItem(PacketLossReport Model)
		{
			return new PacketLossReportViewModel(Model);
		}
	}
}
