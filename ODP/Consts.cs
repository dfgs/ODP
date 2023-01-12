using ODP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP
{
	public static class Consts
	{

		public static readonly List<MatchPropertyViewModel> MatchProperties;
		public static readonly List<MatchOperatorViewModel> MatchOperators;

		static Consts()
		{
			MatchProperties = new List<MatchPropertyViewModel>();
			MatchProperties.Add(new MatchPropertyViewModel() { Value = ViewModels.MatchProperty.SessionID, Name = "Session ID" });
			MatchProperties.Add(new MatchPropertyViewModel() { Value = ViewModels.MatchProperty.CallID, Name = "Call ID" });
			MatchProperties.Add(new MatchPropertyViewModel() { Value = ViewModels.MatchProperty.SrcURI, Name = "Source URI" });
			MatchProperties.Add(new MatchPropertyViewModel() { Value = ViewModels.MatchProperty.DstURI, Name = "Destination URI" });
			MatchProperties.Add(new MatchPropertyViewModel() { Value = ViewModels.MatchProperty.Quality, Name = "Quality" });

			MatchOperators= new List<MatchOperatorViewModel>();
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.Contains, Name = "≈" });
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.EqualTo, Name = "=" });
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.DifferentThan, Name = "≠" });
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.LowerThan, Name = "<" });
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.GreaterThan, Name = ">" });
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.LowerOrEqualThan, Name = "≤" });
			MatchOperators.Add(new MatchOperatorViewModel() { Value = ViewModels.MatchOperator.GreaterOrEqualThan, Name = "≥" });

}
	}
}
