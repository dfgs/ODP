using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ODP.ViewModels
{
	public interface IFilterViewModel
	{
		public bool IsSelected
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
	}
}
