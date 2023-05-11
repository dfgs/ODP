using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public interface IPacketLossSyslogParser
	{
		string? Parse(string? Syslog);
	}
}
