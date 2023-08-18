﻿using EthernetFrameReaderLib;
using LogLib;
using Microsoft.Windows.Themes;
using ODP.CoreLib;
using RTCPFrameReaderLib;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class ProjectViewModel:ViewModel<Project>
	{
		public event EventHandler? SessionsChanged;

		private List<string> loadedFiles;
		private List<string> loadedWiresharkFiles;

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
		
		public static readonly DependencyProperty PacketLossReportsProperty = DependencyProperty.Register("PacketLossReports", typeof(PacketLossReportViewModelCollection), typeof(ProjectViewModel), new PropertyMetadata(null));
		public PacketLossReportViewModelCollection PacketLossReports
		{
			get { return (PacketLossReportViewModelCollection)GetValue(PacketLossReportsProperty); }
			set { SetValue(PacketLossReportsProperty, value); }
		}


		public static readonly DependencyProperty SessionsProperty = DependencyProperty.Register("Sessions", typeof(SessionViewModelCollection), typeof(ProjectViewModel), new PropertyMetadata(null));
		public SessionViewModelCollection Sessions
		{
			get { return (SessionViewModelCollection)GetValue(SessionsProperty); }
			set { SetValue(SessionsProperty, value); }
		}

		public static readonly DependencyProperty FilteredSessionsProperty = DependencyProperty.Register("FilteredSessions", typeof(ViewModelCollection<SessionViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<SessionViewModel> FilteredSessions
		{
			get { return (ViewModelCollection<SessionViewModel>)GetValue(FilteredSessionsProperty); }
			set { SetValue(FilteredSessionsProperty, value); }
		}


		/*public static readonly DependencyProperty FiltersProperty = DependencyProperty.Register("Filters", typeof(ViewModelCollection<FilterViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<FilterViewModel> session
		{
			get { return (ViewModelCollection<FilterViewModel>)GetValue(FiltersProperty); }
			set { SetValue(FiltersProperty, value); }
		}*/



		public static readonly DependencyProperty GlobalFilterProperty = DependencyProperty.Register("GlobalFilter", typeof(GlobalFilterViewModel), typeof(ProjectViewModel), new PropertyMetadata(null));
		public GlobalFilterViewModel GlobalFilter
		{
			get { return (GlobalFilterViewModel)GetValue(GlobalFilterProperty); }
			set { SetValue(GlobalFilterProperty, value); }
		}




		public ProjectViewModel(ILogger Logger) : base(Logger)
		{

			loadedFiles = new List<string>();
			loadedWiresharkFiles = new List<string>();
			PacketLossReports = new PacketLossReportViewModelCollection(Logger);
			Sessions = new SessionViewModelCollection(Logger);
			FilteredSessions = new ViewModelCollection<SessionViewModel>(Logger);
			//session = new ViewModelCollection<FilterViewModel>(Logger);
			GlobalFilter = new GlobalFilterViewModel(Logger);
			OnSessionsChanged();
		}

		protected void OnSessionsChanged()
		{
			SessionsChanged?.Invoke(this,EventArgs.Empty);
		}


		protected override void OnLoaded()
		{
			if (Model == null)
			{
				Sessions.Clear();
				PacketLossReports.Clear();
				return;
			}
			PacketLossReports.Load(Model.PacketLossReports.ToViewModels(() => new PacketLossReportViewModel(Logger)));
			Sessions.Load( Model.Sessions.ToViewModels(()=>new SessionViewModel(Logger)));
			OnSessionsChanged();
			RefreshFilters();
			RefreshSessions();
		}

		public void RefreshFilters()
		{

			foreach(string? ipGroup in Sessions.SelectMany(session => session.Calls).Select(call => call.IPGroup).Distinct())
			{
				if (ipGroup == null) continue;
				if (GlobalFilter.IPGroupFilters.Select(filter=>filter.Name).Contains(ipGroup)) continue;
				GlobalFilter.IPGroupFilters.Add(new IPGroupFilterViewModel(Logger) { Name=ipGroup });
			}

			foreach (string? sipInterface in Sessions.SelectMany(session => session.Calls).Select(call => call.SIPInterfaceId).Distinct())
			{
				if (sipInterface == null) continue;
				if (GlobalFilter.SIPInterfaceFilters.Select(filter => filter.Name).Contains(sipInterface)) continue;
				GlobalFilter.SIPInterfaceFilters.Add(new SIPInterfaceFilterViewModel(Logger) { Name = sipInterface });
			}
			
			foreach (string? termReason in Sessions.SelectMany(session => session.Calls).Select(call => call.TrmReason).Distinct())
			{
				if (termReason == null) continue;
				if (GlobalFilter.TermReasonFilters.Select(filter => filter.Name).Contains(termReason)) continue;
				GlobalFilter.TermReasonFilters.Add(new TermReasonFilterViewModel(Logger) { Name = termReason });
			}
		}


		public void RefreshSessions()
		{
			FilteredSessions.Load( Sessions.Where(session => GlobalFilter.Match(session)) );
		}

		

		public async Task AddFilesAsync(IEnumerable<string> FileNames,IProgress<long> Progress)
		{
			ICDRSyslogParser CDRSyslogParser;
			ICDRReportParser CDRReportParser;
			IPacketLossSyslogParser PacketLossSyslogParser;
			IPacketLossReportParser PacketLossReportParser;
			IPacketReorderSyslogParser PacketReorderSyslogParser;
			IPacketReorderReportParser PacketReorderReportParser;
			int index,count;

			if (Model == null) throw new InvalidOperationException("Model is not loaded");



			CDRSyslogParser = new CDRSyslogParser();
			CDRReportParser = new CDRReportParser(new DateTimeParser());
			PacketLossSyslogParser = new PacketLossSyslogParser();
			PacketLossReportParser = new PacketLossReportParser(new DateTimeParser());
			PacketReorderSyslogParser = new PacketReorderSyslogParser();
			PacketReorderReportParser = new PacketReorderReportParser(new DateTimeParser());

			index = 1;count = FileNames.Count();
			await foreach(string fileName in FileNames.AsAsyncEnumerable())
			{
				RunningTask = $"Loading file ({index}/{count})...";
				if (loadedFiles.Contains(fileName)) continue;
				loadedFiles.Add(fileName);
				await TryAsync(() => Model.AddFileAsync(fileName,
					CDRSyslogParser,CDRReportParser,
					PacketLossSyslogParser,PacketLossReportParser,
					PacketReorderSyslogParser,PacketReorderReportParser,
					Progress)).OrThrow($"Failed to read syslog file {fileName}");
				index++;
			}
			
			PacketLossReports.Load(Model.PacketLossReports.ToViewModels(() => new PacketLossReportViewModel(Logger)));
			Sessions.Load(Model.Sessions.ToViewModels(() => new SessionViewModel(Logger)));

			OnSessionsChanged();
			RefreshFilters();
			RefreshSessions();
			RunningTask = null;
			
		}

		public async Task AddWiresharkFilesAsync(IEnumerable<string> FileNames, IProgress<long> Progress)
		{
			int index, count;
			IFrameReader frameReader;
			IPacketReader packetReader;
			IUDPSegmentReader udpSegmentReader;
			IACDRReader acdrReader;
			IRTCPReader rtcpReader;

			if (Model == null) throw new InvalidOperationException("Model is not loaded");

			frameReader = new FrameReader();
			packetReader = new PacketReader();
			udpSegmentReader=new UDPSegmentReader();
			acdrReader = new ACDRReader();
			rtcpReader=new RTCPReader();

			index = 1; count = FileNames.Count();
			await foreach (string fileName in FileNames.AsAsyncEnumerable())
			{
				RunningTask = $"Loading file ({index}/{count})...";
				if (loadedWiresharkFiles.Contains(fileName)) continue;
				loadedWiresharkFiles.Add(fileName);



				await TryAsync(() => Model.AddWiresharkFileAsync(fileName,
					frameReader,packetReader,udpSegmentReader,acdrReader,rtcpReader,
					Progress)).OrThrow($"Failed to read wireshark file {fileName}");
				index++;
			}

			PacketLossReports.Load(Model.PacketLossReports.ToViewModels(() => new PacketLossReportViewModel(Logger)));
			Sessions.Load(Model.Sessions.ToViewModels(() => new SessionViewModel(Logger)));

			OnSessionsChanged();
			RefreshFilters();
			RefreshSessions();
			RunningTask = null;

		}

		public bool FindNext(MatchProperty Criteria,string Value)
		{
			int index;

			if (FilteredSessions.SelectedItem == null) index = -1;
			else index = FilteredSessions.IndexOf(FilteredSessions.SelectedItem);
			
			for(int t=index+1;t<FilteredSessions.Count;t++)
			{
				if (FilteredSessions[t].Match(Criteria,Value))
				{
					FilteredSessions.SelectedItem= FilteredSessions[t];
					return true;
				}
			}

			return false;
		}
		public bool FindPrevious(MatchProperty Criteria, string Value)
		{
			int index;

			if (FilteredSessions.SelectedItem == null) index = FilteredSessions.Count;
			else index = FilteredSessions.IndexOf(FilteredSessions.SelectedItem);

			for (int t = index - 1; t >= 0; t--)
			{
				if (FilteredSessions[t].Match(Criteria, Value))
				{
					FilteredSessions.SelectedItem = FilteredSessions[t];
					return true;
				}
			}
			return false;
		}

		public async Task SaveAsync(string Path)
		{
			if (Path == null) throw new ArgumentNullException(nameof(Path));
			if (Model==null) throw new InvalidOperationException("Model is not loaded");

			RunningTask = $"Saving project...";

			this.Path= Path;
			this.Name=System.IO.Path.GetFileName(Path);
			await TryAsync(() => Model.SaveAsync(Path)).OrThrow("Failed to save project file");
			RunningTask = null;

		}

		public async Task LoadAsync(string Path)
		{
			Project project;

			RunningTask = $"Loading project...";
			project = await TryAsync(() => Project.LoadAsync(Path)).OrThrow("Failed to open project");
			Load(project);
			RunningTask = null;
		}


	}
}
