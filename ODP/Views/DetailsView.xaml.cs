using ODP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ODP.Views
{
	/// <summary>
	/// Logique d'interaction pour DetailsView.xaml
	/// </summary>
	public partial class DetailsView : UserControl
	{


		public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(DetailsView), new PropertyMetadata(null,SelectedObjectPropertyChanged));
		public object SelectedObject
		{
			get { return (object)GetValue(SelectedObjectProperty); }
			set { SetValue(SelectedObjectProperty, value); }
		}



		public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(ObservableCollection<PropertyViewModel>), typeof(DetailsView), new PropertyMetadata(new ObservableCollection<PropertyViewModel>()));
		public ObservableCollection<PropertyViewModel>	 Properties
		{
			get { return (ObservableCollection<PropertyViewModel>)GetValue(PropertiesProperty); }
			set { SetValue(PropertiesProperty, value); }
		}




		public DetailsView()
		{
			InitializeComponent();
		}

		private static void SelectedObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DetailsView? view;

			view = d as DetailsView;
			if (view != null) view.OnSelectedObjectChanged();
		}

		protected virtual void OnSelectedObjectChanged()
		{
			PropertyInfo[] pis;
			PropertyViewModel property;
			BrowsableAttribute? attr;

			Properties.Clear();
			if (SelectedObject == null) return;
			pis=SelectedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach(PropertyInfo pi in pis)
			{
				attr = pi.GetCustomAttribute<BrowsableAttribute>();
				if (!(attr?.Browsable??false)) continue;
				property=new PropertyViewModel() { Name=pi.Name,Value=pi.GetValue(SelectedObject) };
				Properties.Add(property);
			}
			
		}




	}
}
