using LogLib;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ODP
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ILogger logger;
		private ApplicationViewModel applicationViewModel;

		public MainWindow()
		{
			logger = NullLogger.Instance;
			applicationViewModel = new ApplicationViewModel(logger);

			InitializeComponent();

			DataContext = applicationViewModel;
		}
		private void ShowError(Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error");
		}

		private void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = true;
		}

		private async void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				await applicationViewModel.AddNewProjectAsync();
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private void AddFileCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = applicationViewModel.Projects.SelectedItem!=null;
		}

		private async void AddFileCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;

			dialog = new OpenFileDialog();
			dialog.Title = "Open syslog file";
			dialog.DefaultExt = "txt";
			dialog.Filter = "Text files|*.txt|All files|*.*";

			if (dialog.ShowDialog(this)??false)
			{
				try
				{
					await applicationViewModel.Projects.SelectedItem.AddFileAsync(dialog.FileName);
				}
				catch (Exception ex)
				{
					ShowError(ex);
				}
			}


		}



	}
}
