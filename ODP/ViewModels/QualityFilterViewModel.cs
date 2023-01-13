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
	public class QualityFilterViewModel : BaseViewModel
	{
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(QualityFilterViewModel), new PropertyMetadata(true));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}



		public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("Quality", typeof(Quality), typeof(QualityFilterViewModel), new PropertyMetadata(Quality.NA));
		public Quality Quality
		{
			get { return (Quality)GetValue(QualityProperty); }
			set { SetValue(QualityProperty, value); }
		}



		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(QualityFilterViewModel), new PropertyMetadata(null));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}


		public QualityFilterViewModel(ILogger Logger) : base(Logger)
		{
		}



	}
}
