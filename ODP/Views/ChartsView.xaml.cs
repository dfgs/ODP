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

		public ChartsView()
		{
			InitializeComponent();
			
		}
		

		private void RefreshWpfPlotMediaReportCountByQuality(WpfPlot WpfPlot, ViewModelCollection<SessionViewModel> Sessions)
		{
			var mediaReports = Sessions.SelectMany(session => session.Calls).SelectMany(call => call.MediaReports);
			double[] values = mediaReports.GroupByQuality(Qualities).Select(mediaReports => (double)mediaReports.Count()).ToArray();
			System.Drawing.Color[] sliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green };


			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Number of media reports by quality");

			var pie = WpfPlot.Plot.AddPie(values);
			pie.DonutSize = .6;
			pie.Explode = true;
			pie.ShowValues = true;
			pie.SliceLabels = QualityLabels;
			pie.SliceFillColors = sliceColors;

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
			var calls = Sessions.SelectMany(session => session.Calls);
			string[] sipInterfaces = calls.Select(call => call.SIPInterfaceId).Where(item=> item!= null).Cast<string>().Distinct().ToArray();
			double[][] values = calls.GroupByQuality(Qualities)
				.Select(groupedCalls => groupedCalls.GroupByInterface(sipInterfaces).Select(calls => (double)calls.Count()).ToArray()
				).ToArray();

			System.Drawing.Color[] sliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green };


			WpfPlot.Plot.Clear();
			WpfPlot.Plot.Title("Number of calls by interface");

			// add the grouped bar plots and show a legend
			var bars=WpfPlot.Plot.AddBarGroups(sipInterfaces, QualityLabels, values, null);
			for(int t=0;t<bars.Length;t++)
			{
				bars[t].FillColor= sliceColors[t];
				bars[t].ShowValuesAboveBars = true;
			}

			WpfPlot.Plot.Legend(location: Alignment.UpperRight);
			// adjust axis limits so there is no padding below the bar graph
			WpfPlot.Plot.SetAxisLimits(yMin: 0);
						
			WpfPlot.Refresh();
		}

		private void RefreshCharts()
		{
			ViewModelCollection<SessionViewModel>? sessions;

			sessions = DataContext as ViewModelCollection<SessionViewModel>;
			if (sessions == null) return;

			RefreshWpfPlotMediaReportCountByQuality(WpfPlotMediaReportCountByQuality, sessions);
			RefreshWpfPlotCallsCountByInterface(WpfPlotCallsCountByInterface, sessions);
		}
		
		

		private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ViewModelCollection<SessionViewModel>? sessions;

			sessions = e.OldValue as ViewModelCollection<SessionViewModel>;
			if (sessions != null) sessions.CollectionChanged -= Sessions_CollectionChanged;
			sessions = e.NewValue as ViewModelCollection<SessionViewModel>;
			if (sessions != null) sessions.CollectionChanged += Sessions_CollectionChanged;


		}

		private void Sessions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RefreshCharts();
		}



	}
}
