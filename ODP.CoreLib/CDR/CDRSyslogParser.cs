﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class CDRSyslogParser : ICDRSyslogParser
	{
		private static Regex CDRRegex = new Regex(@"[^]]+] +\|(?<CDR>(CALL|MEDIA|Call|Media).*)");

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
