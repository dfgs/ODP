﻿using LogLib;
using Microsoft.Win32;
using NAudio.Wave;
using ODP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using static System.Windows.Forms.DataFormats;

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
			BaseViewModel.Logger = logger;
			applicationViewModel = new ApplicationViewModel();

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

		private void RunCommand(Action Action)
		{
			try
			{
				Action();
				//await Task.Delay(10000);
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute =  (!TaskIsRunning);
		}

		private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			RunCommand(applicationViewModel.AddNewProject);
		}
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			RunCommand(applicationViewModel.CloseCurrentProject);
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


		private void AddWiresharkFileCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private async void AddWiresharkFileCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;
			IProgress<long> fileProgress;

			if (applicationViewModel.Projects.SelectedItem == null) return;

			dialog = new OpenFileDialog();
			dialog.Title = "Open debug recording";
			dialog.DefaultExt = "pcap";
			dialog.Filter = "pcap files|*.pcap|All files|*.*";
			dialog.Multiselect = true;

			if (dialog.ShowDialog(this) ?? false)
			{
				fileProgress = new Progress<long>((percent) => progressBar.Value = percent);
				await RunCommandAsync(applicationViewModel.Projects.SelectedItem.AddDebugRecordingAsync(dialog.FileNames, fileProgress));
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



		private void ApplyFilterCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (applicationViewModel.Projects.SelectedItem != null) && (!TaskIsRunning);
		}

		private void ApplyFilterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{

			if (applicationViewModel.Projects.SelectedItem == null) return;

			RunCommand(applicationViewModel.Projects.SelectedItem.RefreshSessions);
		}

		private void OpenCallChartsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = (!TaskIsRunning);
		}

		private void OpenCallChartsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CallChartWindow window;

			window = new CallChartWindow();
			window.Owner = this;
			window.DataContext = e.Parameter;
			window.ShowDialog();


		}
 
       


 

        private void TabControl_DragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;
			else e.Effects = DragDropEffects.None;
        }

		private async void TabControl_Drop(object sender, DragEventArgs e)
		{
			Progress<long> fileProgress;

			e.Handled = true;
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


			if (applicationViewModel.Projects.SelectedItem == null) RunCommand(applicationViewModel.AddNewProject);
			if (applicationViewModel.Projects.SelectedItem == null) return;

			fileProgress = new Progress<long>((percent) => progressBar.Value = percent);
			await RunCommandAsync(applicationViewModel.Projects.SelectedItem.AddFilesAsync(files, fileProgress));

		}

	}
}
