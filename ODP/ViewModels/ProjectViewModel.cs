﻿using LogLib;
using Microsoft.Windows.Themes;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class ProjectViewModel:ViewModel<Project>
	{
		public event EventHandler? SessionsChanged;

		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(ProjectViewModel), new PropertyMetadata("New project"));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}


		public static readonly DependencyProperty RunningTaskProperty = DependencyProperty.Register("RunningTask", typeof(string), typeof(ProjectViewModel), new PropertyMetadata(null));
		public string? RunningTask
		{
			get { return (string?)GetValue(RunningTaskProperty); }
			set { SetValue(RunningTaskProperty, value); }
		}

		public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(ProjectViewModel), new PropertyMetadata(null));
		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}


		public static readonly DependencyProperty SessionsProperty = DependencyProperty.Register("Sessions", typeof(ViewModelCollection<SessionViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<SessionViewModel> Sessions
		{
			get { return (ViewModelCollection<SessionViewModel>)GetValue(SessionsProperty); }
			set { SetValue(SessionsProperty, value); }
		}


		public static readonly DependencyProperty FiltersProperty = DependencyProperty.Register("Filters", typeof(ViewModelCollection<FilterViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<FilterViewModel> Filters
		{
			get { return (ViewModelCollection<FilterViewModel>)GetValue(FiltersProperty); }
			set { SetValue(FiltersProperty, value); }
		}


		public ProjectViewModel(ILogger Logger) : base(Logger)
		{
			Sessions = new ViewModelCollection<SessionViewModel>(Logger);
			Filters = new ViewModelCollection<FilterViewModel>(Logger);
			OnSessionsChanged();
		}

		protected void OnSessionsChanged()
		{
			SessionsChanged?.Invoke(this,EventArgs.Empty);
		}


		protected override async Task OnLoadedAsync()
		{
			if (Model == null)
			{
				Sessions.Clear();
				return;
			}
			await Sessions.LoadAsync(await Model.Sessions.ToViewModelsAsync(()=>new SessionViewModel(Logger)));
			OnSessionsChanged();
		}

		public async Task AddFilesAsync(IEnumerable<string> FileNames,IProgress<long> Progress)
		{
			ISyslogParser syslogParser;
			IReportParser reportParser;
			int index,count;

			if (Model == null) throw new InvalidOperationException("Model is not loaded");

			

			syslogParser = new SyslogParser();
			reportParser = new ReportParser(new DateTimeParser());

			index = 1;count = FileNames.Count();
			await foreach(string fileName in FileNames.AsAsyncEnumerable())
			{
				RunningTask = $"Loading file ({index}/{count})...";
				await TryAsync(() => Model.AddFileAsync(fileName,syslogParser,reportParser,Progress)).OrThrow($"Failed to read syslog file {fileName}");
				index++;
			}
			RunningTask = "Creating items...";
			await Sessions.LoadAsync(await Model.Sessions.ToViewModelsAsync(() => new SessionViewModel(Logger)));
			OnSessionsChanged();
			RunningTask = null;
		}

		public bool FindNext(MatchProperty Criteria,string Value)
		{
			int index;

			if (Sessions.SelectedItem == null) index = -1;
			else index = Sessions.IndexOf(Sessions.SelectedItem);
			
			for(int t=index+1;t<Sessions.Count;t++)
			{
				if (Sessions[t].Match(Criteria,Value))
				{
					Sessions.SelectedItem= Sessions[t];
					return true;
				}
			}

			return false;
		}
		public bool FindPrevious(MatchProperty Criteria, string Value)
		{
			int index;

			if (Sessions.SelectedItem == null) index = Sessions.Count;
			else index = Sessions.IndexOf(Sessions.SelectedItem);

			for (int t = index - 1; t >= 0; t--)
			{
				if (Sessions[t].Match(Criteria, Value))
				{
					Sessions.SelectedItem = Sessions[t];
					return true;
				}
			}
			return false;
		}

		public async Task SaveAsync(string Path)
		{
			if (Path == null) throw new ArgumentNullException(nameof(Path));
			if (Model==null) throw new InvalidOperationException("Model is not loaded");
			this.Path= Path;
			this.Name=System.IO.Path.GetFileName(Path);
			await TryAsync(() => Model.SaveAsync(Path)).OrThrow("Failed to save project file");
			
		}

	}
}
