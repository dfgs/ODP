﻿using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Logique d'interaction pour CallsView.xaml
	/// </summary>
	public partial class CallsView : UserControl
	{
		public CallsView()
		{
			InitializeComponent();
		}

		private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DetailsWindow window;

			window= new DetailsWindow();
			window.DataContext = (sender as ListView)!.SelectedItem;
			window.Show();
        }
    }
}
