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
	public class ProjectViewModelCollection : GenericViewModelList<Project, ProjectViewModel>
	{
		public ProjectViewModelCollection(IList<Project> Source) : base(Source, (SourceItem) => new ProjectViewModel(SourceItem))
		{
		}

		public void AddNew()
		{
			Project project;
			ProjectViewModel projectViewModel;

			project = new Project();

			projectViewModel = new ProjectViewModel(project);

			AddInternal(projectViewModel);
			SelectedItem = projectViewModel;
		}
		public async Task AddAsync(string Path)
		{
			ProjectViewModel projectViewModel;

			projectViewModel = await ProjectViewModel.LoadAsync(Path);
			AddInternal(projectViewModel);
			SelectedItem = projectViewModel;

		}


		public void CloseCurrent()
		{
			if (SelectedItem == null) return;

			RemoveInternal(SelectedItem);
			SelectedItem = this.FirstOrDefault();
		}

		

	}
}
