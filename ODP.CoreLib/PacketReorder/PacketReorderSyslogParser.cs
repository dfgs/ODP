using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class PacketReorderSyslogParser : IPacketReorderSyslogParser
	{
		private static Regex PacketReorderRegex = new Regex(@"RTP packets reorder");

		public string? Parse(string? Syslog)
		{
			Match match;

			if (Syslog == null) return null;

			match = PacketReorderRegex.Match(Syslog);
			if (!match.Success) return null;

			return Syslog;
		}
	}
}
