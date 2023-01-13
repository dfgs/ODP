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



		public static readonly DependencyProperty QualityFiltersProperty = DependencyProperty.Register("QualityFilters", typeof(FilterViewModelCollection<QualityFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<QualityFilterViewModel> QualityFilters
		{
			get { return (FilterViewModelCollection<QualityFilterViewModel>)GetValue(QualityFiltersProperty); }
			set { SetValue(QualityFiltersProperty, value); }
		}
		public static readonly DependencyProperty IPGroupFiltersProperty = DependencyProperty.Register("IPGroupFilters", typeof(FilterViewModelCollection<IPGroupFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<IPGroupFilterViewModel> IPGroupFilters
		{
			get { return (FilterViewModelCollection<IPGroupFilterViewModel>)GetValue(IPGroupFiltersProperty); }
			set { SetValue(IPGroupFiltersProperty, value); }
		}
		public static readonly DependencyProperty SIPInterfaceFiltersProperty = DependencyProperty.Register("SIPInterfaceFilters", typeof(FilterViewModelCollection<SIPInterfaceFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<SIPInterfaceFilterViewModel> SIPInterfaceFilters
		{
			get { return (FilterViewModelCollection<SIPInterfaceFilterViewModel>)GetValue(SIPInterfaceFiltersProperty); }
			set { SetValue(SIPInterfaceFiltersProperty, value); }
		}









		public GlobalFilterViewModel(ILogger Logger) : base(Logger)
		{
			QualityFilters = new FilterViewModelCollection<QualityFilterViewModel>(Logger);
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Bad, Name = "Bad quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Average, Name = "Average quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Good, Name = "Good quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.NA, Name = "Not applicable" });

			IPGroupFilters = new FilterViewModelCollection<IPGroupFilterViewModel>(Logger);
			SIPInterfaceFilters = new FilterViewModelCollection<SIPInterfaceFilterViewModel>(Logger);
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
