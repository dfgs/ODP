using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ODP.CoreLib
{
	public class ReportParser : IReportParser
	{
		public Report? Parse(string Line)
		{
			string[] parts;

			if (Line == null) throw new ArgumentNullException(nameof(Line));

			// Ignore headers
			if (Line.StartsWith("SBCReportType")) return null;
			if (Line.StartsWith("MediaReportType")) return null;

			if (Line.StartsWith("CALL_"))
			{
				SBCReport sbcReport;
				
				parts = Line.Split('|');
				if (parts.Length != 39) throw new InvalidDataException("Invalid SBC report format, please check SBC configuration");

				sbcReport = new SBCReport();
				sbcReport.SBCReportType = parts[0].Trim();
				sbcReport.EPTyp = parts[1].Trim();
				sbcReport.SIPCallId = parts[2].Trim();
				sbcReport.SessionId = parts[3].Trim();
				sbcReport.Orig = parts[4].Trim();
				sbcReport.SourceIp = parts[5].Trim();
				sbcReport.SourcePort = parts[6].Trim();
				sbcReport.DestIp = parts[7].Trim();
				sbcReport.DestPort = parts[8].Trim();
				sbcReport.TransportType = parts[9].Trim();
				sbcReport.SrcURI = parts[10];
				sbcReport.SrcURIBeforeMap = parts[11].Trim();
				sbcReport.DstURI = parts[12].Trim();
				sbcReport.DstURIBeforeMap = parts[13].Trim();
				sbcReport.Duration = parts[14].Trim();
				sbcReport.TrmSd = parts[15].Trim();
				sbcReport.TrmReason = parts[16].Trim();
				sbcReport.TrmReasonCategory = parts[17].Trim();
				sbcReport.SetupTime = DateTime.Parse( parts[18].Trim());
				sbcReport.ConnectTime = parts[19].Trim();
				sbcReport.ReleaseTime = parts[20].Trim();
				sbcReport.RedirectReason = parts[21].Trim();
				sbcReport.RedirectURINum = parts[22].Trim();
				sbcReport.RedirectURINumBeforeMap = parts[23].Trim();
				sbcReport.TxSigIPDiffServ = parts[24].Trim();
				sbcReport.IPGroup = parts[25].Trim();
				sbcReport.SrdId = parts[26].Trim();
				sbcReport.SIPInterfaceId = parts[27].Trim();
				sbcReport.ProxySetId = parts[28].Trim();
				sbcReport.IpProfileId = parts[29].Trim();
				sbcReport.MediaRealmId = parts[30].Trim();
				sbcReport.DirectMedia = parts[31].Trim();
				sbcReport.SIPTrmReason = parts[32].Trim();
				sbcReport.SIPTermDesc = parts[33].Trim();
				sbcReport.Caller = parts[34].Trim();
				sbcReport.Callee = parts[35].Trim();
				sbcReport.Trigger = parts[36].Trim();
				sbcReport.LegId = parts[37].Trim();
				sbcReport.VoiceAIConnectorName = parts[38].Trim();

				return sbcReport;

			}
			if (Line.StartsWith("MEDIA_"))
			{
				MediaReport mediaReport;

				parts = Line.Split('|');
				if (parts.Length != 30) throw new InvalidDataException("Invalid media report format, please check SBC configuration");

				mediaReport = new MediaReport();
				mediaReport.MediaReportType = parts[0].Trim();
				mediaReport.SIPCallId = parts[1].Trim();
				mediaReport.SessionId = parts[2].Trim();
				mediaReport.Cid = parts[3].Trim();
				mediaReport.MediaType = parts[4].Trim();
				mediaReport.Coder = parts[5].Trim();
				mediaReport.Intrv = parts[6].Trim();
				mediaReport.LocalRtpIp = parts[7].Trim();
				mediaReport.LocalRtpPort = parts[8].Trim();
				mediaReport.RemoteRtpIp = parts[9].Trim();
				mediaReport.RemoteRtpPort = parts[10].Trim();
				mediaReport.InPackets = parts[11].Trim();
				mediaReport.OutPackets = parts[12].Trim();
				mediaReport.LocalPackLoss = parts[13].Trim();
				mediaReport.RemotePackLoss = parts[14].Trim();
				mediaReport.RTPdelay = parts[15].Trim();
				mediaReport.RTPjitter = parts[16].Trim();
				mediaReport.TxRTPssrc = parts[17].Trim();
				mediaReport.RxRTPssrc = parts[18].Trim();
				mediaReport.LocalRFactor = parts[19].Trim();
				mediaReport.RemoteRFactor = parts[20].Trim();
				mediaReport.LocalMosCQ = parts[21].Trim();
				mediaReport.RemoteMosCQ = parts[22].Trim();
				mediaReport.TxRTPIPDiffServ = parts[23].Trim();
				mediaReport.LatchedRtpIp = parts[24].Trim();
				mediaReport.LatchedRtpPort = parts[25].Trim();
				mediaReport.LatchedT38Ip = parts[26].Trim();
				mediaReport.LatchedT38Port = parts[27].Trim();
				mediaReport.CoderTranscoding = parts[28].Trim();
				mediaReport.LegId = parts[29].Trim();

				return mediaReport;

			}
			throw new InvalidDataException("Invalid report format, please check SBC configuration");

			

			
		}
	}
}
