﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ODP.CoreLib
{
	public class CDRReportParser : ICDRReportParser
	{
		private IDateTimeParser dateTimeParser;

		public CDRReportParser(IDateTimeParser DateTimeParser)
		{
			if (DateTimeParser == null) throw new ArgumentNullException(nameof(DateTimeParser));
			this.dateTimeParser = DateTimeParser;
		}

		public CDRReport? Parse(string Line)
		{
			string[] parts;

			if (Line == null) throw new ArgumentNullException(nameof(Line));

			// Ignore headers
			if (Line.StartsWith("SBCReportType")) return null;
			if (Line.StartsWith("MediaReportType")) return null;

			if (Line.StartsWith("CALL_"))
			{
				CDRSBCReport sbcReport;
				
				parts = Line.Split('|');
				if ((parts.Length < 38) || (parts.Length > 39))
					throw new InvalidDataException("Invalid SBC report format, please check SBC configuration");

				sbcReport = new CDRSBCReport();
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
				sbcReport.SetupTime = dateTimeParser.ParseCDRDate(parts[18].Trim());
				sbcReport.ConnectTime = dateTimeParser.ParseCDRDate(parts[19].Trim());
				sbcReport.ReleaseTime = dateTimeParser.ParseCDRDate(parts[20].Trim());
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
				if (parts.Length == 39) sbcReport.VoiceAIConnectorName = parts[38].Trim();
				else sbcReport.VoiceAIConnectorName = "";

				return sbcReport;

			}
			
			// skip media start
			if (Line.StartsWith("MEDIA_START")) return null;

			if (Line.StartsWith("MEDIA_"))
			{
				CDRMediaReport mediaReport;
				string part;

				parts = Line.Split('|');
				if (parts.Length != 30) 
					throw new InvalidDataException("Invalid media report format, please check SBC configuration");

				mediaReport = new CDRMediaReport();
				mediaReport.MediaReportType = parts[0].Trim();
				mediaReport.SIPCallId = parts[1].Trim();
				mediaReport.SessionId = parts[2].Trim();
				mediaReport.Cid = parts[3].Trim();
				mediaReport.MediaType = parts[4].Trim();
				mediaReport.Coder = parts[5].Trim();
				mediaReport.Intrv = parts[6].Trim();
				mediaReport.LocalRtpIp = parts[7].Trim();
				mediaReport.LocalRtpPort = ushort.Parse( parts[8].Trim());
				mediaReport.RemoteRtpIp = parts[9].Trim();
				mediaReport.RemoteRtpPort = ushort.Parse(parts[10].Trim());
				mediaReport.InPackets = long.Parse(parts[11].Trim());
				mediaReport.OutPackets = long.Parse(parts[12].Trim());
				mediaReport.LocalPackLoss = long.Parse(parts[13].Trim());
				mediaReport.RemotePackLoss = long.Parse(parts[14].Trim());
				mediaReport.RTPdelay = int.Parse(parts[15].Trim());

				// uint int conversion bug
				part = parts[16].Trim();
				if (part == "4294967295") mediaReport.RTPjitter = -1;
				else mediaReport.RTPjitter = int.Parse(part);

				part = parts[17].Trim();
				if (part == "-1") mediaReport.TxRTPssrc = null;
				else mediaReport.TxRTPssrc =  uint.Parse( part);

				part = parts[18].Trim();
				if (part == "-1") mediaReport.TxRTPssrc = null;
				else mediaReport.RxRTPssrc = uint.Parse( part);

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
