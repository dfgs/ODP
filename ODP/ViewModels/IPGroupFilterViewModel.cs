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
	public class IPGroupFilterViewModel : BaseViewModel
	{
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(IPGroupFilterViewModel), new PropertyMetadata(true));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}




		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(IPGroupFilterViewModel), new PropertyMetadata(null));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}


		public IPGroupFilterViewModel(ILogger Logger) : base(Logger)
		{
		}



	}
}
