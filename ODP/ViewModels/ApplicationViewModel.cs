using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class ApplicationViewModel : ViewModel<string>
	{


		public static readonly DependencyProperty ProjectsProperty = DependencyProperty.Register("Projects", typeof(ViewModelCollection<ProjectViewModel>), typeof(ApplicationViewModel), new PropertyMetadata(null));
		public ViewModelCollection<ProjectViewModel> Projects
		{
			get { return (ViewModelCollection<ProjectViewModel>)GetValue(ProjectsProperty); }
			set { SetValue(ProjectsProperty, value); }
		}




		public ApplicationViewModel(ILogger Logger) : base(Logger)
		{
			Projects = new ViewModelCollection<ProjectViewModel>(Logger);
		}
		public async Task AddNewProjectAsync()
		{
			Project project;
			ProjectViewModel projectViewModel;

			project = new Project();

			projectViewModel = new ProjectViewModel(Logger);
			await projectViewModel.LoadAsync(project);

			Projects.Add(projectViewModel);
			Projects.SelectedItem = projectViewModel;
		}

		public async Task CloseCurrentProjectAsync()
		{
			if (Projects.SelectedItem == null) return;

			Projects.Remove(Projects.SelectedItem);
			Projects.SelectedItem = Projects.FirstOrDefault();

			await Task.Yield();
		}

		public async Task OpenProjectAsync(string Path)
		{
			Project project;
			ProjectViewModel projectViewModel;

			project = await TryAsync(() => Project.LoadAsync(Path)).OrThrow("Failed to open project");
			
			projectViewModel = new ProjectViewModel(Logger);
			await projectViewModel.LoadAsync(project);

			Projects.Add(projectViewModel);
			Projects.SelectedItem = projectViewModel;
		}

		
	}
}
