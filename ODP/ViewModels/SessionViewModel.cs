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
	public class SessionViewModel : ViewModel<Session>
	{
		public string? SessionID
		{
			get => Model?.SessionId;
		}

		public static readonly DependencyProperty CallsProperty = DependencyProperty.Register("Calls", typeof(ViewModelCollection<CallViewModel>), typeof(SessionViewModel), new PropertyMetadata(null));
		public ViewModelCollection<CallViewModel> Calls
		{
			get { return (ViewModelCollection<CallViewModel>)GetValue(CallsProperty); }
			set { SetValue(CallsProperty, value); }
		}



		public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register("StartTime", typeof(string), typeof(SessionViewModel), new PropertyMetadata(null));
		public string? StartTime
		{
			get { return (string?)GetValue(StartTimeProperty); }
			set { SetValue(StartTimeProperty, value); }
		}

		public static readonly DependencyProperty StopTimeProperty = DependencyProperty.Register("StopTime", typeof(string), typeof(SessionViewModel), new PropertyMetadata(null));
		public string? StopTime
		{
			get { return (string?)GetValue(StopTimeProperty); }
			set { SetValue(StopTimeProperty, value); }
		}


		public SessionViewModel(ILogger Logger) : base(Logger)
		{
			Calls = new ViewModelCollection<CallViewModel>(Logger);
		}

		protected override async Task OnLoadedAsync()
		{
			if (Model==null)
			{
				Calls.Clear();
				StartTime= null; 
				StopTime= null;
				return;
			}

			await Calls.LoadAsync(await Model.Calls.ToViewModelsAsync(() => new CallViewModel(Logger)));

			StartTime = Calls.SelectMany(item => item.SBCReports.Select(item => item.SetupTime)).Min();
			StopTime = Calls.SelectMany(item => item.SBCReports.Select(item => item.ReleaseTime)).Max();

		}


	}
}
