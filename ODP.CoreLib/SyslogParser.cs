using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class SyslogParser : ISyslogParser
	{
		private static Regex CDRRegex = new Regex(@"[^|]+\|(?<CDR>.*)");

		public string? Parse(string? Syslog)
		{
			Match match;

			if (Syslog == null) return null;

			match = CDRRegex.Match(Syslog);
			if (!match.Success) return null;

			return match.Groups["CDR"].Value;
		}
	}
}
