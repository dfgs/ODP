using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{

	// 17:15:52.048  172.200.0.10  local0.warn    [S=1030955] [BID=c64c90:24]  Packets-Loss report [PL range]=#media-legs:
	// [No PL]=17, [up to 0.5%]=0, [0.5% - 1%]=1, [1% - 2%]=0, [2% - 5%]=0, [5% - 100%]=0
	// [Time:20-02@17:15:50.930]

	public class PacketReorderReport
	{
		public DateTime ReportTime { get; set; }
		public string SourceIP { get; set; } = "";
		public ushort SourcePort { get; set; }

		
	}
}
