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
	public class ProjectViewModel:ViewModel<Project>
	{

		public string? Name
		{
			get { return Model?.Name; }
			set { if(Model!=null) Model.Name = value; }
		}

		public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(ProjectViewModel), new PropertyMetadata(null));



		public static readonly DependencyProperty ReportsProperty = DependencyProperty.Register("Reports", typeof(ViewModelCollection<ReportViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<ReportViewModel> Reports
		{
			get { return (ViewModelCollection<ReportViewModel>)GetValue(ReportsProperty); }
			set { SetValue(ReportsProperty, value); }
		}




		public ProjectViewModel(ILogger Logger) : base(Logger)
		{
			Reports = new ViewModelCollection<ReportViewModel>(Logger);
		}

		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}

		public async Task AddFileAsync(string FileName)
		{
			ISyslogParser syslogParser;
			IReportParser reportParser;

			if (Model == null) return;

			syslogParser= new SyslogParser();
			reportParser = new ReportParser();

			await TryAsync(() => Model.AddFileAsync(FileName,syslogParser,reportParser)).OrThrow($"Failed to read syslog file {FileName}");
			//await Reports.LoadAsync(await Model.Reports.ToViewModelsAsync(() => new ReportViewModel(Logger)));
			
		}


	}
}
