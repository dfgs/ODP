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
			logger = new FileLogger(new DefaultLogFormatter(),"Log.txt");
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
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = applicationViewModel.Projects.SelectedItem != null;
		}

		private async void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				await applicationViewModel.CloseCurrentProjectAsync();
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

			if (applicationViewModel.Projects.SelectedItem == null) return;

			dialog = new OpenFileDialog();
			dialog.Title = "Open syslog file";
			dialog.DefaultExt = "txt";
			dialog.Filter = "Text files|*.txt|All files|*.*";
			dialog.Multiselect = true;

			if (dialog.ShowDialog(this)??false)
			{
				try
				{
					foreach (string fileName in dialog.FileNames)
					{
						await applicationViewModel.Projects.SelectedItem.AddFileAsync(fileName);
					}
				}
				catch (Exception ex)
				{
					ShowError(ex);
				}

				
			}


		}
		private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = applicationViewModel.Projects.SelectedItem != null;
		}

		private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			FindWindow dialog;

			dialog = new FindWindow();
			dialog.Owner = this;
			dialog.ApplicationViewModel= applicationViewModel;
			dialog.Show();
		}

		private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = applicationViewModel.Projects.SelectedItem != null;
		}

		private async void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (applicationViewModel.Projects.SelectedItem == null) return;
			if (applicationViewModel.Projects.SelectedItem.Path == null) await SaveProjectAsAsync();
			else await SaveProjectAsync();
		}
		private void SaveAsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = applicationViewModel.Projects.SelectedItem != null;
		}

		private async void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (applicationViewModel.Projects.SelectedItem == null) return;
			await SaveProjectAsync();
		}



		private async Task SaveProjectAsync()
		{
			if (applicationViewModel.Projects.SelectedItem == null) return;
			try
			{
				await applicationViewModel.Projects.SelectedItem.SaveAsync(applicationViewModel.Projects.SelectedItem.Path);
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}
		private async Task SaveProjectAsAsync()
		{
			SaveFileDialog dialog;

			if (applicationViewModel.Projects.SelectedItem == null) return;

			dialog = new SaveFileDialog();
			dialog.Title = "Save project as";
			dialog.DefaultExt = "xml";
			dialog.Filter = "xml files|*.xml|All files|*.*";

			if (dialog.ShowDialog(this) ?? false)
			{
				try
				{
					await applicationViewModel.Projects.SelectedItem.SaveAsync(dialog.FileName);
				}
				catch (Exception ex)
				{
					ShowError(ex);
				}
			}

		}


		private void OpenFileCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = true;
		}

		private async void OpenFileCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;

			dialog = new OpenFileDialog();
			dialog.Title = "Open xml file";
			dialog.DefaultExt = "xml";
			dialog.Filter = "xml files|*.xml|All files|*.*";
			dialog.Multiselect = false;

			if (dialog.ShowDialog(this) ?? false)
			{
				try
				{
					await applicationViewModel.OpenProjectAsync(dialog.FileName);
				}
				catch (Exception ex)
				{
					ShowError(ex);
				}


			}


		}





	}
}
