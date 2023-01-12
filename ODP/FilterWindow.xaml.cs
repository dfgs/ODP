using ODP.ViewModels;
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
using System.Windows.Shapes;

namespace ODP
{
	/// <summary>
	/// Logique d'interaction pour FilterWindow.xaml
	/// </summary>
	public partial class FilterWindow : Window
	{


		public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(FilterViewModel), typeof(FilterWindow), new PropertyMetadata(null));
		public FilterViewModel	Filter
		{
			get { return (FilterViewModel)GetValue(FilterProperty); }
			set { SetValue(FilterProperty, value); }
		}




		public FilterWindow()
		{
			InitializeComponent();
		}

		private void ShowError(Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error");
		}
		private void OKCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = !string.IsNullOrEmpty(Filter?.Value);
		}

		private void OKCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = Filter != null;
			Close();
		}
		private void CancelCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = true;
		}

		private void CancelCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
