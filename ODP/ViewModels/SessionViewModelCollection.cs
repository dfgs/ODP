using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class SessionViewModelCollection : ViewModelCollection<SessionViewModel>
	{
		public SessionViewModelCollection(ILogger Logger) : base(Logger)
		{
		}
		public override int GetNewItemIndex(SessionViewModel Item)
		{
			if (Item.StartTime == null) return Count;
			for (int t=0;t<Count;t++)
			{
				if (Item.StartTime!.Value < (this[t].StartTime ?? DateTime.MinValue)) return t;
			}
			return Count;
		}
	}
}
