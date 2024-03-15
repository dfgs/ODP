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
	public class PacketReorderReportViewModel : GenericViewModel<PacketReorderReport>
	{

		[Browsable(true)]
		public DateTime? ReportTime
		{
			get => Model?.ReportTime;
		}

		[Browsable(true)]
		public string? SessionId
		{
			get => Model?.SessionId;
		}

		[Browsable(true)]
		public string? SourceIP
		{
			get => Model?.SourceIP;
		}

		[Browsable(true)]
		public ushort? SourcePort
		{
			get => Model?.SourcePort;
		}

		[Browsable(true)]
		public ulong? SequenceNumber
		{
			get => Model?.SequenceNumber;
		}
		[Browsable(true)]
		public ulong? LastSequenceNumber
		{
			get => Model?.LastSequenceNumber;
		}


		public PacketReorderReportViewModel(PacketReorderReport Model) : base(Model)
		{
		}
	}
}
