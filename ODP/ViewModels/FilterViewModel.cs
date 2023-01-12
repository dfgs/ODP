using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class FilterViewModel : BaseViewModel
	{

		public static readonly DependencyProperty MatchPropertyProperty = DependencyProperty.Register("MatchProperty", typeof(MatchPropertyViewModel), typeof(FilterViewModel), new PropertyMetadata(null));
		public MatchPropertyViewModel MatchProperty
		{
			get { return (MatchPropertyViewModel)GetValue(MatchPropertyProperty); }
			set { SetValue(MatchPropertyProperty, value); }
		}


		public static readonly DependencyProperty MatchOperatorProperty = DependencyProperty.Register("MatchOperator", typeof(MatchOperatorViewModel), typeof(FilterViewModel), new PropertyMetadata(null));
		public MatchOperatorViewModel MatchOperator
		{
			get { return (MatchOperatorViewModel)GetValue(MatchOperatorProperty); }
			set { SetValue(MatchOperatorProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(FilterViewModel), new PropertyMetadata(null));
		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}



		public FilterViewModel() : base(NullLogger.Instance)
		{
		}

		public bool Match(SessionViewModel Session)
		{
			if (Session == null) return false;

			
			switch (MatchProperty.Value)
			{
				case ODP.ViewModels.MatchProperty.SessionID: 
					switch(MatchOperator.Value)
					{
						case ViewModels.MatchOperator.Contains:return Session.SessionID?.Contains(Value)??false;
						case ViewModels.MatchOperator.EqualTo: return Session.SessionID?.Equals(Value) ?? false;
						case ViewModels.MatchOperator.DifferentThan: return !Session.SessionID?.Equals(Value) ?? false;
						case ViewModels.MatchOperator.LowerThan: return Comparer<string>.Default.Compare(Session.SessionID,Value)<0;
						case ViewModels.MatchOperator.GreaterThan: return Comparer<string>.Default.Compare(Session.SessionID, Value) > 0;
						case ViewModels.MatchOperator.LowerOrEqualThan: return Comparer<string>.Default.Compare(Session.SessionID, Value) <= 0;
						case ViewModels.MatchOperator.GreaterOrEqualThan: return Comparer<string>.Default.Compare(Session.SessionID, Value) >= 0;
						default:return false;
					}
				case ODP.ViewModels.MatchProperty.CallID: return Session.Calls.FirstOrDefault(call => call.SIPCallID?.Contains(Value) ?? false) != null;
				case ODP.ViewModels.MatchProperty.IPGroup: return Session.Calls.FirstOrDefault(call => call.IPGroup?.Contains(Value) ?? false) != null;
				case ODP.ViewModels.MatchProperty.SIPInterface: return Session.Calls.FirstOrDefault(call => call.SIPInterfaceId?.Contains(Value) ?? false) != null;


				case ODP.ViewModels.MatchProperty.SrcURI:
					switch (MatchOperator.Value)
					{
						case ViewModels.MatchOperator.Contains: return Session.SrcURI?.Contains(Value) ?? false;
						case ViewModels.MatchOperator.EqualTo: return Session.SrcURI?.Equals(Value) ?? false;
						case ViewModels.MatchOperator.DifferentThan: return !Session.SrcURI?.Equals(Value) ?? false;
						case ViewModels.MatchOperator.LowerThan: return Comparer<string>.Default.Compare(Session.SrcURI, Value) < 0;
						case ViewModels.MatchOperator.GreaterThan: return Comparer<string>.Default.Compare(Session.SrcURI, Value) > 0;
						case ViewModels.MatchOperator.LowerOrEqualThan: return Comparer<string>.Default.Compare(Session.SrcURI, Value) <= 0;
						case ViewModels.MatchOperator.GreaterOrEqualThan: return Comparer<string>.Default.Compare(Session.SrcURI, Value) >= 0;
						default: return false;
					}
				case ODP.ViewModels.MatchProperty.DstURI:
					switch (MatchOperator.Value)
					{
						case ViewModels.MatchOperator.Contains: return Session.DstURI?.Contains(Value) ?? false;
						case ViewModels.MatchOperator.EqualTo: return Session.DstURI?.Equals(Value) ?? false;
						case ViewModels.MatchOperator.DifferentThan: return !Session.DstURI?.Equals(Value) ?? false;
						case ViewModels.MatchOperator.LowerThan: return Comparer<string>.Default.Compare(Session.DstURI, Value) < 0;
						case ViewModels.MatchOperator.GreaterThan: return Comparer<string>.Default.Compare(Session.DstURI, Value) > 0;
						case ViewModels.MatchOperator.LowerOrEqualThan: return Comparer<string>.Default.Compare(Session.DstURI, Value) <= 0;
						case ViewModels.MatchOperator.GreaterOrEqualThan: return Comparer<string>.Default.Compare(Session.DstURI, Value) >= 0;
						default: return false;
					}
				case ODP.ViewModels.MatchProperty.Quality:
					Quality convertedValue;
					if (!Enum.TryParse<Quality>(Value, out convertedValue)) return false;
					switch (MatchOperator.Value)
					{
						case ViewModels.MatchOperator.Contains: return Session.Quality.Equals(convertedValue);
						case ViewModels.MatchOperator.EqualTo: return Session.Quality.Equals(convertedValue);
						case ViewModels.MatchOperator.DifferentThan: return !Session.Quality.Equals(convertedValue);
						case ViewModels.MatchOperator.LowerThan: return Comparer<Quality>.Default.Compare(Session.Quality, convertedValue) < 0;
						case ViewModels.MatchOperator.GreaterThan: return Comparer<Quality>.Default.Compare(Session.Quality, convertedValue) > 0;
						case ViewModels.MatchOperator.LowerOrEqualThan: return Comparer<Quality>.Default.Compare(Session.Quality, convertedValue) <= 0;
						case ViewModels.MatchOperator.GreaterOrEqualThan: return Comparer<Quality>.Default.Compare(Session.Quality, convertedValue) >= 0;
						default: return false;
					}
				default: return false;
			}

		}



	}
}
