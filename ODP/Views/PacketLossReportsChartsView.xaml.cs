﻿using ODP.CoreLib;
using ODP.ViewModels;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModelLib;

namespace ODP.Views
{
    /// <summary>
    /// Logique d'interaction pour PacketLossReportsChartsView.xaml
    /// </summary>
    public partial class PacketLossReportsChartsView : UserControl
    {

		private static Quality[] Qualities = Enum.GetValues<Quality>();
		private static string[] QualityLabels = new string[] { "Bad quality", "Average quality", "Good quality", "NA" };
		private static System.Drawing.Color[] SliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green, System.Drawing.Color.Gray };


		public PacketLossReportsChartsView()
        {
            InitializeComponent();
        }


		private void RefreshWpfPlotPacketLossReportCount(WpfPlot WpfPlot, PacketLossReportViewModelCollection PacketLossReports)
		{
			long[] positions;

			TimeSpan ts = TimeSpan.FromSeconds(15); // time between data points
			long sampleTicks = ts.Ticks;
			double sampleRate = (double)TimeSpan.TicksPerDay / ts.Ticks;

			Sample<PacketLossReportViewModel>[] samples = PacketLossReports.Sample(sampleTicks, item => item.ReportTime, item => item).ToArray();



			WpfPlot.Plot.Clear();
			if (samples.Length == 0) return;


			Sample<PacketLossReportViewModel[]>[] values = samples.AggregateAndOrder(items => items.Select(item=>item.Value).ToArray()).ToArray();

			positions = values.GenerateSampledPosition(sampleTicks);


			var queryJoinedValues =
				from position in positions
				join value in values on position equals value.Ticks into gj
				from joinedValue in  gj.DefaultIfEmpty()
				select joinedValue.Value;
			PacketLossReportViewModel[][] joinedValues = queryJoinedValues.ToArray();

			//var scatter0 = WpfPlot.Plot.AddSignalConst(goodQualityValues,sampleRate, System.Drawing.Color.Green,"Good quality");
			var scatter1 = WpfPlot.Plot.AddSignalConst(joinedValues.Select(item => item?.Sum(item => item.CallsCountLevel1 ?? 0) ?? 0).ToArray(), sampleRate, null, "Up to 0.5%");
			var scatter2 = WpfPlot.Plot.AddSignalConst(joinedValues.Select(item => item?.Sum(item => item.CallsCountLevel2 ?? 0) ?? 0).ToArray(), sampleRate, null, "0.5% - 1%");
			var scatter3 = WpfPlot.Plot.AddSignalConst(joinedValues.Select(item => item?.Sum(item => item.CallsCountLevel3 ?? 0) ?? 0).ToArray(), sampleRate, null, "1% - 2%");
			var scatter4 = WpfPlot.Plot.AddSignalConst(joinedValues.Select(item => item?.Sum(item => item.CallsCountLevel4 ?? 0) ?? 0).ToArray(), sampleRate, null, "2% - 5%");
			var scatter5 = WpfPlot.Plot.AddSignalConst(joinedValues.Select(item => item?.Sum(item => item.CallsCountLevel5 ?? 0) ?? 0).ToArray(), sampleRate, null, "5% - 100%");

			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			scatter1.OffsetX = new DateTime(values[0].Ticks).ToOADate();
			scatter2.OffsetX = new DateTime(values[0].Ticks).ToOADate();
			scatter3.OffsetX = new DateTime(values[0].Ticks).ToOADate();
			scatter4.OffsetX = new DateTime(values[0].Ticks).ToOADate();
			scatter5.OffsetX = new DateTime(values[0].Ticks).ToOADate();


			WpfPlot.Plot.Legend(true, Alignment.UpperRight);

			WpfPlot.Refresh();

		}

