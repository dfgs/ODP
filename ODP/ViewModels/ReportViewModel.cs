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
	public abstract class ReportViewModel<ModelT> : ViewModel<ModelT>
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


		public ReportViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
