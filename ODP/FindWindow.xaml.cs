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
		public static readonly List<SearchCriteriaViewModel> SearchCriterias;




		public static readonly DependencyProperty SearchCriteriaProperty = DependencyProperty.Register("SearchCriteria", typeof(SearchCriteriaViewModel), typeof(FindWindow), new PropertyMetadata(null));
		public SearchCriteriaViewModel SearchCriteria
		{
			get { return (SearchCriteriaViewModel)GetValue(SearchCriteriaProperty); }
			set { SetValue(SearchCriteriaProperty, value); }
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

		static FindWindow()
		{
			SearchCriterias = new List<SearchCriteriaViewModel>();
			SearchCriterias.Add(new SearchCriteriaViewModel() { Value = ViewModels.SearchCriteria.SessionID, Name = "Session ID" });
			SearchCriterias.Add(new SearchCriteriaViewModel() { Value = ViewModels.SearchCriteria.CallID, Name = "Call ID" });
			SearchCriterias.Add(new SearchCriteriaViewModel() { Value = ViewModels.SearchCriteria.SrcURI, Name = "Source URI" });
			SearchCriterias.Add(new SearchCriteriaViewModel() { Value = ViewModels.SearchCriteria.DstURI, Name = "Destination URI" });
			SearchCriterias.Add(new SearchCriteriaViewModel() { Value = ViewModels.SearchCriteria.Quality, Name = "Quality" });
		}

		public FindWindow()
		{
			SearchCriteria = SearchCriterias.First();
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
			e.Handled = true; e.CanExecute =(ApplicationViewModel?.Projects.SelectedItem!=null) && (SearchCriteria!=null) && (!string.IsNullOrEmpty(SearchValue));
		}

		private void FindPreviousCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (ApplicationViewModel?.Projects.SelectedItem == null) return;
			try
			{
				ApplicationViewModel.Projects.SelectedItem.FindPrevious(SearchCriteria.Value, SearchValue);
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}
		private void FindNextCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (ApplicationViewModel?.Projects.SelectedItem != null) && (SearchCriteria != null) && (!string.IsNullOrEmpty(SearchValue));
		}

		private void FindNextCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (ApplicationViewModel?.Projects.SelectedItem == null) return;
			try
			{ 
				ApplicationViewModel.Projects.SelectedItem.FindNext(SearchCriteria.Value, SearchValue);
			}
			catch(Exception ex)
			{
				ShowError(ex);
			}
		}





	}
}