		private void RefreshWpfPlotCallAdmissionControlErrorCount(WpfPlot WpfPlot, IEnumerable<CallViewModel> Calls)
		{
			long[] positions;
			CallViewModel[] validCalls;


			TimeSpan ts = TimeSpan.FromSeconds(30); // time between data points
			long sampleTicks = ts.Ticks;
			double sampleRate = (double)TimeSpan.TicksPerDay / ts.Ticks;
			string[] IPGroups = Calls.Select(item => item.IPGroup ?? "Unknow").Distinct().ToArray();

			WpfPlot.Plot.Clear();


			foreach (string IPGroup in IPGroups)
			{
				validCalls = Calls.Where(item => ((item.TrmReason== "RELEASE_BECAUSE_IN_ADMISSION_FAILED") || (item.TrmReason == "RELEASE_BECAUSE_OUT_ADMISSION_FAILED")) && (item.IPGroup == IPGroup)).ToArray();
				Sample<int>[] samples = validCalls.Sample(sampleTicks, item => item.SetupTime, item => 1).ToArray();

				if (samples.Length == 0) continue;

				Sample<int>[] values = samples.AggregateAndOrder(items => items.Sum(item => item.Value)).ToArray();

				positions = values.GenerateSampledPosition(sampleTicks);

				var queryJoinedValues =
					from position in positions
					join value in values on position equals value.Ticks into gj
					from joinedValue in gj.DefaultIfEmpty()
					select joinedValue.Value;

				int[] joinedValues = queryJoinedValues.ToArray();

				
				var scatter1 = WpfPlot.Plot.AddSignalConst(joinedValues, sampleRate, null, IPGroup);

				WpfPlot.Plot.XAxis.DateTimeFormat(true);
				scatter1.OffsetX = new DateTime(values[0].Ticks).ToOADate();

			}

			WpfPlot.Plot.Legend(true, Alignment.UpperRight);

			WpfPlot.Refresh();

		}

		private void RefreshWpfPlotActiveSessionsCount(WpfPlot WpfPlot, IEnumerable<SessionViewModel> Sessions)
		{
			long[] positions;
			SessionViewModel[] validSessions;

			TimeSpan ts = TimeSpan.FromSeconds(30); // time between data points
			long sampleTicks = ts.Ticks;
			double sampleRate = (double)TimeSpan.TicksPerDay / ts.Ticks;

			validSessions = Sessions.Where(item => (item.StartTime.HasValue) ).ToArray();

			Sample<int>[] samples = validSessions.Sample(sampleTicks, item => item.StartTime, item => 1)
								.Concat(validSessions.Sample(sampleTicks, item => item.StopTime, item => -1)).ToArray();				

			WpfPlot.Plot.Clear();
			if (samples.Length == 0) return;

			Sample<int>[] values = samples.AggregateAndOrder(items => items.Sum(item => item.Value)).ToArray();

			positions = values.GenerateSampledPosition(sampleTicks);
						
			var queryJoinedValues =
				from position in positions
				join value in values on position equals value.Ticks into gj
				from joinedValue in gj.DefaultIfEmpty()
				select joinedValue.Value;
			
			int[] joinedValues = queryJoinedValues.Accumulate(0).ToArray();

			/*string line = "";
			for (int t=0;t<joinedValues.Length;t++)
			{
				line += $"{new DateTime(positions[t])};Global;{joinedValues[t]}\r\n";
			}//*/

			var scatter1 = WpfPlot.Plot.AddSignalConst(joinedValues , sampleRate, null, "Sessions count");

			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			scatter1.OffsetX = new DateTime(values[0].Ticks).ToOADate();
			
			WpfPlot.Plot.Legend(true, Alignment.UpperRight);

			WpfPlot.Refresh();

		}
		private void RefreshWpfPlotActiveCallsCount(WpfPlot WpfPlot, IEnumerable<CallViewModel> Calls)
		{
			long[] positions;
			CallViewModel[] validCalls;


			TimeSpan ts = TimeSpan.FromSeconds(30); // time between data points
			long sampleTicks = ts.Ticks;
			double sampleRate = (double)TimeSpan.TicksPerDay / ts.Ticks;
			string[] IPGroups = Calls.Select(item => item.IPGroup??"Unknow").Distinct().ToArray();

			WpfPlot.Plot.Clear();



			foreach (string IPGroup in IPGroups)
			{

				validCalls = Calls.Where(item => (item.SetupTime.HasValue) && (item.IPGroup == IPGroup)).ToArray();
				Sample<int>[] samples = validCalls.Sample(sampleTicks, item => item.SetupTime, item => 1)
									.Concat(validCalls.Sample(sampleTicks, item => item.ReleaseTime, item => -1)).ToArray();

				if (samples.Length == 0) continue;

				Sample<int>[] values = samples.AggregateAndOrder(items => items.Sum(item => item.Value)).ToArray();

				positions = values.GenerateSampledPosition(sampleTicks);

				var queryJoinedValues =
					from position in positions
					join value in values on position equals value.Ticks into gj
					from joinedValue in gj.DefaultIfEmpty()
					select joinedValue.Value;

				int[] joinedValues = queryJoinedValues.Accumulate(0).ToArray();

				/*string line = "";
				for (int t=0;t<joinedValues.Length;t++)
				{
					line += $"{new DateTime(positions[t])};{IPGroup};{joinedValues[t]}\r\n";
				}*/

				var scatter1 = WpfPlot.Plot.AddSignalConst(joinedValues, sampleRate, null, IPGroup);

				WpfPlot.Plot.XAxis.DateTimeFormat(true);
				scatter1.OffsetX = new DateTime(values[0].Ticks).ToOADate();
			}

			WpfPlot.Plot.Legend(true, Alignment.UpperRight);

			WpfPlot.Refresh();

		}


