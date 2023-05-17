using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ODP.CoreLib
{
	public class CDRMediaReport:CDRReport
	{
		public string? MediaReportType { get; set; }
		public string? Cid { get; set; }
		public string? MediaType { get; set; }
		public string? Coder { get; set; }
		public string? Intrv { get; set; }
		public string? LocalRtpIp { get; set; }
		public ushort? LocalRtpPort { get; set; }
		public string? RemoteRtpIp { get; set; }
		public ushort? RemoteRtpPort { get; set; }
		public long? InPackets { get; set; }
		public long? OutPackets { get; set; }
		public long? LocalPackLoss { get; set; }
		public long? RemotePackLoss { get; set; }
		public int? RTPdelay { get; set; }
		public int? RTPjitter { get; set; }
		public string? TxRTPssrc { get; set; }
		public string? RxRTPssrc { get; set; }
		public string? LocalRFactor { get; set; }
		public string? RemoteRFactor { get; set; }
		public string? LocalMosCQ { get; set; }
		public string? RemoteMosCQ { get; set; }
		public string? TxRTPIPDiffServ { get; set; }
		public string? LatchedRtpIp { get; set; }
		public string? LatchedRtpPort { get; set; }
		public string? LatchedT38Ip { get; set; }
		public string? LatchedT38Port { get; set; }
		public string? CoderTranscoding { get; set; }
		public string? LegId { get; set; }

	}
}
