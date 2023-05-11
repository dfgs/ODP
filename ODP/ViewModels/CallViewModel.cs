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

		public static readonly DependencyProperty SBCReportsProperty = DependencyProperty.Register("SBCReports", typeof(ViewModelCollection<CDRSBCReportViewModel>), typeof(CallViewModel), new PropertyMetadata(null));
		public ViewModelCollection<CDRSBCReportViewModel> SBCReports
		{
			get { return (ViewModelCollection<CDRSBCReportViewModel>)GetValue(SBCReportsProperty); }
			set { SetValue(SBCReportsProperty, value); }
		}
		public static readonly DependencyProperty MediaReportsProperty = DependencyProperty.Register("MediaReports", typeof(ViewModelCollection<CDRMediaReportViewModel>), typeof(CallViewModel), new PropertyMetadata(null));
		public ViewModelCollection<CDRMediaReportViewModel> MediaReports
		{
			get { return (ViewModelCollection<CDRMediaReportViewModel>)GetValue(MediaReportsProperty); }
			set { SetValue(MediaReportsProperty, value); }
		}

		public CallViewModel(ILogger Logger) : base(Logger)
		{
			SBCReports = new ViewModelCollection<CDRSBCReportViewModel>(Logger);
			MediaReports = new ViewModelCollection<CDRMediaReportViewModel>(Logger);
			
		}


		protected override void OnLoaded()
		{
			if (Model==null)
			{
				MediaReports.SelectedItem = null;				
				SBCReports.Clear();
				MediaReports.Clear();
				return;
			}

			SBCReports.Load(Model.SBCReports.ToViewModels(() => new CDRSBCReportViewModel(Logger)));
			MediaReports.Load(Model.MediaReports.ToViewModels(() => new CDRMediaReportViewModel(Logger)));
			MediaReports.SelectedItem = MediaReports.FirstOrDefault();

		}


	}
}
