using LogLib;
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
using ViewModelLib;

namespace ODP
{
	/// <summary>
	/// Logique d'interaction pour FindWindow.xaml
	/// </summary>
	public partial class FindWindow : Window
	{



		public static readonly DependencyProperty SessionFoundProperty = DependencyProperty.Register("SessionFound", typeof(bool), typeof(FindWindow), new PropertyMetadata(true));
		public bool SessionFound
		{
			get { return (bool)GetValue(SessionFoundProperty); }
			set { SetValue(SessionFoundProperty, value); }
		}





		public static readonly DependencyProperty MatchPropertyProperty = DependencyProperty.Register("MatchProperty", typeof(MatchPropertyViewModel), typeof(FindWindow), new PropertyMetadata(null));
		public MatchPropertyViewModel MatchProperty
		{
			get { return (MatchPropertyViewModel)GetValue(MatchPropertyProperty); }
			set { SetValue(MatchPropertyProperty, value); }
		}



		public static readonly DependencyProperty SearchValueProperty = DependencyProperty.Register("SearchValue", typeof(string), typeof(FindWindow), new PropertyMetadata(null));
		public string SearchValue
		{
			get { return (string)GetValue(SearchValueProperty); }
			set { SetValue(SearchValueProperty, value); }
		}


		public ApplicationViewModel? ApplicationViewModel
		{ 
			get; 
			set; 
		}

		
		public FindWindow()
		{
			MatchProperty = Consts.MatchProperties.First();
			InitializeComponent();
		}
		private void ShowError(Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error");
		}
		private void OKCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled= true;e.CanExecute= true;
        }

		private void OKCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
        }

		private void FindPreviousCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute =(ApplicationViewModel?.Projects.SelectedItem!=null) && (MatchProperty!=null) && (!string.IsNullOrEmpty(SearchValue));
		}

		private void FindPreviousCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (ApplicationViewModel?.Projects.SelectedItem == null) return;
			try
			{
				SessionFound=ApplicationViewModel.Projects.SelectedItem.FindPrevious(MatchProperty.Value, SearchValue);
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}
		private void FindNextCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (ApplicationViewModel?.Projects.SelectedItem != null) && (MatchProperty != null) && (!string.IsNullOrEmpty(SearchValue));
		}

		private void FindNextCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (ApplicationViewModel?.Projects.SelectedItem == null) return;
			try
			{
				SessionFound = ApplicationViewModel.Projects.SelectedItem.FindNext(MatchProperty.Value, SearchValue);
			}
			catch(Exception ex)
			{
				ShowError(ex);
			}
		}





	}
}
