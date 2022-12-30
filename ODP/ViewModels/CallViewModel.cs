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
	public class CallViewModel : ViewModel<Call>
	{
		public string? SIPCallID
		{
			get => Model?.SIPCallId;
		}

		public string? SrcURI
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SrcURI != null)?.SrcURI;
		}
		public string? DstURI
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.DstURI != null)?.DstURI;
		}

		public DateTime? SetupTime
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SetupTime != null)?.SetupTime;
		}
		public DateTime? ConnectTime
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.ConnectTime != null)?.ConnectTime;
		}
		public DateTime? ReleaseTime
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.ReleaseTime != null)?.ReleaseTime;
		}
		public string? TrmReason
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.TrmReason != null)?.TrmReason;
		}

		public string? SourceIp
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SourceIp!= null)?.SourceIp;
		}
		public string? DestIp
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.DestIp != null)?.DestIp;
		}

		public string? IPGroup
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.IPGroup != null)?.IPGroup;
		}
		public string? SIPInterfaceId
		{
			get => Model?.SBCReports.FirstOrDefault(item => item.SIPInterfaceId != null)?.SIPInterfaceId;
		}

		public Quality? Quality
		{
			get => MediaReports.Select(item => item.Quality).Min();
		}


		public static readonly DependencyProperty SBCReportsProperty = DependencyProperty.Register("SBCReports", typeof(ViewModelCollection<SBCReportViewModel>), typeof(CallViewModel), new PropertyMetadata(null));
		public ViewModelCollection<SBCReportViewModel> SBCReports
		{
			get { return (ViewModelCollection<SBCReportViewModel>)GetValue(SBCReportsProperty); }
			set { SetValue(SBCReportsProperty, value); }
		}
		public static readonly DependencyProperty MediaReportsProperty = DependencyProperty.Register("MediaReports", typeof(ViewModelCollection<MediaReportViewModel>), typeof(CallViewModel), new PropertyMetadata(null));
		public ViewModelCollection<MediaReportViewModel> MediaReports
		{
			get { return (ViewModelCollection<MediaReportViewModel>)GetValue(MediaReportsProperty); }
			set { SetValue(MediaReportsProperty, value); }
		}

		public CallViewModel(ILogger Logger) : base(Logger)
		{
			SBCReports = new ViewModelCollection<SBCReportViewModel>(Logger);
			MediaReports = new ViewModelCollection<MediaReportViewModel>(Logger);
			
		}


		protected override async Task OnLoadedAsync()
		{
			if (Model==null)
			{
				SBCReports.Clear();
				MediaReports.Clear();
				return;
			}

			await SBCReports.LoadAsync(await Model.SBCReports.ToViewModelsAsync(() => new SBCReportViewModel(Logger)));
			await MediaReports.LoadAsync(await Model.MediaReports.ToViewModelsAsync(() => new MediaReportViewModel(Logger)));

		}


	}
}
