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
	public class ReportViewModel : ViewModel<Report>
	{
		public ReportViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
