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
	public class QualityFilterViewModel : BaseFilterViewModel<string>
	{

		public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("Quality", typeof(Quality), typeof(QualityFilterViewModel), new PropertyMetadata(Quality.NA));
		public Quality Quality
		{
			get { return (Quality)GetValue(QualityProperty); }
			set { SetValue(QualityProperty, value); }
		}


		public QualityFilterViewModel() : base("")
		{
		}



	}
}
