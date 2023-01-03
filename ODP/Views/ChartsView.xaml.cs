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
		public ChartsView()
		{
			InitializeComponent();
			
		}

		private void RefreshWpfPlotMediaReportCountByQuality(ViewModelCollection<SessionViewModel> Sessions)
		{
			var mediaReports = sessions.SelectMany(session => session.Calls).SelectMany(call => call.MediaReports);
			double[] values = Enum.GetValues<Quality>().GroupJoin(mediaReports, quality => quality, mediaReport => mediaReport.Quality, (quality, mediaReports) => mediaReports)
				.Select(mediaReports => (double)mediaReports.Count()).ToArray();
			string[] labels = new string[] { "Bad quality", "Average quality", "Good quality" };
			System.Drawing.Color[] sliceColors = { System.Drawing.Color.Red, System.Drawing.Color.Orange, System.Drawing.Color.Green };


			WpfPlotMediaReportCountByQuality.Plot.Clear();
			WpfPlotMediaReportCountByQuality.Plot.Title("Number of media reports by quality");

			var pie = WpfPlotMediaReportCountByQuality.Plot.AddPie(values);
			pie.DonutSize = .6;
			pie.Explode = true;
			pie.ShowValues = true;
			pie.SliceLabels = labels;
			pie.SliceFillColors = sliceColors;

			WpfPlotMediaReportCountByQuality.Plot.Legend();
			try
			{

				WpfPlotMediaReportCountByQuality.Refresh();
			}
			catch (ArgumentException)
			{
				// bug in graphics.drawPie
			}
		}
		private void RefreshCharts()
		{
			ViewModelCollection<SessionViewModel>? sessions;

			sessions = DataContext as ViewModelCollection<SessionViewModel>;
			if (sessions == null) return;

			RefreshWpfPlotMediaReportCountByQuality(sessions);
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
