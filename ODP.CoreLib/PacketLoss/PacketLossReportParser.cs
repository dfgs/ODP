using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class PacketLossReportParser : IPacketLossReportParser
	{
		private static Regex PacketLossRegex = new Regex(@"(?<TimeStamp>\d\d:\d\d:\d\d\.\d\d\d).*Packets-Loss report.+\[No PL]=(?<Level0>\d+)[^]]+\]=(?<Level1>\d+)[^]]+\]=(?<Level2>\d+)[^]]+\]=(?<Level3>\d+)[^]]+\]=(?<Level4>\d+)[^]]+\]=(?<Level5>\d+)");
		private IDateTimeParser dateTimeParser;



		public PacketLossReportParser(IDateTimeParser DateTimeParser) 
		{
			if (DateTimeParser == null) throw new ArgumentNullException(nameof(DateTimeParser));
			this.dateTimeParser = DateTimeParser;

		}

		public PacketLossReport Parse(string Line)
		{
			Match match;
			PacketLossReport report;
			DateTime? timeStamp;

			if (Line == null) throw new ArgumentNullException(nameof(Line));

			match= PacketLossRegex.Match(Line);
			if (!match.Success) throw new InvalidDataException("Invalid PacketLoss report format, please check SBC configuration");
			
			report = new PacketLossReport();
			timeStamp= dateTimeParser.ParseShortDate(match.Groups["TimeStamp"].Value);
			if (!timeStamp.HasValue) throw new InvalidDataException("Invalid PacketLoss timestamp format, please check SBC configuration");
			report.ReportTime = timeStamp.Value;
			report.CallsCountLevel0 = uint.Parse(match.Groups["Level0"].Value);
			report.CallsCountLevel1 = uint.Parse(match.Groups["Level1"].Value);
			report.CallsCountLevel2 = uint.Parse(match.Groups["Level2"].Value);
			report.CallsCountLevel3 = uint.Parse(match.Groups["Level3"].Value);
			report.CallsCountLevel4 = uint.Parse(match.Groups["Level4"].Value);
			report.CallsCountLevel5 = uint.Parse(match.Groups["Level5"].Value);

			return report;
		}


	}
}
