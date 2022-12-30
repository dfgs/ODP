using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class MediaReportViewModel : ReportViewModel<MediaReport>
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
		public string? InPackets
		{
			get => Model?.InPackets ;
		}
		public string? OutPackets
		{
			get => Model?.OutPackets ;
		}
		public string? LocalPackLoss
		{
			get => Model?.LocalPackLoss ;
		}
		public string? RemotePackLoss
		{
			get => Model?.RemotePackLoss ;
		}
		public string? RTPdelay
		{
			get => Model?.RTPdelay ;
		}
		public string? RTPjitter
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

		public MediaReportViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
