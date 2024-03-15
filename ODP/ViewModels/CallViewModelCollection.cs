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
	public class CallViewModelCollection : GenericViewModelList<Call, CallViewModel>
	{
		public CallViewModelCollection(IList<Call> Source) : base(Source)
		{
		}

		protected override CallViewModel OnCreateItem(Call Model)
		{
			return new CallViewModel(Model);
		}
	}
}
