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
		public static readonly DependencyProperty TermReasonFiltersProperty = DependencyProperty.Register("TermReasonFilters", typeof(FilterViewModelCollection<TermReasonFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<TermReasonFilterViewModel> TermReasonFilters
		{
			get { return (FilterViewModelCollection<TermReasonFilterViewModel>)GetValue(TermReasonFiltersProperty); }
			set { SetValue(TermReasonFiltersProperty, value); }
		}


		public static readonly DependencyProperty DelayFiltersProperty = DependencyProperty.Register("DelayFilters", typeof(FilterViewModelCollection<DelayFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<DelayFilterViewModel> DelayFilters
		{
			get { return (FilterViewModelCollection<DelayFilterViewModel>)GetValue(DelayFiltersProperty); }
			set { SetValue(DelayFiltersProperty, value); }
		}

		public static readonly DependencyProperty JitterFiltersProperty = DependencyProperty.Register("JitterFilters", typeof(FilterViewModelCollection<JitterFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<JitterFilterViewModel> JitterFilters
		{
			get { return (FilterViewModelCollection<JitterFilterViewModel>)GetValue(JitterFiltersProperty); }
			set { SetValue(JitterFiltersProperty, value); }
		}


		public static readonly DependencyProperty PacketLossFiltersProperty = DependencyProperty.Register("PacketLossFilters", typeof(FilterViewModelCollection<PacketLossFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<PacketLossFilterViewModel> PacketLossFilters
		{
			get { return (FilterViewModelCollection<PacketLossFilterViewModel>)GetValue(PacketLossFiltersProperty); }
			set { SetValue(PacketLossFiltersProperty, value); }
		}




		public GlobalFilterViewModel(ILogger Logger) : base(Logger)
		{
			QualityFilters = new FilterViewModelCollection<QualityFilterViewModel>(Logger);
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Bad, Name = "Bad quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Average, Name = "Average quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.Good, Name = "Good quality" });
			QualityFilters.Add(new QualityFilterViewModel(Logger) { Quality = Quality.NA, Name = "Not applicable" });

			DelayFilters = new FilterViewModelCollection<DelayFilterViewModel>(Logger);
			DelayFilters.Add(new DelayFilterViewModel(Logger) { MinValue = int.MinValue, MaxValue = int.MaxValue, Name = "Any delay", AnyValue = true });
			DelayFilters.Add(new DelayFilterViewModel(Logger) { MinValue = 150, MaxValue = int.MaxValue, Name = "Above 150 ms", IsSelected = false });
			DelayFilters.Add(new DelayFilterViewModel(Logger) { MinValue = 250, MaxValue = int.MaxValue, Name = "Above 250 ms", IsSelected = false });
			DelayFilters.Add(new DelayFilterViewModel(Logger) { MinValue = 350, MaxValue = int.MaxValue, Name = "Above 350 ms", IsSelected = false });
			DelayFilters.SelectedItem = DelayFilters.LastOrDefault();

			JitterFilters = new FilterViewModelCollection<JitterFilterViewModel>(Logger);
			JitterFilters.Add(new JitterFilterViewModel(Logger) { MinValue = int.MinValue, MaxValue = int.MaxValue, Name = "Any jitter", AnyValue = true });
			JitterFilters.Add(new JitterFilterViewModel(Logger) { MinValue = 20, MaxValue = int.MaxValue, Name = "Above 20 ms", IsSelected = false });
			JitterFilters.Add(new JitterFilterViewModel(Logger) { MinValue = 30, MaxValue = int.MaxValue, Name = "Above 30 ms", IsSelected = false });
			JitterFilters.Add(new JitterFilterViewModel(Logger) { MinValue = 50, MaxValue = int.MaxValue, Name = "Above 50 ms", IsSelected = false });
			JitterFilters.SelectedItem = JitterFilters.LastOrDefault();

			PacketLossFilters = new FilterViewModelCollection<PacketLossFilterViewModel>(Logger);
			PacketLossFilters.Add(new PacketLossFilterViewModel(Logger) { MinValue = int.MinValue, MaxValue = int.MaxValue, Name = "Any packet loss", AnyValue = true });
			PacketLossFilters.Add(new PacketLossFilterViewModel(Logger) { MinValue = 1, MaxValue = int.MaxValue, Name = "Above 1%", IsSelected = false });
			PacketLossFilters.Add(new PacketLossFilterViewModel(Logger) { MinValue = 5, MaxValue = int.MaxValue, Name = "Above 5%", IsSelected = false });
			PacketLossFilters.Add(new PacketLossFilterViewModel(Logger) { MinValue = 10, MaxValue = int.MaxValue, Name = "Above 10%", IsSelected = false });
			PacketLossFilters.SelectedItem = PacketLossFilters.LastOrDefault();

			IPGroupFilters = new FilterViewModelCollection<IPGroupFilterViewModel>(Logger);
			SIPInterfaceFilters = new FilterViewModelCollection<SIPInterfaceFilterViewModel>(Logger);
			TermReasonFilters = new FilterViewModelCollection<TermReasonFilterViewModel>(Logger);
		}

		public bool Match(SessionViewModel Session)
		{
			DelayFilterViewModel? delayFilter;
			JitterFilterViewModel? jitterFilter;
			PacketLossFilterViewModel? packetLossFilter;


			if (!QualityFilters.Where(filter => filter.IsSelected).Select(filter => filter.Quality).Contains(Session.Quality)) return false;
			if (!IPGroupFilters.Where(filter => filter.IsSelected).Join(Session.Calls, filter => filter.Name, call => call.IPGroup, (ipGroup, call) => call).Any()) return false;
			if (!SIPInterfaceFilters.Where(filter => filter.IsSelected).Join(Session.Calls, filter => filter.Name, call => call.SIPInterfaceId, (SIPInterface, call) => call).Any()) return false;
			if (!TermReasonFilters.Where(filter => filter.IsSelected).Join(Session.Calls, filter => filter.Name, call => call.TrmReason, (TermReason, call) => call).Any()) return false;

			delayFilter = DelayFilters.SelectedItem;
			if ((delayFilter!= null) && (!delayFilter.AnyValue))
			{
				if (Session.Calls.SelectMany(call => call.MediaReports).All(mediaReport => (mediaReport.RTPdelay <= delayFilter.MinValue) || (mediaReport.RTPdelay > delayFilter.MaxValue))) return false;
			}

			jitterFilter = JitterFilters.SelectedItem;
			if ((jitterFilter != null) && (!jitterFilter.AnyValue)) 
			{
				if (Session.Calls.SelectMany(call => call.MediaReports).All(mediaReport => (mediaReport.RTPjitterms <= jitterFilter.MinValue) || (mediaReport.RTPjitterms > jitterFilter.MaxValue))) return false;
			}

			packetLossFilter = PacketLossFilters.SelectedItem;
			if ((packetLossFilter != null) && (!packetLossFilter.AnyValue))
			{
				if (Session.Calls.SelectMany(call => call.MediaReports).All(mediaReport => (mediaReport.PacketLossPercent <= packetLossFilter.MinValue) || (mediaReport.PacketLossPercent > packetLossFilter.MaxValue))) return false;
			}

			return true;
		}



	}
}
