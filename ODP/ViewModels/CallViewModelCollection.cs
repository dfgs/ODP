using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.ViewModels
{
	public class CallViewModelCollection : ODPViewModelCollection<Call, CallViewModel>
	{
		public CallViewModelCollection(ILogger Logger) : base(Logger)
		{
		}
	}
}
