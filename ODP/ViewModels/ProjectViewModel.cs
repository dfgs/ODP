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
	public class ProjectViewModel:ViewModel<Project>
	{

		public string? Name
		{
			get { return Model?.Name; }
			set { if(Model!=null) Model.Name = value; }
		}

		public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(ProjectViewModel), new PropertyMetadata(null));



		public static readonly DependencyProperty CDRsProperty = DependencyProperty.Register("CDRs", typeof(ViewModelCollection<string>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<string> CDRs
		{
			get { return (ViewModelCollection<string>)GetValue(CDRsProperty); }
			set { SetValue(CDRsProperty, value); }
		}




		public ProjectViewModel(ILogger Logger) : base(Logger)
		{
			CDRs = new ViewModelCollection<string>(Logger);
		}

		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}

		public async Task AddFileAsync(string FileName)
		{
			if (Model == null) return;

			await TryAsync(() => Model.AddFileAsync(FileName)).OrThrow($"Failed to read syslog file {FileName}");
			await CDRs.LoadAsync(Model.CDRs);
			
		}


	}
}
