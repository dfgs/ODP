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
	public abstract class BaseFilterViewModel<FilterT> : GenericViewModel<FilterT>,IFilterViewModel
	{
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(BaseFilterViewModel<FilterT>), new PropertyMetadata(true));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}



		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(BaseFilterViewModel<FilterT>), new PropertyMetadata(null));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}


		public BaseFilterViewModel(FilterT Model) : base(Model)
		{
		}



	}
}
