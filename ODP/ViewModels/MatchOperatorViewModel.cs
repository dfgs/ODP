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
	public class MatchOperatorViewModel : BaseViewModel
	{


		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(MatchOperatorViewModel), new PropertyMetadata(null));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(MatchOperator), typeof(MatchOperatorViewModel), new PropertyMetadata(null));
		public MatchOperator Value
		{
			get { return (MatchOperator)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public MatchOperatorViewModel() : base()
		{
		}
	}
}
