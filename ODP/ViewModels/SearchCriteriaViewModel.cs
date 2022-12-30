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
	public class SearchCriteriaViewModel : BaseViewModel
	{


		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(SearchCriteriaViewModel), new PropertyMetadata(null));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(SearchCriteria), typeof(SearchCriteriaViewModel), new PropertyMetadata(null));
		public SearchCriteria Value
		{
			get { return (SearchCriteria)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public SearchCriteriaViewModel() : base(NullLogger.Instance)
		{
		}
	}
}
