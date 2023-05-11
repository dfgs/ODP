using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class PacketLossSyslogParser : IPacketLossSyslogParser
	{
		private static Regex PacketLossRegex = new Regex(@"Packets-Loss report");

		public string? Parse(string? Syslog)
		{
			Match match;

			if (Syslog == null) return null;

			match = PacketLossRegex.Match(Syslog);
			if (!match.Success) return null;

			return Syslog;
		}
	}
}
