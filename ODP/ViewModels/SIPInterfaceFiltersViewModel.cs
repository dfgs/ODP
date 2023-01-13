using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class SIPInterfaceFiltersViewModel : ViewModelCollection<SIPInterfaceFilterViewModel>
	{
		public SIPInterfaceFiltersViewModel(ILogger Logger) : base(Logger)
		{
		}

		public override int GetNewItemIndex(SIPInterfaceFilterViewModel Item)
		{
			Comparer<string> comparer = Comparer<string>.Default;

			for(int t=0;t<Count;t++)
			{
				if (comparer.Compare(Item.Name , this[t].Name)<0) return t;
			}
			return Count;
		}


	}
}
