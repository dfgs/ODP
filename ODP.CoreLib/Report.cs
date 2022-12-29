using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public abstract class Report
	{
		public string? SIPCallId { get; set; }
		public string? SessionId { get; set; }

	}
}
