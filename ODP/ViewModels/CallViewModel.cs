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
	public class CallViewModel : GenericViewModel<Call>, IQualityProvider
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
			get => string.Join(',',Model?.SBCReports.Select(item=>item.TrmReason).Where(item => !string.IsNullOrEmpty(item) && (item!= "REASON N/A")).Distinct() ?? [""]);
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
			get => Model?.SBCReports.FirstOrDefault(item => !string.IsNullOrEmpty(item.IPGroup ))?.IPGroup??"N/A";
		}

		[Browsable(true)]
		public string? SIPInterfaceId
		{
			get => Model?.SBCReports.FirstOrDefault(item => !string.IsNullOrEmpty(item.SIPInterfaceId))?.SIPInterfaceId ?? "N/A";
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
		
		


		public CallViewModel(Call Model) : base(Model)
		{
			SBCReports = new CDRSBCReportViewModelCollection(Model.SBCReports);
			MediaReports = new CDRMediaReportViewModelCollection(Model.MediaReports);
			MediaReports.SelectedItem = MediaReports.FirstOrDefault();
		}


	


	}
}
