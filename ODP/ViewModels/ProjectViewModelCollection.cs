using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class ProjectViewModelCollection : ViewModelCollection<Project, ProjectViewModel>
	{
		public ProjectViewModelCollection(ILogger Logger) : base(Logger)
		{
		}

		public void AddNew()
		{
			Project project;
			ProjectViewModel projectViewModel;

			project = new Project();

			projectViewModel = new ProjectViewModel(Logger);
			projectViewModel.Load(project);

			AddInternal(projectViewModel);
			SelectedItem = projectViewModel;
		}
		public async Task AddAsync(string Path)
		{
			ProjectViewModel projectViewModel;

			projectViewModel = new ProjectViewModel(Logger);
			AddInternal(projectViewModel);
			SelectedItem = projectViewModel;

			await projectViewModel.LoadAsync(Path);
		}


		public void CloseCurrent()
		{
			if (SelectedItem == null) return;

			RemoveInternal(SelectedItem);
			SelectedItem = this.FirstOrDefault();
		}

		
	}
}
