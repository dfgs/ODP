using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class ODPViewModelCollection<ModelT, ViewModelT> : ViewModelCollection<ModelT, ViewModelT>
	{
		public ODPViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		public void Load(IEnumerable<ViewModelT> Items)
		{
			LoadInternal(Items);
		}


	}
}
