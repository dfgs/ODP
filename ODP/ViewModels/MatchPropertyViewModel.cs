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
	public class MatchPropertyViewModel : BaseViewModel
	{


		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(MatchPropertyViewModel), new PropertyMetadata(null));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(MatchProperty), typeof(MatchPropertyViewModel), new PropertyMetadata(null));
		public MatchProperty Value
		{
			get { return (MatchProperty)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public MatchPropertyViewModel() : base()
		{
		}
	}
}
