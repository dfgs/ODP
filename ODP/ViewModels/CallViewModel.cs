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
	public class CallViewModel : ViewModel<Call>, IQualityProvider
	{
		[Browsable(true)]
		public string? SIPCallID
		{
			get => Model?.SIPCallId;
		}

		[Browsable(true)]
		public string? SrcURI
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SrcURI != null)?.SrcURI;
		}

		[Browsable(true)]
		public string? DstURI
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.DstURI != null)?.DstURI;
		}

		[Browsable(true)]
		public DateTime? SetupTime
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SetupTime != null)?.SetupTime;
		}

		[Browsable(true)]
		public DateTime? ConnectTime
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.ConnectTime != null)?.ConnectTime;
		}

		[Browsable(true)]
		public DateTime? ReleaseTime
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.ReleaseTime != null)?.ReleaseTime;
		}
		[Browsable(true)]
		public TimeSpan? Duration
		{
			get
			{
				if (SetupTime == null) return null;
				if (ReleaseTime == null) return null;
				return ReleaseTime - SetupTime;
			}
		}
		[Browsable(true)]
		public string? TrmReason
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.TrmReason != null)?.TrmReason;
		}

		[Browsable(true)]
		public string? SourceIp
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SourceIp!= null)?.SourceIp;
		}

		[Browsable(true)]
		public string? DestIp
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.DestIp != null)?.DestIp;
		}

		[Browsable(true)]
		public string? IPGroup
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.IPGroup != null)?.IPGroup;
		}

		[Browsable(true)]
		public string? SIPInterfaceId
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SIPInterfaceId != null)?.SIPInterfaceId;
		}

		[Browsable(true)]
		public Quality Quality
		{
			get
			{
				if (HasMediaReport) return MediaReports.Select(item => item.Quality).Min();
				if (ShouldHaveAudio) return Quality.Bad;
				else return Quality.NA;
			}
		}

		
		public bool ShouldHaveAudio
		{
			get { return ConnectTime.HasValue; }
		}
		public bool HasMediaReport
		{
			get { return MediaReports.Any(); }
		}
		public bool HasRTCPReport
		{
			get	{ return MediaReports.Where(item=>item.HasRTCPReport).Any();	}
		}

		public static readonly DependencyProperty SBCReportsProperty = DependencyProperty.Register("SBCReports", typeof(CDRSBCReportViewModelCollection), typeof(CallViewModel), new PropertyMetadata(null));
		public CDRSBCReportViewModelCollection SBCReports
		{
			get { return (CDRSBCReportViewModelCollection)GetValue(SBCReportsProperty); }
			set { SetValue(SBCReportsProperty, value); }
		}
		public static readonly DependencyProperty MediaReportsProperty = DependencyProperty.Register("MediaReports", typeof(CDRMediaReportViewModelCollection), typeof(CallViewModel), new PropertyMetadata(null));
		public CDRMediaReportViewModelCollection MediaReports
		{
			get { return (CDRMediaReportViewModelCollection)GetValue(MediaReportsProperty); }
			set { SetValue(MediaReportsProperty, value); }
		}
		
		


		public CallViewModel(ILogger Logger) : base(Logger)
		{
			SBCReports = new CDRSBCReportViewModelCollection(Logger);
			MediaReports = new CDRMediaReportViewModelCollection(Logger);
		}


		protected override void OnLoaded()
		{
			/*if (Model==null)
			{
				MediaReports.SelectedItem = null;				
				SBCReports.Clear();
				MediaReports.Clear();
				return;
			}*/

			SBCReports.Load(Model.SBCReports.ToViewModels(() => new CDRSBCReportViewModel(Logger)));
			MediaReports.Load(Model.MediaReports.ToViewModels(() => new CDRMediaReportViewModel(Logger)));
			MediaReports.SelectedItem = MediaReports.FirstOrDefault();
		}


	}
}
