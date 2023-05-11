using ODP.CoreLib;
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
	/// Logique d'interaction pour ChartsView.xaml
	/// </summary>
	public partial class ChartsView : UserControl
	{
		private static Quality[] Qualities = Enum.GetValues<Quality>();
		private static string[] QualityLabels = new string[] { "Bad quality", "Average quality", "Good quality", "NA" };
		private static System.Drawing.Color[] SliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green, System.Drawing.Color.Gray };
		//private static double chartScale = 0.7;





		public ChartsView()
		{
			InitializeComponent();

		}
		


		private void RefreshWpfPlotMediaReportCountByQuality(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CDRMediaReportViewModel[] mediaReports = Sessions.Calls().WithAudio().MediaReports().ToArray();
			double[] values = mediaReports.GroupByQuality(Qualities).Select(mediaReports => (double)mediaReports.Count()).ToArray();


			WpfPlot.Plot.Clear();

			var pie = WpfPlot.Plot.AddPie(values);
			pie.DonutSize = .6;
			pie.Explode = true;
			pie.ShowValues = true;
			pie.SliceLabels = QualityLabels;
			pie.SliceFillColors = SliceColors;
			
			WpfPlot.Plot.Legend(true,Alignment.UpperRight);



			try
			{
				WpfPlot.Refresh();
				
			}
			catch (ArgumentException)
			{
				// bug in graphics.drawPie
			}
		}
		private void RefreshWpfPlotCallsCountByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();
			double[][] values = calls.GroupByQuality(Qualities)
				.Select(groupedCalls => groupedCalls.GroupByInterface(sipInterfaces).Select(calls => (double)calls.Count()).ToArray()
				).ToArray();



			WpfPlot.Plot.Clear();
			

			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var bars=WpfPlot.Plot.AddBarGroups(sipInterfaces, QualityLabels, values, null);
				for(int t=0;t<bars.Length;t++)
				{
					bars[t].FillColor= SliceColors[t];
					bars[t].ShowValuesAboveBars = true;
				}

				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
				// adjust axis limits so there is no padding below the bar graph
				WpfPlot.Plot.SetAxisLimits(yMin: 0);
				WpfPlot.Plot.XAxis.TickLabelStyle(null,null,null,null,45);
			}

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotMaxPacketLossByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.MaxPacketLoss()).ToArray();
			double[] positions = Enumerable.Range(1, sipInterfaces.Length).Select(i=>(double)i).ToArray();


			WpfPlot.Plot.Clear();
			
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				var bar = WpfPlot.Plot.AddBar(values,positions);
				bar.ShowValuesAboveBars = true;
				bar.ValueFormatter= value => String.Format("{0:0.00}", value) ;
				WpfPlot.Plot.XTicks(positions, sipInterfaces);
				WpfPlot.Plot.XAxis.TickLabelStyle(null, null, null, null, 45);
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
			//WpfPlot.Plot.AxisAuto(chartScale,chartScale);
		}
		private void RefreshWpfPlotMaxDelayByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => (double)item.MaxDelay()).ToArray();
			double[] positions = Enumerable.Range(1, sipInterfaces.Length).Select(i => (double)i).ToArray();



			WpfPlot.Plot.Clear();
			
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				var bar = WpfPlot.Plot.AddBar(values, positions);
				bar.ShowValuesAboveBars = true;
				bar.ValueFormatter = value => ((int)value).ToString();
				WpfPlot.Plot.XTicks(positions, sipInterfaces);
				WpfPlot.Plot.XAxis.TickLabelStyle(null, null, null, null, 45);
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
			//WpfPlot.Plot.AxisAuto(chartScale, chartScale);
		}
		private void RefreshWpfPlotMaxJitterByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => (double)item.MaxJitter()).ToArray();
			double[] positions = Enumerable.Range(1, sipInterfaces.Length).Select(i => (double)i).ToArray();


			WpfPlot.Plot.Clear();
			
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				var bar = WpfPlot.Plot.AddBar(values, positions);
				bar.ShowValuesAboveBars = true;
				bar.ValueFormatter = value => ((int)value).ToString();
				WpfPlot.Plot.XTicks(positions, sipInterfaces);
				WpfPlot.Plot.XAxis.TickLabelStyle(null, null, null, null, 45);
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
			//WpfPlot.Plot.AxisAuto(chartScale, chartScale);
		}
		private void RefreshWpfPlotAvgPacketLossByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.AvgPacketLoss()).ToArray();
			double[] positions = Enumerable.Range(1, sipInterfaces.Length).Select(i => (double)i).ToArray();

			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;


			if (sipInterfaces.Length > 0)
			{
				var bar = WpfPlot.Plot.AddBar(values, positions);
				bar.ShowValuesAboveBars = true;
				bar.ValueFormatter = value => String.Format("{0:0.00}", value);
				WpfPlot.Plot.XTicks(positions, sipInterfaces);
				WpfPlot.Plot.XAxis.TickLabelStyle(null, null, null, null, 45);
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
			//WpfPlot.Plot.AxisAuto(chartScale, chartScale);
		}
		private void RefreshWpfPlotAvgDelayByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.AvgDelay()).ToArray();
			double[] positions = Enumerable.Range(1, sipInterfaces.Length).Select(i => (double)i).ToArray();



			WpfPlot.Plot.Clear();
			
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				var bar = WpfPlot.Plot.AddBar(values, positions);
				bar.ShowValuesAboveBars = true;
				bar.ValueFormatter = value => ((int)value).ToString();
				WpfPlot.Plot.XTicks(positions, sipInterfaces);
				WpfPlot.Plot.XAxis.TickLabelStyle(null, null, null, null, 45);
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
			//WpfPlot.Plot.AxisAuto(chartScale, chartScale);
		}
		private void RefreshWpfPlotAvgJitterByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.AvgJitter()).ToArray();
			double[] positions = Enumerable.Range(1, sipInterfaces.Length).Select(i => (double)i).ToArray();



			WpfPlot.Plot.Clear();
			
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				var bar = WpfPlot.Plot.AddBar(values, positions);
				bar.ShowValuesAboveBars = true;
				bar.ValueFormatter = value => ((int)value).ToString();
				WpfPlot.Plot.XTicks(positions, sipInterfaces);
				WpfPlot.Plot.XAxis.TickLabelStyle(null, null, null, null, 45);
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
			//WpfPlot.Plot.AxisAuto(chartScale, chartScale);
		}


		private void RefreshCharts()
		{
			ProjectViewModel? project;
			project = DataContext as ProjectViewModel;

			//return;
			if (project == null) return;

			

			RefreshWpfPlotMediaReportCountByQuality(WpfPlotMediaReportCountByQuality.WpfPlot, project.Sessions);
			RefreshWpfPlotCallsCountByInterface(WpfPlotCallsCountByInterface.WpfPlot, project.Sessions);
			RefreshWpfPlotMaxPacketLossByInterface(WpfPlotMaxPacketLossByInterface.WpfPlot, project.Sessions);
			RefreshWpfPlotMaxDelayByInterface(WpfPlotMaxDelayByInterface.WpfPlot, project.Sessions);
			RefreshWpfPlotMaxJitterByInterface(WpfPlotMaxJitterByInterface.WpfPlot, project.Sessions);
			RefreshWpfPlotAvgPacketLossByInterface(WpfPlotAvgPacketLossByInterface.WpfPlot, project.Sessions);
			RefreshWpfPlotAvgDelayByInterface(WpfPlotAvgDelayByInterface.WpfPlot, project.Sessions);
			RefreshWpfPlotAvgJitterByInterface(WpfPlotAvgJitterByInterface.WpfPlot, project.Sessions);
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

		private void MaximizeCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;e.Handled= true;
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

			if (clickedView == WpfPlotMediaReportCountByQuality)
			{
				RefreshWpfPlotMediaReportCountByQuality(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotCallsCountByInterface)
			{
				RefreshWpfPlotCallsCountByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotMaxPacketLossByInterface)
			{
				RefreshWpfPlotMaxPacketLossByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotMaxJitterByInterface)
			{
				RefreshWpfPlotMaxJitterByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotMaxDelayByInterface)
			{
				RefreshWpfPlotMaxDelayByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotAvgPacketLossByInterface)
			{
				RefreshWpfPlotAvgPacketLossByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotAvgJitterByInterface)
			{
				RefreshWpfPlotAvgJitterByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}
			if (clickedView == WpfPlotAvgDelayByInterface)
			{
				RefreshWpfPlotAvgDelayByInterface(WpfPlotMaximized.WpfPlot, project.Sessions);
				return;
			}

		}


	}
}
