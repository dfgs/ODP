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



		private void RefreshWpfPlotRemotePacketLoss(WpfPlot WpfPlot, ViewModelCollection<RTCPReportViewModel> RTCPReports)
		{
			double[] positions;
			double[] values;


			RTCPReportViewModel[] samples = RTCPReports.ToArray();

			WpfPlot.Plot.Clear();
			if (samples.Length == 0) return;

			positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
			values = samples.Select(item => (double)item.PacketLossPercent!.Value).ToArray();

			var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
			scatter1.Label = "Packet loss %";

			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			scatter1.OffsetX = values[0];
			
			WpfPlot.Plot.Legend(true, Alignment.UpperRight);

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotRemoteDelay(WpfPlot WpfPlot, ViewModelCollection<RTCPReportViewModel> RTCPReports)
		{

		}
		private void RefreshWpfPlotRemoteJitter(WpfPlot WpfPlot, ViewModelCollection<RTCPReportViewModel> RTCPReports)
		{

		}
		private void RefreshWpfPlotLocalPacketLoss(WpfPlot WpfPlot, ViewModelCollection<RTCPReportViewModel> RTCPReports)
		{
			double[] positions;
			double[] values;


			RTCPReportViewModel[] samples = RTCPReports.ToArray();

			WpfPlot.Plot.Clear();
			if (samples.Length == 0) return;

			positions = samples.Select(item => item.TimeStamp!.Value.ToOADate()).ToArray();
			values = samples.Select(item => (double)item.PacketLossPercent!.Value).ToArray();

			var scatter1 = WpfPlot.Plot.AddScatter(positions, values);
			scatter1.Label = "Packet loss %";

			WpfPlot.Plot.XAxis.DateTimeFormat(true);
			scatter1.OffsetX = values[0];

			WpfPlot.Plot.Legend(true, Alignment.UpperRight);

			WpfPlot.Refresh();
		}

		private void RefreshWpfPlotLocalDelay(WpfPlot WpfPlot, ViewModelCollection<RTCPReportViewModel> RTCPReports)
		{

		}
		private void RefreshWpfPlotLocalJitter(WpfPlot WpfPlot, ViewModelCollection<RTCPReportViewModel> RTCPReports)
		{

		}



		private void RefreshCharts()
		{
			CallViewModel? call;
			call = DataContext as CallViewModel;

			//return;
			if (call == null) return;
			RefreshWpfPlotRemotePacketLoss(WpfPlotRemotePacketLoss.WpfPlot, call.LocalRTCPReports);
			RefreshWpfPlotRemoteDelay(WpfPlotRemoteDelay.WpfPlot, call.LocalRTCPReports);
			RefreshWpfPlotRemoteJitter(WpfPlotRemoteJitter.WpfPlot, call.LocalRTCPReports);
	
			RefreshWpfPlotLocalPacketLoss(WpfPlotLocalPacketLoss.WpfPlot, call.RemoteRTCPReports);
			RefreshWpfPlotLocalDelay(WpfPlotLocalDelay.WpfPlot, call.RemoteRTCPReports);
			RefreshWpfPlotLocalJitter(WpfPlotLocalJitter.WpfPlot, call.RemoteRTCPReports);


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

			if (clickedView == WpfPlotRemotePacketLoss)
			{
				RefreshWpfPlotRemotePacketLoss(WpfPlotMaximized.WpfPlot, call.LocalRTCPReports);
				return;
			}
			if (clickedView == WpfPlotRemoteDelay)
			{
				RefreshWpfPlotRemoteDelay(WpfPlotMaximized.WpfPlot, call.LocalRTCPReports);
				return;
			}
			if (clickedView == WpfPlotRemoteJitter)
			{
				RefreshWpfPlotRemoteJitter(WpfPlotMaximized.WpfPlot, call.LocalRTCPReports);
				return;
			}

			if (clickedView == WpfPlotLocalPacketLoss)
			{
				RefreshWpfPlotLocalPacketLoss(WpfPlotMaximized.WpfPlot, call.RemoteRTCPReports);
				return;
			}
			if (clickedView == WpfPlotLocalDelay)
			{
				RefreshWpfPlotLocalDelay(WpfPlotMaximized.WpfPlot, call.RemoteRTCPReports);
				return;
			}
			if (clickedView == WpfPlotLocalJitter)
			{
				RefreshWpfPlotLocalJitter(WpfPlotMaximized.WpfPlot, call.RemoteRTCPReports);
				return;
			}

		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshCharts();
		}

	}
}
