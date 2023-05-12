﻿using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class PacketLossReportViewModelCollection : ViewModelCollection<PacketLossReportViewModel>
	{
		public PacketLossReportViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		public override int GetNewItemIndex(PacketLossReportViewModel Item)
		{
			if (Item.ReportTime == null) return Count;
			for(int t=0;t<Count;t++)
			{
				if (Item.ReportTime!.Value < (this[t].ReportTime ?? DateTime.MinValue)) return t;
			}
			return Count;
		}


	}
}