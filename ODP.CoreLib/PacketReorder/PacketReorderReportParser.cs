using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class PacketReorderReportParser : IPacketReorderReportParser
	{
		// 2023-03-06 
		private static Regex PacketReorderRegex = new Regex(@"(?<TimeStamp>(\d\d\d\d-\d\d-\d\d )?\d\d:\d\d:\d\d(\.\d\d\d)?).*\[SID=(?<SID>[^]]+)\].*RTP packets reorder.*SeqNum=(?<SeqNumber>\d+).*LastSeqRecv=(?<LastSeqNumber>\d+).*SrcIP=(?<SourceIP>\d+\.\d+\.\d+\.\d+).*SrcPort=(?<SourcePort>\d+)");
		private IDateTimeParser dateTimeParser;



		public PacketReorderReportParser(IDateTimeParser DateTimeParser) 
		{
			if (DateTimeParser == null) throw new ArgumentNullException(nameof(DateTimeParser));
			this.dateTimeParser = DateTimeParser;

		}

		public PacketReorderReport Parse(string Line)
		{
			Match match;
			PacketReorderReport report;
			DateTime? timeStamp;

			if (Line == null) throw new ArgumentNullException(nameof(Line));

			match= PacketReorderRegex.Match(Line);
			if (!match.Success) throw new InvalidDataException("Invalid PacketReorder report format, please check SBC configuration");
			
			report = new PacketReorderReport();
			timeStamp= dateTimeParser.ParseSyslogDate(match.Groups["TimeStamp"].Value);
			if (!timeStamp.HasValue) throw new InvalidDataException("Invalid PacketReorder timestamp format, please check SBC configuration");
			report.ReportTime = timeStamp.Value;
			report.SessionId = match.Groups["SID"].Value;
			report.SourceIP =match.Groups["SourceIP"].Value;
			report.SourcePort = ushort.Parse(match.Groups["SourcePort"].Value);
			report.SequenceNumber = ulong.Parse(match.Groups["SeqNumber"].Value);
			report.LastSequenceNumber = ulong.Parse(match.Groups["LastSeqNumber"].Value);

			return report;
		}


	}
}
