﻿using LogLib;
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


		public static readonly DependencyProperty ProjectsProperty = DependencyProperty.Register("Projects", typeof(ProjectViewModelCollection), typeof(ApplicationViewModel), new PropertyMetadata(null));
		public ProjectViewModelCollection Projects
		{
			get { return (ProjectViewModelCollection)GetValue(ProjectsProperty); }
			set { SetValue(ProjectsProperty, value); }
		}




		public ApplicationViewModel(ILogger Logger) : base(Logger)
		{
			Projects = new ProjectViewModelCollection(Logger);
		}
		public void AddNewProject()
		{
			Projects.AddNew();
		}

		public void CloseCurrentProject()
		{
			Projects.CloseCurrent();
		}

		public async Task OpenProjectAsync(string Path)
		{

			await Projects.AddAsync(Path);

			

		}

		
	}
}
