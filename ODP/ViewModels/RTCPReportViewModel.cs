using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class RTCPReportViewModel : ViewModel<RTCPReport>
	{


		[Browsable(true)]
		public string? SessionId
		{
			get => Model?.SessionId;
		}
		[Browsable(true)]
		public DateTime? TimeStamp
		{
			get => Model?.TimeStamp;
		}
		[Browsable(true)]
		public byte? PacketLossPercent
		{
			get => Model?.PacketLossPercent;
		}
		[Browsable(true)]
		public uint? Jitter
		{
			get => Model?.Jitter;
		}

		[Browsable(true)]
		public uint? SSRC
		{
			get => Model?.SSRC;
		}

		[Browsable(true)]
		public string? SourceName
		{
			get=>Model?.SourceName;
		}


		public RTCPReportViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
