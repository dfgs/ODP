using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class CallViewModelCollection : ViewModelCollection<CallViewModel>
	{
		public CallViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		public override int GetNewItemIndex(CallViewModel Item)
		{
			if (Item.SetupTime == null) return Count;
			for(int t=0;t<Count;t++)
			{
				if (Item.SetupTime!.Value < (this[t].SetupTime??DateTime.MinValue)) return t;
			}
			return Count;
		}

	}
}
