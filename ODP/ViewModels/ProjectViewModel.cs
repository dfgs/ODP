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



		public static readonly DependencyProperty SessionsProperty = DependencyProperty.Register("Sessions", typeof(ViewModelCollection<SessionViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<SessionViewModel> Sessions
		{
			get { return (ViewModelCollection<SessionViewModel>)GetValue(SessionsProperty); }
			set { SetValue(SessionsProperty, value); }
		}




		public ProjectViewModel(ILogger Logger) : base(Logger)
		{
			Sessions = new ViewModelCollection<SessionViewModel>(Logger);
		}

		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}


		/*protected override async Task OnLoadedAsync()
		{
			await base.OnLoadedAsync();
			OnPropertyChanged(nameof(Name));
		}*/

		public async Task AddFileAsync(string FileName)
		{
			ISyslogParser syslogParser;
			IReportParser reportParser;

			if (Model == null) return;

			//this.Path = FileName;
			//this.Name = System.IO.Path.GetFileNameWithoutExtension(FileName);

			syslogParser= new SyslogParser();
			reportParser = new ReportParser(new DateTimeParser());

			await TryAsync(() => Model.AddFileAsync(FileName,syslogParser,reportParser)).OrThrow($"Failed to read syslog file {FileName}");
			await Sessions.LoadAsync(await Model.Sessions.ToViewModelsAsync(() => new SessionViewModel(Logger)));
			
		}

		public void FindNext(SearchCriteria Criteria,string Value)
		{
			int index;

			if (Sessions.SelectedItem == null) index = -1;
			else index = Sessions.IndexOf(Sessions.SelectedItem);
			
			for(int t=index+1;t<Sessions.Count;t++)
			{
				if (Sessions[t].Match(Criteria,Value))
				{
					Sessions.SelectedItem= Sessions[t];
					return;
				}
			}

		}
		public void FindPrevious(SearchCriteria Criteria, string Value)
		{
			int index;

			if (Sessions.SelectedItem == null) index = Sessions.Count;
			else index = Sessions.IndexOf(Sessions.SelectedItem);

			for (int t = index - 1; t >= 0; t--)
			{
				if (Sessions[t].Match(Criteria, Value))
				{
					Sessions.SelectedItem = Sessions[t];
					return;
				}
			}

		}

	}
}
