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
	public class JitterFilterViewModel : BaseFilterViewModel
	{

		public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(JitterFilterViewModel), new PropertyMetadata(0));
		public int MinValue
		{
			get { return (int)GetValue(MinValueProperty); }
			set { SetValue(MinValueProperty, value); }
		}
		public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(JitterFilterViewModel), new PropertyMetadata(0));
		public int MaxValue
		{
			get { return (int)GetValue(MaxValueProperty); }
			set { SetValue(MaxValueProperty, value); }
		}

		public static readonly DependencyProperty AnyValueProperty = DependencyProperty.Register("AnyValue", typeof(bool), typeof(JitterFilterViewModel), new PropertyMetadata(false));
		public bool AnyValue
		{
			get { return (bool)GetValue(AnyValueProperty); }
			set { SetValue(AnyValueProperty, value); }
		}
		public JitterFilterViewModel(ILogger Logger) : base(Logger)
		{
		}



	}
}