		private void RefreshCharts()
		{
			ProjectViewModel? project;
			project = DataContext as ProjectViewModel;

			//return;
			if (project == null) return;



			RefreshWpfPlotPacketLossReportCount(WpfPlotPacketLossReportCount.WpfPlot, project.PacketLossReports);
			RefreshWpfPlotCallAdmissionControlErrorCount(WpfPlotCallAdmissionControlErrorCount.WpfPlot, project.FilteredSessions.Calls());
			RefreshWpfPlotActiveSessionsCount(WpfPlotActiveSessionsCount.WpfPlot, project.FilteredSessions);
			RefreshWpfPlotActiveCallsCount(WpfPlotActiveCallsCount.WpfPlot, project.FilteredSessions.Calls());
		}

		private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ProjectViewModel? project;

			project = e.OldValue as ProjectViewModel;
			if (project != null) project.FilteredSessionsChanged -= Project_SessionsChanged;
			project = e.NewValue as ProjectViewModel;
			if (project == null) return;

			project.FilteredSessionsChanged += Project_SessionsChanged;
			RefreshCharts();


		}

		private void Project_SessionsChanged(object? sender, EventArgs e)
		{
			RefreshCharts();
		}

		private void MaximizeCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}

		private void MaximizeCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ChartView? clickedView;
			clickedView = e.Parameter as ChartView;
			ProjectViewModel? project;
			project = DataContext as ProjectViewModel;

			if (project == null) return;

			if (clickedView == null) return;

			if (clickedView == WpfPlotMaximized)
			{
				WpfPlotMaximized.IsMaximized = false;
				return;
			}

			WpfPlotMaximized.Title = clickedView.Title;
			WpfPlotMaximized.IsMaximized = true;

			if (clickedView == WpfPlotPacketLossReportCount)
			{
				RefreshWpfPlotPacketLossReportCount(WpfPlotMaximized.WpfPlot, project.PacketLossReports);
				return;
			}
			if (clickedView == WpfPlotCallAdmissionControlErrorCount)
			{
				RefreshWpfPlotCallAdmissionControlErrorCount(WpfPlotMaximized.WpfPlot, project.FilteredSessions.Calls());
				return;
			}
			if (clickedView == WpfPlotActiveSessionsCount)
			{
				RefreshWpfPlotActiveSessionsCount(WpfPlotMaximized.WpfPlot, project.FilteredSessions);
				return;
			}
			if (clickedView == WpfPlotActiveCallsCount)
			{
				RefreshWpfPlotActiveCallsCount(WpfPlotMaximized.WpfPlot, project.FilteredSessions.Calls());
				return;
			}
		}





	}
}
