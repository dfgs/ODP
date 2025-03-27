using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ODP
{
	public static class RoutedCommands
	{
		public static RoutedCommand AddFile = new RoutedCommand();
		public static RoutedCommand AddWiresharkFile = new RoutedCommand();
		public static RoutedCommand Refresh = new RoutedCommand();
		public static RoutedCommand OK = new RoutedCommand();
		public static RoutedCommand Cancel = new RoutedCommand();
		public static RoutedCommand AddFilter = new RoutedCommand();
		public static RoutedCommand EditFilter = new RoutedCommand();
		public static RoutedCommand DeleteFilter = new RoutedCommand();
		public static RoutedCommand Find = new RoutedCommand();
		public static RoutedCommand FindPrevious = new RoutedCommand();
		public static RoutedCommand FindNext = new RoutedCommand();
		public static RoutedCommand Help = new RoutedCommand();
		public static RoutedCommand ApplyFilter = new RoutedCommand();
		public static RoutedCommand Maximize = new RoutedCommand();
        public static RoutedCommand OpenCallCharts = new RoutedCommand();
        public static RoutedCommand PlayRTP = new RoutedCommand();
        public static RoutedCommand StopRTP= new RoutedCommand();
    }
}
