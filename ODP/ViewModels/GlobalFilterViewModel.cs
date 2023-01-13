using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class GlobalFilterViewModel : BaseViewModel
	{



		public static readonly DependencyProperty QualityFiltersProperty = DependencyProperty.Register("QualityFilters", typeof(ViewModelCollection<QualityFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public ViewModelCollection<QualityFilterViewModel> QualityFilters
		{
			get { return (ViewModelCollection<QualityFilterViewModel>)GetValue(QualityFiltersProperty); }
			set { SetValue(QualityFiltersProperty, value); }
		}
		public static readonly DependencyProperty IPGroupFiltersProperty = DependencyProperty.Register("IPGroupFilters", typeof(IPGroupFiltersViewModel), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public IPGroupFiltersViewModel IPGroupFilters
		{
			get { return (IPGroupFiltersViewModel)GetValue(IPGroupFiltersProperty); }
			set { SetValue(IPGroupFiltersProperty, value); }
		}
		public static readonly DependencyProperty SIPInterfaceFiltersProperty = DependencyProperty.Register("SIPInterfaceFilters", typeof(SIPInterfaceFiltersViewModel), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public SIPInterfaceFiltersViewModel SIPInterfaceFilters
		{
			get { return (SIPInterfaceFiltersViewModel)GetValue(SIPInterfaceFiltersProperty); }
			set { SetValue(SIPInterfaceFiltersProperty, value); }
		}




		public GlobalFilterViewModel(ILogger Logger) : base(Logger)
		{
			QualityFilters = new ViewModelCollection<QualityFilterViewModel>(Logger);
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Bad, Name = "Bad quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Average, Name = "Average quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Good, Name = "Good quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.NA, Name = "Not applicable" });

			IPGroupFilters = new IPGroupFiltersViewModel(Logger);
			SIPInterfaceFilters = new SIPInterfaceFiltersViewModel(Logger);
		}

		public bool Match(SessionViewModel Session)
		{
			if (!QualityFilters.Where(filter => filter.IsSelected).Select(filter => filter.Quality).Contains(Session.Quality)) return false;
			if (!IPGroupFilters.Where(filter => filter.IsSelected).Join(Session.Calls, filter => filter.Name, call => call.IPGroup, (ipGroup, call) => call).Any()) return false;
			if (!SIPInterfaceFilters.Where(filter => filter.IsSelected).Join(Session.Calls, filter => filter.Name, call => call.SIPInterfaceId, (SIPInterface, call) => call).Any()) return false;

			return true;
		}



	}
}
