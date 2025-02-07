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
	public class CallViewModelCollection : BaseViewModelEnumerable< CallViewModel>
	{
		public CallViewModelCollection(IList<Call> Source,GlobalFilterViewModel GlobalFilter) : base()
		{
			LoadInternal(
				Source.Select(item => new CallViewModel(item)).Where(call => GlobalFilter.Match(call))
			);
		}


	}
}
