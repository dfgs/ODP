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
	public class PacketLossReportViewModelCollection : ODPViewModelCollection<PacketLossReport,PacketLossReportViewModel>
	{
		public PacketLossReportViewModelCollection(ILogger Logger) : base(Logger)
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


	}
}
