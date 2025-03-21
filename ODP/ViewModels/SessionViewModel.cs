﻿using LogLib;
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
	public class SessionViewModel : GenericViewModel<Session>, IQualityProvider
	{
		public static readonly DependencyProperty CallsProperty = DependencyProperty.Register("Calls", typeof(CallViewModelCollection), typeof(SessionViewModel), new PropertyMetadata(null));
		public CallViewModelCollection Calls
		{
			get { return (CallViewModelCollection)GetValue(CallsProperty); }
			set { SetValue(CallsProperty, value); }
		}
		public static readonly DependencyProperty PacketReorderReportsProperty = DependencyProperty.Register("PacketReorderReports", typeof(PacketReorderReportViewModelCollection), typeof(SessionViewModel), new PropertyMetadata(null));
		public PacketReorderReportViewModelCollection PacketReorderReports
		{
			get { return (PacketReorderReportViewModelCollection)GetValue(PacketReorderReportsProperty); }
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



		public SessionViewModel(Session Model, GlobalFilterViewModel GlobalFilter) : base(Model)
		{
			/*if (Model.SessionId== "c64c90:24:3499")
			{
				CallViewModel vm;
				bool b1, b2, b3;
				vm = new CallViewModel(Model.Calls[0]);

				b1 = GlobalFilter.Match(vm.SBCReports[0]);
				b2 = GlobalFilter.Match(vm.SBCReports[1]);

				int t = 0;
			}*/
			Calls = new CallViewModelCollection(Model.Calls,GlobalFilter);
			Calls.SelectedItem = Calls.FirstOrDefault();
			PacketReorderReports = new PacketReorderReportViewModelCollection(Model.PacketReorderReports);
			AssociatePacketReorderReports();
		}
		private void AssociatePacketReorderReport(PacketReorderReport Report)
		{
			if (Report == null) throw new ArgumentNullException(nameof(Report));

			foreach(CallViewModel call in Calls)
			{
				foreach(CDRMediaReportViewModel mediaReport in call.MediaReports)
				{
					if ((mediaReport.RemoteRtpIp==Report.SourceIP) && (mediaReport.RemoteRtpPort == Report.SourcePort))
					{
						mediaReport.PacketReorderCount++;
						return;
					}
				}
			}
		}
		private void AssociatePacketReorderReports()
		{
			if (Model==null) throw new ArgumentNullException(nameof(Model));

			foreach(PacketReorderReport report in Model.PacketReorderReports)
			{
				AssociatePacketReorderReport(report);
			}
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
