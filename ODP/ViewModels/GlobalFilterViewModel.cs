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
	public class GlobalFilterViewModel : BaseViewModel
	{



		public static readonly DependencyProperty QualityFiltersProperty = DependencyProperty.Register("QualityFilters", typeof(FilterViewModelCollection<string, QualityFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, QualityFilterViewModel> QualityFilters
		{
			get { return (FilterViewModelCollection<string, QualityFilterViewModel>)GetValue(QualityFiltersProperty); }
			set { SetValue(QualityFiltersProperty, value); }
		}
		public static readonly DependencyProperty IPGroupFiltersProperty = DependencyProperty.Register("IPGroupFilters", typeof(FilterViewModelCollection<string, IPGroupFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, IPGroupFilterViewModel> IPGroupFilters
		{
			get { return (FilterViewModelCollection<string, IPGroupFilterViewModel>)GetValue(IPGroupFiltersProperty); }
			set { SetValue(IPGroupFiltersProperty, value); }
		}
		public static readonly DependencyProperty SIPInterfaceFiltersProperty = DependencyProperty.Register("SIPInterfaceFilters", typeof(FilterViewModelCollection<string, SIPInterfaceFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, SIPInterfaceFilterViewModel> SIPInterfaceFilters
		{
			get { return (FilterViewModelCollection<string, SIPInterfaceFilterViewModel>)GetValue(SIPInterfaceFiltersProperty); }
			set { SetValue(SIPInterfaceFiltersProperty, value); }
		}
		public static readonly DependencyProperty TermReasonFiltersProperty = DependencyProperty.Register("TermReasonFilters", typeof(FilterViewModelCollection<string, TermReasonFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, TermReasonFilterViewModel> TermReasonFilters
		{
			get { return (FilterViewModelCollection<string, TermReasonFilterViewModel>)GetValue(TermReasonFiltersProperty); }
			set { SetValue(TermReasonFiltersProperty, value); }
		}


		public static readonly DependencyProperty DelayFiltersProperty = DependencyProperty.Register("DelayFilters", typeof(FilterViewModelCollection<string, DelayFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, DelayFilterViewModel> DelayFilters
		{
			get { return (FilterViewModelCollection<string, DelayFilterViewModel>)GetValue(DelayFiltersProperty); }
			set { SetValue(DelayFiltersProperty, value); }
		}

		public static readonly DependencyProperty JitterFiltersProperty = DependencyProperty.Register("JitterFilters", typeof(FilterViewModelCollection<string, JitterFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, JitterFilterViewModel> JitterFilters
		{
			get { return (FilterViewModelCollection<string, JitterFilterViewModel>)GetValue(JitterFiltersProperty); }
			set { SetValue(JitterFiltersProperty, value); }
		}


		public static readonly DependencyProperty PacketLossFiltersProperty = DependencyProperty.Register("PacketLossFilters", typeof(FilterViewModelCollection<string, PacketLossFilterViewModel>), typeof(GlobalFilterViewModel), new PropertyMetadata(null));
		public FilterViewModelCollection<string, PacketLossFilterViewModel> PacketLossFilters
		{
			get { return (FilterViewModelCollection<string, PacketLossFilterViewModel>)GetValue(PacketLossFiltersProperty); }
			set { SetValue(PacketLossFiltersProperty, value); }
		}




		public GlobalFilterViewModel() : base()
		{
			QualityFilters = new FilterViewModelCollection<string, QualityFilterViewModel>();
			QualityFilters.Add(new QualityFilterViewModel() { Quality = Quality.Bad, Name = "Bad quality" });
			QualityFilters.Add(new QualityFilterViewModel() { Quality = Quality.Average, Name = "Average quality" });
			QualityFilters.Add(new QualityFilterViewModel() { Quality = Quality.Good, Name = "Good quality" });
			QualityFilters.Add(new QualityFilterViewModel() { Quality = Quality.NA, Name = "Not applicable" });

			DelayFilters = new FilterViewModelCollection<string, DelayFilterViewModel>();
			DelayFilters.Add(new DelayFilterViewModel() { MinValue = int.MinValue, MaxValue = int.MaxValue, Name = "Any delay", AnyValue = true });
			DelayFilters.Add(new DelayFilterViewModel() { MinValue = 150, MaxValue = int.MaxValue, Name = "Above 150 ms", IsSelected = false });
			DelayFilters.Add(new DelayFilterViewModel() { MinValue = 250, MaxValue = int.MaxValue, Name = "Above 250 ms", IsSelected = false });
			DelayFilters.Add(new DelayFilterViewModel() { MinValue = 350, MaxValue = int.MaxValue, Name = "Above 350 ms", IsSelected = false });
			DelayFilters.SelectedItem = DelayFilters.LastOrDefault();

			JitterFilters = new FilterViewModelCollection<string, JitterFilterViewModel>();
			JitterFilters.Add(new JitterFilterViewModel() { MinValue = int.MinValue, MaxValue = int.MaxValue, Name = "Any jitter", AnyValue = true });
			JitterFilters.Add(new JitterFilterViewModel() { MinValue = 20, MaxValue = int.MaxValue, Name = "Above 20 ms", IsSelected = false });
			JitterFilters.Add(new JitterFilterViewModel() { MinValue = 30, MaxValue = int.MaxValue, Name = "Above 30 ms", IsSelected = false });
			JitterFilters.Add(new JitterFilterViewModel() { MinValue = 50, MaxValue = int.MaxValue, Name = "Above 50 ms", IsSelected = false });
			JitterFilters.SelectedItem = JitterFilters.LastOrDefault();

			PacketLossFilters = new FilterViewModelCollection<string, PacketLossFilterViewModel>();
			PacketLossFilters.Add(new PacketLossFilterViewModel() { MinValue = int.MinValue, MaxValue = int.MaxValue, Name = "Any packet loss", AnyValue = true });
			PacketLossFilters.Add(new PacketLossFilterViewModel() { MinValue = 1, MaxValue = int.MaxValue, Name = "Above 1%", IsSelected = false });
			PacketLossFilters.Add(new PacketLossFilterViewModel() { MinValue = 5, MaxValue = int.MaxValue, Name = "Above 5%", IsSelected = false });
			PacketLossFilters.Add(new PacketLossFilterViewModel() { MinValue = 10, MaxValue = int.MaxValue, Name = "Above 10%", IsSelected = false });
			PacketLossFilters.SelectedItem = PacketLossFilters.LastOrDefault();

			IPGroupFilters = new FilterViewModelCollection<string, IPGroupFilterViewModel>();
			SIPInterfaceFilters = new FilterViewModelCollection<string, SIPInterfaceFilterViewModel>();
			TermReasonFilters = new FilterViewModelCollection<string, TermReasonFilterViewModel>();
		}
		public bool Match(CDRSBCReportViewModel CDRSBCReport)
		{
			if (!IPGroupFilters.Where(filter => (filter.IsSelected) && (filter.Value == CDRSBCReport.IPGroup)).Any()) return false;

			if (!SIPInterfaceFilters.Where(filter => (filter.IsSelected) && (filter.Value == CDRSBCReport.SIPInterfaceId)).Any()) return false;
			if (!TermReasonFilters.Where(filter => (filter.IsSelected) && (filter.Value == CDRSBCReport.TrmReason)).Any()) return false;

			return true;
		}

		public bool Match(CallViewModel Call)
		{
			DelayFilterViewModel? delayFilter;
			JitterFilterViewModel? jitterFilter;
			PacketLossFilterViewModel? packetLossFilter;


			if (!QualityFilters.Where(filter => filter.IsSelected).Select(filter => filter.Quality).Contains(Call.Quality)) return false;

			if (!Call.SBCReports.Where(report => Match(report)).Any()) return false;

			delayFilter = DelayFilters.SelectedItem;
			if ((delayFilter != null) && (!delayFilter.AnyValue))
			{
				if (Call.MediaReports.All(mediaReport => (mediaReport.RTPdelay <= delayFilter.MinValue) || (mediaReport.RTPdelay > delayFilter.MaxValue))) return false;
			}

			jitterFilter = JitterFilters.SelectedItem;
			if ((jitterFilter != null) && (!jitterFilter.AnyValue))
			{
				if (Call.MediaReports.All(mediaReport => (mediaReport.RTPjitterms <= jitterFilter.MinValue) || (mediaReport.RTPjitterms > jitterFilter.MaxValue))) return false;
			}

			packetLossFilter = PacketLossFilters.SelectedItem;
			if ((packetLossFilter != null) && (!packetLossFilter.AnyValue))
			{
				if (Call.MediaReports.All(mediaReport => (mediaReport.PacketLossPercent <= packetLossFilter.MinValue) || (mediaReport.PacketLossPercent > packetLossFilter.MaxValue))) return false;
			}

			return true;
		}

		public bool Match(SessionViewModel Session)
		{

			return Session.Calls.Where(call => Match(call)).Any();
		}



	}
}
