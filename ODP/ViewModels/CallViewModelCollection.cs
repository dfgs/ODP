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
	public class CallViewModelCollection : ListViewModel<Call, CallViewModel>
	{
		public CallViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		protected override CallViewModel OnCreateItem()
		{
			return new CallViewModel(Logger);
		}
	}
}
