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

		private void RefreshWpfPlotPacketLossReportCount(WpfPlot WpfPlot, ViewModelCollection<PacketLossReportViewModel> PacketLossReports)
		{
			PacketLossReportViewModel[] packetLossReports = PacketLossReports.ToArray();
			
			TimeSpan ts = TimeSpan.FromSeconds(15); // time between data points
			long sampleTicks = ts.Ticks;
			double[] positions = packetLossReports.Select(item => (double)(item!.ReportTime!.Value.Ticks / sampleTicks) ).ToArray();

			WpfPlot.Plot.Clear();

			if (positions.Length == 0) return;

			double maxPosition, minPosition,numberOfSamples;

			DateTime startDate = packetLossReports.Min(item => item.ReportTime!.Value);

			maxPosition = positions.Max();
			minPosition = positions.Min();
			numberOfSamples = (maxPosition - minPosition) ;
			double[] completePositions= Enumerable.Range(0, (int)numberOfSamples).Select((item)=>minPosition+ item).ToArray();

			var queryCompleteReports =
				from reportTime in completePositions
				join report in packetLossReports on reportTime equals (double)(report.ReportTime!.Value.Ticks / sampleTicks) into gj
				from subReport in gj.DefaultIfEmpty()
				select subReport;
			PacketLossReportViewModel[] completeReports = queryCompleteReports.ToArray();


			double[] level1Values = completeReports.Select(item => item?.CallsCountLevel1 ?? 0d).ToArray();
			double[] level2Values = completeReports.Select(item => item?.CallsCountLevel2 ?? 0d).ToArray();
			double[] level3Values = completeReports.Select(item => item?.CallsCountLevel3 ?? 0d).ToArray();
			double[] level4Values = completeReports.Select(item => item?.CallsCountLevel4 ?? 0d).ToArray();
			double[] level5Values = completeReports.Select(item => item?.CallsCountLevel5 ?? 0d).ToArray();

			double sampleRate = (double)TimeSpan.TicksPerDay / ts.Ticks;


			//var scatter0 = WpfPlot.Plot.AddSignalConst(goodQualityValues,sampleRate, System.Drawing.Color.Green,"Good quality");
			var scatter1 = WpfPlot.Plot.AddSignalConst(level1Values, sampleRate, null, "Up to 0.5%");
			var scatter2 = WpfPlot.Plot.AddSignalConst(level2Values, sampleRate, null, "0.5% - 1%");
			var scatter3 = WpfPlot.Plot.AddSignalConst(level3Values, sampleRate, null, "1% - 2%");
			var scatter4 = WpfPlot.Plot.AddSignalConst(level4Values, sampleRate, null, "2% - 5%");
			var scatter5 = WpfPlot.Plot.AddSignalConst(level5Values, sampleRate, null, "5% - 100%");

			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			scatter1.OffsetX = startDate.ToOADate();
			scatter2.OffsetX = startDate.ToOADate();
			scatter3.OffsetX = startDate.ToOADate();
			scatter4.OffsetX = startDate.ToOADate();
			scatter5.OffsetX = startDate.ToOADate();


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
		}

		private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ProjectViewModel? project;

			project = e.OldValue as ProjectViewModel;
			if (project != null) project.SessionsChanged -= Project_SessionsChanged;
			project = e.NewValue as ProjectViewModel;
			if (project == null) return;

			project.SessionsChanged += Project_SessionsChanged;
			RefreshCharts();


		}

		private void Project_SessionsChanged(object? sender, EventArgs e)
		{
			RefreshCharts();
		}



	}
}
