using ODP.CoreLib;
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

		public static int MaxPacketLoss(this IEnumerable<CallViewModel> Calls)
		{
			if (Calls.SelectMany(call => call.MediaReports).Any()) return Calls.SelectMany(call => call.MediaReports).Select(mediaReport => mediaReport.PacketLossPercent ?? 0).Max();
			else return 0;
		}
		public static int MaxDelay(this IEnumerable<CallViewModel> Calls)
		{
			if (Calls.SelectMany(call => call.MediaReports).Any()) return Calls.SelectMany(call => call.MediaReports).Select(mediaReport => mediaReport.RTPdelay ?? 0).Max();
			else return 0;
		}
		public static int MaxJitter(this IEnumerable<CallViewModel> Calls)
		{
			if (Calls.SelectMany(call => call.MediaReports).Any()) return Calls.SelectMany(call => call.MediaReports).Select(mediaReport => mediaReport.RTPjitter ?? 0).Max();
			else return 0;
		}

		public static double AvgPacketLoss(this IEnumerable<CallViewModel> Calls)
		{
			if (Calls.SelectMany(call => call.MediaReports).Any()) return Calls.SelectMany(call => call.MediaReports).Select(mediaReport => mediaReport.PacketLossPercent ?? 0).Average();
			else return 0;
		}
		public static double AvgDelay(this IEnumerable<CallViewModel> Calls)
		{
			if (Calls.SelectMany(call => call.MediaReports).Any()) return Calls.SelectMany(call => call.MediaReports).Select(mediaReport => mediaReport.RTPdelay ?? 0).Average();
			else return 0;

		}
		public static double AvgJitter(this IEnumerable<CallViewModel> Calls)
		{
			if (Calls.SelectMany(call => call.MediaReports).Any())  return Calls.SelectMany(call => call.MediaReports).Select(mediaReport => mediaReport.RTPjitter ?? 0).Average();
			else return 0;
		}


		public static IEnumerable<string> SIPInterfaces(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.Select(call => call.SIPInterfaceId).Where(item => item != null).Cast<string>().Distinct();
		}

		public static IEnumerable<CallViewModel> Calls(this IEnumerable<SessionViewModel> Sessions)
		{
			return Sessions.SelectMany(session => session.Calls); 
		}
		public static IEnumerable<MediaReportViewModel> MediaReports(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.SelectMany(call => call.MediaReports);
		}

	}
}
