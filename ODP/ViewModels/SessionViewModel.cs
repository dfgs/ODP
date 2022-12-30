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
	public class SessionViewModel : ViewModel<Session>
	{
		public static readonly DependencyProperty CallsProperty = DependencyProperty.Register("Calls", typeof(ViewModelCollection<CallViewModel>), typeof(SessionViewModel), new PropertyMetadata(null));
		public ViewModelCollection<CallViewModel> Calls
		{
			get { return (ViewModelCollection<CallViewModel>)GetValue(CallsProperty); }
			set { SetValue(CallsProperty, value); }
		}


		public string? SessionID
		{
			get => Model?.SessionId;
		}
		
		public DateTime? StartTime
		{
			get => Calls.SelectMany(item => item.SBCReports.Select(item => item.SetupTime)).Min();
		}

		public DateTime? StopTime
		{
			get => Calls.SelectMany(item => item.SBCReports.Select(item => item.ReleaseTime)).Max();
	}
		public string? SrcURI
		{
			get => Calls.FirstOrDefault(item => item.SrcURI != null)?.SrcURI;
		}
		public string? DstURI
		{
			get => Calls.FirstOrDefault(item => item.DstURI != null)?.DstURI;
		}


		public Quality? Quality
		{
			get => Calls.Select(item => item.Quality).Min();
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
				return;
			}

			await Calls.LoadAsync(await Model.Calls.ToViewModelsAsync(() => new CallViewModel(Logger)));

		}


	}
}
