using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.ViewModels
{
	public static class ChartExtensions
	{
		public static IEnumerable<IEnumerable<T>> GroupByQuality<T>(this IEnumerable<T> Items,IEnumerable<Quality> Qualities)
			where T : IQualityProvider
		{
			return Qualities.GroupJoin(Items, quality => quality, item => item.Quality, (quality, item) => item);
		}
		public static IEnumerable<IEnumerable<CallViewModel>> GroupByInterface(this IEnumerable<CallViewModel> Items, IEnumerable<string> SIPInterfaces)
		{
			return SIPInterfaces.GroupJoin(Items, sipInterface => sipInterface, item => item.SIPInterfaceId, (sipInterface, item) => item);
		}
	}
}
