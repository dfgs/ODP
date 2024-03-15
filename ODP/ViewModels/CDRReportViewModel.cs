using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public abstract class CDRReportGenericViewModel<ModelT> : GenericViewModel<ModelT>
		where ModelT:CDRReport
	{


		public string? SessionId
		{
			get => Model?.SessionId; 
		}

		public string? SIPCallId
		{
			get => Model?.SIPCallId; 
		}


		public CDRReportGenericViewModel(ModelT Model) : base(Model)
		{
		}
	}
}
