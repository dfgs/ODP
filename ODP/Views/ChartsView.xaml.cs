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
		private static string[] QualityLabels = new string[] { "Bad quality", "Average quality", "Good quality" };
		private static System.Drawing.Color[] SliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green };

		public ChartsView()
		{
			InitializeComponent();
			
		}
		

		private void RefreshWpfPlotMediaReportCountByQuality(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			MediaReportViewModel[] mediaReports = Sessions.Calls().MediaReports().ToArray();
			double[] values = mediaReports.GroupByQuality(Qualities).Select(mediaReports => (double)mediaReports.Count()).ToArray();


			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Number of media reports by quality");

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
			WpfPlot.Plot.Title("Number of calls by interface");

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
			}

			WpfPlot.Refresh();
		}
	
		
		
		
		private void RefreshWpfPlotMaxPacketLossByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => (double)item.MaxPacketLoss()).ToArray();
				


			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Max packet loss by interface");
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var gauge = WpfPlot.Plot.AddRadialGauge(values);
				gauge.CircularBackground = false;
				gauge.MaximumAngle = 180;
				gauge.StartingAngle = 180;

				gauge.Labels = sipInterfaces;
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotMaxDelayByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => (double)item.MaxDelay()).ToArray();



			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Max delay by interface");
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var gauge = WpfPlot.Plot.AddRadialGauge(values);
				gauge.CircularBackground = false;
				gauge.MaximumAngle = 180;
				gauge.StartingAngle = 180;

				gauge.Labels = sipInterfaces;
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotMaxJitterByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => (double)item.MaxJitter()).ToArray();


			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Max jitter by interface");
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var gauge = WpfPlot.Plot.AddRadialGauge(values);
				gauge.CircularBackground = false;
				gauge.MaximumAngle = 180;
				gauge.StartingAngle = 180;

				gauge.Labels = sipInterfaces;
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotAvgPacketLossByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.AvgPacketLoss()).ToArray();

			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Avg packet loss by interface");
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;


			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var gauge = WpfPlot.Plot.AddRadialGauge(values);
				gauge.CircularBackground = false;
				gauge.MaximumAngle = 180;
				gauge.StartingAngle = 180;

				gauge.Labels = sipInterfaces;
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotAvgDelayByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.AvgDelay()).ToArray();



			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Avg delay by interface");
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var gauge = WpfPlot.Plot.AddRadialGauge(values);
				gauge.CircularBackground = false;
				gauge.MaximumAngle = 180;
				gauge.StartingAngle = 180;

				gauge.Labels = sipInterfaces;
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
		}
		private void RefreshWpfPlotAvgJitterByInterface(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			CallViewModel[] calls = Sessions.Calls().ToArray();
			string[] sipInterfaces = calls.SIPInterfaces().ToArray();

			double[] values = sipInterfaces.GroupJoin(calls, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item.AvgJitter()).ToArray();



			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Avg jitter by interface");
			WpfPlot.Plot.Palette = ScottPlot.Palette.Nord;

			if (sipInterfaces.Length > 0)
			{
				// add the grouped bar plots and show a legend
				var gauge = WpfPlot.Plot.AddRadialGauge(values);
				gauge.CircularBackground = false;
				gauge.MaximumAngle = 180;
				gauge.StartingAngle = 180;

				gauge.Labels = sipInterfaces;
				WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			}

			WpfPlot.Refresh();
		}





		private void RefreshCharts()
		{
			ViewModelCollection<SessionViewModel>? sessions;

			sessions = DataContext as ViewModelCollection<SessionViewModel>;
			if (sessions == null) return;

			RefreshWpfPlotMediaReportCountByQuality(WpfPlotMediaReportCountByQuality, sessions);
			RefreshWpfPlotCallsCountByInterface(WpfPlotCallsCountByInterface, sessions);
			RefreshWpfPlotMaxPacketLossByInterface(WpfPlotMaxPacketLossByInterface, sessions);
			RefreshWpfPlotMaxDelayByInterface(WpfPlotMaxDelayByInterface, sessions);
			RefreshWpfPlotMaxJitterByInterface(WpfPlotMaxJitterByInterface, sessions);
			RefreshWpfPlotAvgPacketLossByInterface(WpfPlotAvgPacketLossByInterface, sessions);
			RefreshWpfPlotAvgDelayByInterface(WpfPlotAvgDelayByInterface, sessions);
			RefreshWpfPlotAvgJitterByInterface(WpfPlotAvgJitterByInterface, sessions);
		}



		private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ViewModelCollection<SessionViewModel>? sessions;

			sessions = e.OldValue as ViewModelCollection<SessionViewModel>;
			if (sessions != null) sessions.CollectionChanged -= Sessions_CollectionChanged;
			sessions = e.NewValue as ViewModelCollection<SessionViewModel>;
			if (sessions == null) return;
			
			sessions.CollectionChanged += Sessions_CollectionChanged;
			RefreshCharts();


		}

		private void Sessions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RefreshCharts();
		}



	}
}
