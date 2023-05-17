using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class SessionViewModel : ViewModel<Session>, IQualityProvider
	{
		public static readonly DependencyProperty CallsProperty = DependencyProperty.Register("Calls", typeof(ViewModelCollection<CallViewModel>), typeof(SessionViewModel), new PropertyMetadata(null));
		public ViewModelCollection<CallViewModel> Calls
		{
			get { return (ViewModelCollection<CallViewModel>)GetValue(CallsProperty); }
			set { SetValue(CallsProperty, value); }
		}
		public static readonly DependencyProperty PacketReorderReportsProperty = DependencyProperty.Register("PacketReorderReports", typeof(ViewModelCollection<PacketReorderReportViewModel>), typeof(SessionViewModel), new PropertyMetadata(null));
		public ViewModelCollection<PacketReorderReportViewModel> PacketReorderReports
		{
			get { return (ViewModelCollection<PacketReorderReportViewModel>)GetValue(PacketReorderReportsProperty); }
			set { SetValue(PacketReorderReportsProperty, value); }
		}

		[Browsable(true)]
		public string? SessionID
		{
			get => Model?.SessionId;
		}

		[Browsable(true)]
		public DateTime? StartTime
		{
			get => Calls.SelectMany(item => item.SBCReports.Select(item => item.SetupTime)).Min();
		}

		[Browsable(true)]
		public DateTime? StopTime
		{
			get => Calls.SelectMany(item => item.SBCReports.Select(item => item.ReleaseTime)).Max();
		}

		[Browsable(true)]
		public TimeSpan? Duration
		{
			get
			{
				if (StartTime == null) return null;
				if (StopTime==null) return null;
				return StopTime - StartTime;
			}
		}

		[Browsable(true)]
		public string? SrcURI
		{
			get => Calls.FirstOrDefault(item => item.SrcURI != null)?.SrcURI;
		}
		[Browsable(true)]
		public string? DstURI
		{
			get => Calls.FirstOrDefault(item => item.DstURI != null)?.DstURI;
		}


		[Browsable(true)]
		public Quality Quality
		{
			get => Calls.Select(item => item.Quality).MinOrDefault(Quality.NA);
		}



		public SessionViewModel(ILogger Logger) : base(Logger)
		{
			Calls = new ViewModelCollection<CallViewModel>(Logger);
			PacketReorderReports = new ViewModelCollection<PacketReorderReportViewModel>(Logger);
		}

		protected override void OnLoaded()
		{
			if (Model==null)
			{
				Calls.SelectedItem= null;
				Calls.Clear();
				PacketReorderReports.SelectedItem = null;
				PacketReorderReports.Clear();
				return;
			}

			Calls.Load(Model.Calls.ToViewModels(() => new CallViewModel(Logger)));
			Calls.SelectedItem = Calls.FirstOrDefault();

			PacketReorderReports.Load(Model.PacketReorderReports.ToViewModels(()=>new PacketReorderReportViewModel(Logger)));
		}
		public bool Match(MatchProperty Property, string Value)
		{
			switch(Property)
			{
				case MatchProperty.SessionID: return SessionID?.Contains(Value) ?? false;
				case MatchProperty.CallID: return Calls.FirstOrDefault(call=>call.SIPCallID?.Contains(Value)??false)!=null;
				case MatchProperty.SrcURI:return SrcURI?.Contains(Value) ?? false;
				case MatchProperty.DstURI:return DstURI?.Contains(Value) ?? false;
				case MatchProperty.Quality: return Quality.ToString().Contains(Value);
				case MatchProperty.IPGroup: return Calls.FirstOrDefault(call => call.IPGroup?.Contains(Value) ?? false) != null;
				case MatchProperty.SIPInterface: return Calls.FirstOrDefault(call => call.SIPInterfaceId?.Contains(Value) ?? false) != null;
				default: return false;
			}
		}
		public bool Match(IEnumerable<FilterViewModel> Filters)
		{
			return Filters.All(filter=>filter.Match(this));
		}


	}
}
