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
	public class PacketLossReportViewModel : ViewModel<PacketLossReport>
	{

		[Browsable(true)]
		public DateTime? ReportTime
		{
			get => Model?.ReportTime;
		}
		[Browsable(true)]
		public uint? CallsCountLevel0
		{
			get => Model?.CallsCountLevel0;
		}
		[Browsable(true)]
		public uint? CallsCountLevel1
		{
			get => Model?.CallsCountLevel1;
		}
		[Browsable(true)]
		public uint? CallsCountLevel2
		{
			get => Model?.CallsCountLevel2;
		}
		[Browsable(true)]
		public uint? CallsCountLevel3
		{
			get => Model?.CallsCountLevel3;
		}
		[Browsable(true)]
		public uint? CallsCountLevel4
		{
			get => Model?.CallsCountLevel4;
		}
		[Browsable(true)]
		public uint? CallsCountLevel5
		{
			get => Model?.CallsCountLevel5;
		}

		[Browsable(true)]
		public uint? TotalCallsCount
		{
			get => Model?.CallsCountLevel0 + Model?.CallsCountLevel1 + Model?.CallsCountLevel2 + Model?.CallsCountLevel3 + Model?.CallsCountLevel4 + Model?.CallsCountLevel5;
		}

		[Browsable(true)]
		public uint? GoodCallsCount
		{
			get => Model?.CallsCountLevel0+ Model?.CallsCountLevel1+ Model?.CallsCountLevel2;
		}
		[Browsable(true)]
		public uint? AverageCallsCount
		{
			get => Model?.CallsCountLevel3;
		}
		[Browsable(true)]
		public uint? BadCallsCount
		{
			get => Model?.CallsCountLevel4 + Model?.CallsCountLevel5;
		}

		[Browsable(true)]
		public double? GoodCallsPercent
		{
			get => GoodCallsCount * 100 / TotalCallsCount;
		}
		[Browsable(true)]
		public double? AverageCallsPercent
		{
			get => AverageCallsCount * 100 / TotalCallsCount;
		}
		[Browsable(true)]
		public double? BadCallsPercent
		{
			get => BadCallsCount * 100 / TotalCallsCount;
		}

		[Browsable(true)]
		public Quality Quality
		{
			get
			{
				if (TotalCallsCount < 10) return Quality.NA;
				if (GoodCallsPercent >= 90) return Quality.Good;
				if (GoodCallsPercent >= 70) return Quality.Average;
				return Quality.Bad;
			}
		}


		public PacketLossReportViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
