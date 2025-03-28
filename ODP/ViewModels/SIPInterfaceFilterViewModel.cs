﻿using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class SIPInterfaceFilterViewModel : BaseFilterViewModel<string>
	{

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(SIPInterfaceFilterViewModel), new PropertyMetadata(null));
		public string? Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public SIPInterfaceFilterViewModel() : base("")
		{
		}



	}
}
