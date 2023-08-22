using ODP.CoreLib;
using ODP.ViewModels;
using ScottPlot;
using System;
using System.Collections.Generic;
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
	/// Logique d'interaction pour CallChartsView.xaml
	/// </summary>
	public partial class CallChartsView : UserControl
	{
		private static System.Drawing.Color[] SliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green, System.Drawing.Color.Gray };

		public CallChartsView()
		{
			InitializeComponent();
		}



		private void RefreshWpfPlotPacketLoss(WpfPlot WpfPlot, CallViewModel Call)
		{
			double[] positions;
			double[] values;
			RTCPReportViewModel[] samples;

			WpfPlot.Plot.Clear();

			foreach(CDRMediaReportViewModel mediaReport in Call.MediaReports)
			{
				foreach (var groupedReports in mediaReport.TxRTCPReports.GroupBy(item => item.SourceName))
				{
					samples = groupedReports.ToArray();

					if (samples.Length == 0) continue;

					positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
					values = samples.Select(item => (double)item.PacketLossPercent!.Value).ToArray();

					var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
					scatter1.Label = $"{groupedReports.Key} ({mediaReport.LocalRtpIp}->{mediaReport.RemoteRtpIp})";

				}//*/
				foreach (var groupedReports in mediaReport.RxRTCPReports.GroupBy(item => item.SourceName))
				{
					samples = groupedReports.ToArray();

					if (samples.Length == 0) continue;

					positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
					values = samples.Select(item => (double)item.PacketLossPercent!.Value).ToArray();

					var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
					scatter1.Label = $"{groupedReports.Key} ({mediaReport.RemoteRtpIp}->{mediaReport.LocalRtpIp})";

				}//*/
			}


			WpfPlot.Plot.YLabel("%");
			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			WpfPlot.Plot.Legend(true, Alignment.UpperRight);
			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotDelay(WpfPlot WpfPlot, CallViewModel Call)
		{
			double[] positions;
			double[] values;
			RTCPReportViewModel[] samples;

			WpfPlot.Plot.Clear();

			foreach (CDRMediaReportViewModel mediaReport in Call.MediaReports)
			{
				foreach (var groupedReports in mediaReport.TxRTCPReports.GroupBy(item => item.SourceName))
				{
					samples = groupedReports.ToArray();

					if (samples.Length == 0) continue;

					positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
					values = samples.Select(item => (double)item.Jitter!.Value).ToArray();

					var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
					scatter1.Label = $"{groupedReports.Key} ({mediaReport.LocalRtpIp}->{mediaReport.RemoteRtpIp})";

				}//*/
				foreach (var groupedReports in mediaReport.RxRTCPReports.GroupBy(item => item.SourceName))
				{
					samples = groupedReports.ToArray();

					if (samples.Length == 0) continue;

					positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
					values = samples.Select(item => (double)item.Jitter!.Value).ToArray();

					var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
					scatter1.Label = $"{groupedReports.Key} ({mediaReport.RemoteRtpIp}->{mediaReport.LocalRtpIp})";

				}//*/
			}



			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			WpfPlot.Plot.Legend(true, Alignment.UpperRight);
			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotJitter(WpfPlot WpfPlot, CallViewModel Call)
		{
			double[] positions;
			double[] values;
			RTCPReportViewModel[] samples;

			WpfPlot.Plot.Clear();

			foreach (CDRMediaReportViewModel mediaReport in Call.MediaReports)
			{
				foreach (var groupedReports in mediaReport.TxRTCPReports.GroupBy(item => item.SourceName))
				{
					samples = groupedReports.ToArray();

					if (samples.Length == 0) continue;

					positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
					values = samples.Select(item => (double)item.Jitter!.Value).ToArray();

					var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
					scatter1.Label = $"{groupedReports.Key} ({mediaReport.LocalRtpIp}->{mediaReport.RemoteRtpIp})";

				}//*/
				foreach (var groupedReports in mediaReport.RxRTCPReports.GroupBy(item => item.SourceName))
				{
					samples = groupedReports.ToArray();

					if (samples.Length == 0) continue;

					positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
					values = samples.Select(item => (double)item.Jitter!.Value).ToArray();

					var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
					scatter1.Label = $"{groupedReports.Key} ({mediaReport.RemoteRtpIp}->{mediaReport.LocalRtpIp})";

				}//*/
			}


			WpfPlot.Plot.YLabel("ts");
			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			WpfPlot.Plot.Legend(true, Alignment.UpperRight);
			WpfPlot.Refresh();

		}

		private void RefreshCharts()
		{
			CallViewModel? call;
			call = DataContext as CallViewModel;

			//return;
			if (call == null) return;
			RefreshWpfPlotPacketLoss(WpfPlotPacketLoss.WpfPlot, call);
			RefreshWpfPlotJitter(WpfPlotJitter.WpfPlot, call);
	

		}



		

		private void MaximizeCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}

		private void MaximizeCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ChartView? clickedView;
			clickedView = e.Parameter as ChartView;
			CallViewModel? call;
			call = DataContext as CallViewModel;

			if (call == null) return;

			if (clickedView == null) return;

			if (clickedView == WpfPlotMaximized)
			{
				WpfPlotMaximized.IsMaximized = false;
				return;
			}

			WpfPlotMaximized.Title = clickedView.Title;
			WpfPlotMaximized.IsMaximized = true;

			if (clickedView == WpfPlotPacketLoss)
			{
				RefreshWpfPlotPacketLoss(WpfPlotMaximized.WpfPlot, call);
				return;
			}
		
			if (clickedView == WpfPlotJitter)
			{
				RefreshWpfPlotJitter(WpfPlotMaximized.WpfPlot, call);
				return;
			}

			

		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshCharts();
		}

	}
}
