using RTCPFrameReaderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class RTCPReport
	{
		public string? SessionId { get; set; }

		public DateTime? TimeStamp { get; set; }

		public byte PacketLossPercent { get; set; }

		public uint Jitter { get; set; }

		public uint SSRC { get; set; }
	}
}
