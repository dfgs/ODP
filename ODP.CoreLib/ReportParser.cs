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
				sbcReport.SBCReportType = parts[0];
				sbcReport.EPTyp = parts[1];
				sbcReport.SIPCallId = parts[2];
				sbcReport.SessionId = parts[3];
				sbcReport.Orig = parts[4];
				sbcReport.SourceIp = parts[5];
				sbcReport.SourcePort = parts[6];
				sbcReport.DestIp = parts[7];
				sbcReport.DestPort = parts[8];
				sbcReport.TransportType = parts[9];
				sbcReport.SrcURI = parts[10];
				sbcReport.SrcURIBeforeMap = parts[11];
				sbcReport.DstURI = parts[12];
				sbcReport.DstURIBeforeMap = parts[13];
				sbcReport.Duration = parts[14];
				sbcReport.TrmSd = parts[15];
				sbcReport.TrmReason = parts[16];
				sbcReport.TrmReasonCategory = parts[17];
				sbcReport.SetupTime = parts[18];
				sbcReport.ConnectTime = parts[19];
				sbcReport.ReleaseTime = parts[20];
				sbcReport.RedirectReason = parts[21];
				sbcReport.RedirectURINum = parts[22];
				sbcReport.RedirectURINumBeforeMap = parts[23];
				sbcReport.TxSigIPDiffServ = parts[24];
				sbcReport.IPGroup = parts[25];
				sbcReport.SrdId = parts[26];
				sbcReport.SIPInterfaceId = parts[27];
				sbcReport.ProxySetId = parts[28];
				sbcReport.IpProfileId = parts[29];
				sbcReport.MediaRealmId = parts[30];
				sbcReport.DirectMedia = parts[31];
				sbcReport.SIPTrmReason = parts[32];
				sbcReport.SIPTermDesc = parts[33];
				sbcReport.Caller = parts[34];
				sbcReport.Callee = parts[35];
				sbcReport.Trigger = parts[36];
				sbcReport.LegId = parts[37];
				sbcReport.VoiceAIConnectorName = parts[38];

				return sbcReport;

			}
			if (Line.StartsWith("MEDIA_"))
			{
				MediaReport mediaReport;

				parts = Line.Split('|');
				if (parts.Length != 30) throw new InvalidDataException("Invalid media report format, please check SBC configuration");

				mediaReport = new MediaReport();
				mediaReport.MediaReportType = parts[0];
				mediaReport.SIPCallId = parts[1];
				mediaReport.SessionId = parts[2];
				mediaReport.Cid = parts[3];
				mediaReport.MediaType = parts[4];
				mediaReport.Coder = parts[5];
				mediaReport.Intrv = parts[6];
				mediaReport.LocalRtpIp = parts[7];
				mediaReport.LocalRtpPort = parts[8];
				mediaReport.RemoteRtpIp = parts[9];
				mediaReport.RemoteRtpPort = parts[10];
				mediaReport.InPackets = parts[11];
				mediaReport.OutPackets = parts[12];
				mediaReport.LocalPackLoss = parts[13];
				mediaReport.RemotePackLoss = parts[14];
				mediaReport.RTPdelay = parts[15];
				mediaReport.RTPjitter = parts[16];
				mediaReport.TxRTPssrc = parts[17];
				mediaReport.RxRTPssrc = parts[18];
				mediaReport.LocalRFactor = parts[19];
				mediaReport.RemoteRFactor = parts[20];
				mediaReport.LocalMosCQ = parts[21];
				mediaReport.RemoteMosCQ = parts[22];
				mediaReport.TxRTPIPDiffServ = parts[23];
				mediaReport.LatchedRtpIp = parts[24];
				mediaReport.LatchedRtpPort = parts[25];
				mediaReport.LatchedT38Ip = parts[26];
				mediaReport.LatchedT38Port = parts[27];
				mediaReport.CoderTranscoding = parts[28];
				mediaReport.LegId = parts[29];

				return mediaReport;

			}
			throw new InvalidDataException("Invalid report format, please check SBC configuration");

			

			
		}
	}
}
