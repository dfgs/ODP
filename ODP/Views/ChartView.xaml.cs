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

namespace ODP.Views
{
	/// <summary>
	/// Logique d'interaction pour ChartView.xaml
	/// </summary>
	public partial class ChartView : UserControl
	{


		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ChartView), new PropertyMetadata("Title"));
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}



		public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register("IsMaximized", typeof(bool), typeof(ChartView), new PropertyMetadata(true));
		public bool IsMaximized
		{
			get { return (bool)GetValue(IsMaximizedProperty); }
			set { SetValue(IsMaximizedProperty, value); }
		}




		public ChartView()
		{
			InitializeComponent();
			WpfPlot.Plot.Title(null);
		}
	}
}
