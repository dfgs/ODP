using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class MediaReportViewModel : ReportViewModel<MediaReport>, IQualityProvider
	{
		public string? MediaReportType
		{
			get => Model?.MediaReportType;
		}
		public string? Cid
		{
			get => Model?.Cid;
		}
		public string? MediaType
		{
			get => Model?.MediaType ;
		}
		public string? Coder
		{
			get => Model?.Coder ;
		}
		public string? Intrv
		{
			get => Model?.Intrv ;
		}
		public string? LocalRtpIp
		{
			get => Model?.LocalRtpIp ;
		}
		public string? LocalRtpPort
		{
			get => Model?.LocalRtpPort ;
		}
		public string? RemoteRtpIp
		{
			get => Model?.RemoteRtpIp ;
		}
		public string? RemoteRtpPort
		{
			get => Model?.RemoteRtpPort ;
		}
		public int? InPackets
		{
			get => Model?.InPackets ;
		}
		public int? OutPackets
		{
			get => Model?.OutPackets ;
		}
		public int? LocalPackLoss
		{
			get => Model?.LocalPackLoss ;
		}
		public int? RemotePackLoss
		{
			get => Model?.RemotePackLoss ;
		}
		public int? RTPdelay
		{
			get => Model?.RTPdelay ;
		}
		public int? RTPjitter
		{
			get => Model?.RTPjitter ;
		}
		public string? TxRTPssrc
		{
			get => Model?.TxRTPssrc ;
		}
		public string? RxRTPssrc
		{
			get => Model?.RxRTPssrc ;
		}
		public string? LocalRFactor
		{
			get => Model?.LocalRFactor ;
		}
		public string? RemoteRFactor
		{
			get => Model?.RemoteRFactor ;
		}
		public string? LocalMosCQ
		{
			get => Model?.LocalMosCQ ;
		}
		public string? RemoteMosCQ
		{
			get => Model?.RemoteMosCQ ;
		}
		public string? TxRTPIPDiffServ
		{
			get => Model?.TxRTPIPDiffServ ;
		}
		public string? LatchedRtpIp
		{
			get => Model?.LatchedRtpIp ;
		}
		public string? LatchedRtpPort
		{
			get => Model?.LatchedRtpPort ;
		}
		public string? LatchedT38Ip
		{
			get => Model?.LatchedT38Ip ;
		}
		public string? LatchedT38Port
		{
			get => Model?.LatchedT38Port ;
		}
		public string? CoderTranscoding
		{
			get => Model?.CoderTranscoding ;
		}
		public string? LegId
		{
			get => Model?.LegId ;
		}

		public int? PacketLossPercent
		{
			get
			{
				if (Model == null) return null;
				if (LocalPackLoss==null) return null;
				if (InPackets==null) return null;
				if (InPackets == 0) return 0;
				return LocalPackLoss*100 / InPackets ;
			}
		}

		public Quality Quality
		{
			get
			{
				if ((LocalPackLoss > 1) || (RTPjitter >= 30) || (RTPdelay >= 250)) return ODP.ViewModels.Quality.Bad;
				if ((RTPjitter >= 20) || (RTPdelay >= 150)) return ODP.ViewModels.Quality.Average;
				return ODP.ViewModels.Quality.Good;
			}
		}

		public Brush Background
		{
			get
			{
				switch (Quality)
				{
					case ViewModels.Quality.Bad: return new SolidColorBrush(Colors.Red);
					case ViewModels.Quality.Average: return new SolidColorBrush(Colors.Orange);
					default: return new SolidColorBrush(Colors.Green);
				}
			}
		}


		public MediaReportViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
