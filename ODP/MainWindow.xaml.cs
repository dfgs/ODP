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
using ViewModelLib;

namespace ODP
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ILogger logger;
		private ApplicationViewModel applicationViewModel;



		public static readonly DependencyProperty TaskIsRunningProperty = DependencyProperty.Register("TaskIsRunning", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
		public bool TaskIsRunning
		{
			get { return (bool)GetValue(TaskIsRunningProperty); }
			set { SetValue(TaskIsRunningProperty, value); }
		}





		public MainWindow()
		{
			string appData;

			appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			System.IO.Directory.CreateDirectory(System.IO.Path.Combine(appData, "ODP"));
			logger = new FileLogger(new DefaultLogFormatter(), System.IO.Path.Combine(appData, "ODP","log.txt"));
			applicationViewModel = new ApplicationViewModel(logger);

			InitializeComponent();

			DataContext = applicationViewModel;
		}
		private void ShowError(Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error");
		}

		private async Task RunCommandAsync(Task Task)
		{
			TaskIsRunning= true;
			try
			{
				await Task;
				//await Task.Delay(10000);
			}
			catch(Exception ex)
			{
				ShowError(ex);
			}
			finally { TaskIsRunning = false; }
		}
		

		private void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute =  (!TaskIsRunning);
		}

		private async void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await RunCommandAsync(applicationViewModel.AddNewProjectAsync());
		}
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private async void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await RunCommandAsync(applicationViewModel.CloseCurrentProjectAsync());
		}
		private void AddFileCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem!=null) && (!TaskIsRunning);
		}

		private async void AddFileCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;
			IProgress<long> fileProgress;

			if (applicationViewModel.Projects.SelectedItem == null) return;

			dialog = new OpenFileDialog();
			dialog.Title = "Open syslog file";
			dialog.DefaultExt = "txt";
			dialog.Filter = "Text files|*.txt|All files|*.*";
			dialog.Multiselect = true;

			if (dialog.ShowDialog(this)??false)
			{
				fileProgress = new Progress<long>((percent) => progressBar.Value = percent);
				await RunCommandAsync( applicationViewModel.Projects.SelectedItem.AddFilesAsync(dialog.FileNames, fileProgress));
			}


		}
		private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			FindWindow dialog;

			dialog = new FindWindow();
			dialog.Owner = this;
			dialog.ApplicationViewModel= applicationViewModel;

			dialog.ShowDialog();
		}

		private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private async void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (applicationViewModel.Projects.SelectedItem == null) return;
			if (applicationViewModel.Projects.SelectedItem.Path == null) await SaveProjectAsAsync();
			else await SaveProjectAsync();
		}
		private void SaveAsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private async void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (applicationViewModel.Projects.SelectedItem == null) return;
			await SaveProjectAsAsync();
		}



		private async Task SaveProjectAsync()
		{
			if (applicationViewModel.Projects.SelectedItem == null) return;
			await RunCommandAsync( applicationViewModel.Projects.SelectedItem.SaveAsync(applicationViewModel.Projects.SelectedItem.Path));
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
				await RunCommandAsync( applicationViewModel.Projects.SelectedItem.SaveAsync(dialog.FileName));
			}

		}


		private void OpenFileCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (!TaskIsRunning);
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
				await RunCommandAsync( applicationViewModel.OpenProjectAsync(dialog.FileName));
			}


		}

		private void HelpCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (!TaskIsRunning);
		}

		private void HelpCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			HelpWindow window;

			window = new HelpWindow();
			window.Owner = this;
			window.ShowDialog();
		}


		private void AddFilterCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private void AddFilterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			FilterWindow dialog;
			FilterViewModel? filter;

			if (applicationViewModel.Projects.SelectedItem == null) return;

			filter = new FilterViewModel() { MatchProperty = Consts.MatchProperties.First(), MatchOperator = Consts.MatchOperators.First() };

			dialog = new FilterWindow();
			dialog.Owner = this;
			dialog.Filter = filter;

			if (dialog.ShowDialog()??false)
			{
				applicationViewModel.Projects.SelectedItem.Filters.Add(filter);
			}
		}

		private void EditFilterCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects?.SelectedItem?.Filters?.SelectedItem != null) && (!TaskIsRunning);
		}

		private void EditFilterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			FilterWindow dialog;
			FilterViewModel? selectedFilter,filter;

			selectedFilter = applicationViewModel.Projects.SelectedItem?.Filters.SelectedItem;
			if (selectedFilter == null) return;

			filter = new FilterViewModel() { MatchProperty =selectedFilter.MatchProperty,MatchOperator=selectedFilter.MatchOperator,Value=selectedFilter.Value };

			dialog = new FilterWindow();
			dialog.Owner = this;
			dialog.Filter = filter;

			if (dialog.ShowDialog() ?? false)
			{
				selectedFilter.MatchProperty = filter.MatchProperty;
				selectedFilter.MatchOperator = filter.MatchOperator;
				selectedFilter.Value= filter.Value;
			}
		}
		private void DeleteFilterCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects?.SelectedItem?.Filters?.SelectedItem != null) && (!TaskIsRunning);
		}

		private void DeleteFilterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			FilterViewModel? selectedFilter;

			if (applicationViewModel.Projects.SelectedItem==null) return;
			
			selectedFilter = applicationViewModel.Projects.SelectedItem.Filters.SelectedItem;
			if (selectedFilter == null) return;

			applicationViewModel.Projects.SelectedItem.Filters.Remove(selectedFilter);

		}

	}
}
