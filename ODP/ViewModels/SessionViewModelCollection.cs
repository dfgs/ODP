﻿using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class SessionViewModelCollection : BaseViewModelEnumerable<SessionViewModel>
	{
		public SessionViewModelCollection(IList<Session> Source,GlobalFilterViewModel GlobalFilter) 
		{
			LoadInternal(
				Source.Select(item => new SessionViewModel(item, GlobalFilter)).Where(session => GlobalFilter.Match(session))
			);
		}
		public override int GetNewItemIndex(SessionViewModel Item)
		{
			if (Item.StartTime == null) return Count;
			for (int t=Count-1;t>=0;t--)
			{
				if (Item.StartTime!.Value >= (this[t].StartTime ?? DateTime.MaxValue)) return t+1;
			}
			return 0;//*/
			/*for (int t=0;t<Count;t++)
			{
				if (Item.StartTime!.Value < (this[t].StartTime ?? DateTime.MinValue)) return t;
			}
			return Count;//*/
		}//*/

		

		

	}
}
