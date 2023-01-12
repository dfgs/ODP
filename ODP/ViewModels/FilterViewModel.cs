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
	public class FilterViewModel : BaseViewModel
	{

		public static readonly DependencyProperty MatchPropertyProperty = DependencyProperty.Register("MatchProperty", typeof(MatchPropertyViewModel), typeof(FilterViewModel), new PropertyMetadata(null));
		public MatchPropertyViewModel MatchProperty
		{
			get { return (MatchPropertyViewModel)GetValue(MatchPropertyProperty); }
			set { SetValue(MatchPropertyProperty, value); }
		}


		public static readonly DependencyProperty MatchOperatorProperty = DependencyProperty.Register("MatchOperator", typeof(MatchOperatorViewModel), typeof(FilterViewModel), new PropertyMetadata(null));
		public MatchOperatorViewModel MatchOperator
		{
			get { return (MatchOperatorViewModel)GetValue(MatchOperatorProperty); }
			set { SetValue(MatchOperatorProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(FilterViewModel), new PropertyMetadata(null));
		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}



		public FilterViewModel() : base(NullLogger.Instance)
		{
		}

		
	}
}
