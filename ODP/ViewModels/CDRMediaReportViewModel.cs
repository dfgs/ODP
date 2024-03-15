using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class CDRMediaReportViewModel : CDRReportGenericViewModel<CDRMediaReport>, IQualityProvider
	{
		private static SolidColorBrush NABrush = new SolidColorBrush(Colors.Gray);
		private static SolidColorBrush BadBrush = new SolidColorBrush(Colors.Red);
		private static SolidColorBrush AverageBrush = new SolidColorBrush(Colors.Orange);
		private static SolidColorBrush GoodBrush = new SolidColorBrush(Colors.Green);


		[Browsable(true)]
		public string? MediaReportType
		{
			get => Model?.MediaReportType;
		}
		[Browsable(true)]
		public string? Cid
		{
			get => Model?.Cid;
		}
		[Browsable(true)]
		public string? MediaType
		{
			get => Model?.MediaType ;
		}
		[Browsable(true)]
		public string? Coder
		{
			get => Model?.Coder ;
		}
		[Browsable(true)]
		public string? Intrv
		{
			get => Model?.Intrv ;
		}
		[Browsable(true)]
		public string? LocalRtpIp
		{
			get => Model?.LocalRtpIp ;
		}
		[Browsable(true)]
		public ushort? LocalRtpPort
		{
			get => Model?.LocalRtpPort ;
		}
		[Browsable(true)]
		public string? RemoteRtpIp
		{
			get => Model?.RemoteRtpIp ;
		}
		[Browsable(true)]
		public ushort? RemoteRtpPort
		{
			get => Model?.RemoteRtpPort ;
		}
		[Browsable(true)]
		public long? InPackets
		{
			get => Model?.InPackets ;
		}
		[Browsable(true)]
		public long? OutPackets
		{
			get => Model?.OutPackets ;
		}
		[Browsable(true)]
		public long? LocalPackLoss
		{
			get => Model?.LocalPackLoss ;
		}
		[Browsable(true)]
		public long? RemotePackLoss
		{
			get => Model?.RemotePackLoss ;
		}
		[Browsable(true)]
		public int? RTPdelay
		{
			get => Model?.RTPdelay ;
		}
		[Browsable(true)]
		public int? RTPjitter
		{
			get => Model?.RTPjitter ;
		}
		[Browsable(true)]
		public double? RTPjitterms
		{
			get => Model?.RTPjitter/8000;
		}
		[Browsable(true)]
		public string? RTPjitterDisplay
		{
			get => RTPjitterms?.ToString("N2") ?? "0";
		}

		[Browsable(true)]
		public uint? TxRTPssrc
		{
			get => Model?.TxRTPssrc ;
		}
		[Browsable(true)]
		public uint? RxRTPssrc
		{
			get => Model?.RxRTPssrc ;
		}
		[Browsable(true)]
		public string? LocalRFactor
		{
			get => Model?.LocalRFactor ;
		}
		[Browsable(true)]
		public string? RemoteRFactor
		{
			get => Model?.RemoteRFactor ;
		}
		[Browsable(true)]
		public string? LocalMosCQ
		{
			get => Model?.LocalMosCQ ;
		}
		[Browsable(true)]
		public string? RemoteMosCQ
		{
			get => Model?.RemoteMosCQ ;
		}
		[Browsable(true)]
		public string? TxRTPIPDiffServ
		{
			get => Model?.TxRTPIPDiffServ ;
		}
		[Browsable(true)]
		public string? LatchedRtpIp
		{
			get => Model?.LatchedRtpIp ;
		}
		[Browsable(true)]
		public string? LatchedRtpPort
		{
			get => Model?.LatchedRtpPort ;
		}
		[Browsable(true)]
		public string? LatchedT38Ip
		{
			get => Model?.LatchedT38Ip ;
		}
		[Browsable(true)]
		public string? LatchedT38Port
		{
			get => Model?.LatchedT38Port ;
		}
		[Browsable(true)]
		public string? CoderTranscoding
		{
			get => Model?.CoderTranscoding ;
		}
		[Browsable(true)]
		public string? LegId
		{
			get => Model?.LegId ;
		}

		public static readonly DependencyProperty TxRTCPReportsProperty = DependencyProperty.Register("TxRTCPReports", typeof(RTCPReportViewModelCollection), typeof(CDRMediaReportViewModel), new PropertyMetadata(null));
		public RTCPReportViewModelCollection TxRTCPReports
		{
			get { return (RTCPReportViewModelCollection)GetValue(TxRTCPReportsProperty); }
			set { SetValue(TxRTCPReportsProperty, value); }
		}
		public static readonly DependencyProperty RxRTCPReportsProperty = DependencyProperty.Register("RxRTCPReports", typeof(RTCPReportViewModelCollection), typeof(CDRMediaReportViewModel), new PropertyMetadata(null));
		public RTCPReportViewModelCollection RxRTCPReports
		{
			get { return (RTCPReportViewModelCollection)GetValue(RxRTCPReportsProperty); }
			set { SetValue(RxRTCPReportsProperty, value); }
		}

		[Browsable(true)]
		public double? PacketLossPercent
		{
			get
			{
				if (Model == null) return null;
				if (LocalPackLoss==null) return null;
				if (InPackets==null) return null;
				if (InPackets == 0) return 0;
				return LocalPackLoss*100.0d / (InPackets+LocalPackLoss) ;
			}
		}
		[Browsable(false)]
		public string? PacketLossPercentDisplay
		{
			get
			{
				return PacketLossPercent?.ToString("N2") ?? "0";
			}
		}
		[Browsable(true)]
		public ulong PacketReorderCount
		{
			get;
			set;
		}

		public bool HasValidDelay
		{
			get => RTPdelay > -1;
		}
		public bool HasValidJitter
		{
			get => RTPjitter > -1;
		}
		public bool HasValidPacketLoss
		{
			get => LocalPackLoss > -1;
		}

		public bool HasRTCPReport
		{
			get { return TxRTCPReports.Any() || RxRTCPReports.Any(); }
		}

		public Quality Quality
		{
			get
			{
				if ((LocalPackLoss > 1) || (RTPjitterms >= 30) || (RTPdelay >= 250)) return ODP.ViewModels.Quality.Bad;
				if ((RTPjitterms >= 20) || (RTPdelay >= 150)) return ODP.ViewModels.Quality.Average;
				return ODP.ViewModels.Quality.Good;
			}
		}


		public Brush Background
		{
			get
			{
				switch (Quality)
				{
					case ViewModels.Quality.NA: return NABrush;
					case ViewModels.Quality.Bad: return BadBrush;
					case ViewModels.Quality.Average: return AverageBrush;
					default: return GoodBrush;
				}
			}
		}


		public CDRMediaReportViewModel(CDRMediaReport Model) : base(Model)
		{
			TxRTCPReports = new RTCPReportViewModelCollection(Model.TxRTCPReports);
			RxRTCPReports = new RTCPReportViewModelCollection(Model.RxRTCPReports);

		}

		


	}
}
