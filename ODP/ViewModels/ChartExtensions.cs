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

		public static IEnumerable<AccumulatorEvent> SelectAccumulate<T>(this IEnumerable<T> Items, int Seed, long SampleTicks, Func<T, DateTime?> StartTimeSelector, Func<T, DateTime?> StopTimeSelector)
		{
			DateTime? startTime, stopTime;
			long startTick,stopTick;
			List<AccumulatorEvent> events;
			int total;

			events= new List<AccumulatorEvent>();
			foreach(T item in Items)
			{
				startTime= StartTimeSelector(item);
				stopTime= StopTimeSelector(item);
				if ((!startTime.HasValue) || (!stopTime.HasValue)) continue;

				startTick = (startTime.Value.Ticks / SampleTicks)* SampleTicks;
				stopTick= (stopTime.Value.Ticks/ SampleTicks)* SampleTicks;

				events.Add(new AccumulatorEvent(startTick, 1));
				events.Add(new AccumulatorEvent(stopTick, -1));
			}

			total = 0;
			foreach (IGrouping<long, AccumulatorEvent> eventGroup in events.GroupBy(item => item.Ticks).OrderBy(item => item.Key))
			{
				total += eventGroup.Sum(item => item.Delta);
				yield return new AccumulatorEvent(eventGroup.Key, total);
			}

			yield break;
		}

	

		public static IEnumerable<IEnumerable<T>> GroupByQuality<T>(this IEnumerable<T> Items,IEnumerable<Quality> Qualities)
			where T : IQualityProvider
		{
			return Qualities.GroupJoin(Items, quality => quality, item => item.Quality, (quality, item) => item);
		}
		public static IEnumerable<IEnumerable<CallViewModel>> GroupByInterface(this IEnumerable<CallViewModel> Calls, IEnumerable<string> SIPInterfaces)
		{
			return SIPInterfaces.GroupJoin(Calls, sipInterface => sipInterface, call => call.SIPInterfaceId, (sipInterface, call) => call);
		}
		public static IEnumerable<CallViewModel> WithAudio(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.Where(call => call.HasMediaReport);
		}
		public static IEnumerable<CDRMediaReportViewModel> WithValidDelay(this IEnumerable<CDRMediaReportViewModel> Reports)
		{
			return Reports.Where(report => report.HasValidDelay);
		}
		public static IEnumerable<CDRMediaReportViewModel> WithValidjitter(this IEnumerable<CDRMediaReportViewModel> Reports)
		{
			return Reports.Where(report => report.HasValidJitter);
		}
		public static IEnumerable<CDRMediaReportViewModel> WithValidPacketLoss(this IEnumerable<CDRMediaReportViewModel> Reports)
		{
			return Reports.Where(report => report.HasValidPacketLoss);
		}
		public static IEnumerable<SessionViewModel> WithValidTimeStamps(this IEnumerable<SessionViewModel> Reports)
		{
			return Reports.Where(session => session.StartTime.HasValue && session.StopTime.HasValue);
		}


		public static T MaxOrDefault<T>(this IEnumerable<T> Items, T DefaultValue)
		{
			if (Items.Any()) return Items.Max() ?? DefaultValue;
			else return DefaultValue;
		}
		public static T MinOrDefault<T>(this IEnumerable<T> Items, T DefaultValue)
		{
			if (Items.Any()) return Items.Min() ?? DefaultValue;
			else return DefaultValue;
		}
		public static double AverageOrDefault(this IEnumerable<double> Items, double DefaultValue)
		{
			if (Items.Any()) return Items.Average();
			else return DefaultValue;
		}

		public static double AverageOrDefault(this IEnumerable<int> Items, double DefaultValue)
		{
			if (Items.Any()) return Items.Average();
			else return DefaultValue;
		}

		public static double AverageOrDefault(this IEnumerable<long> Items, double DefaultValue)
		{
			if (Items.Any()) return Items.Average();
			else return DefaultValue;
		}


		public static double MaxPacketLoss(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.WithAudio().SelectMany(call => call.MediaReports.WithValidPacketLoss()).Select(mediaReport => mediaReport.PacketLossPercent ?? 0).MaxOrDefault(0);
		}
		public static int MaxDelay(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.WithAudio().SelectMany(call => call.MediaReports.WithValidDelay()).Select(mediaReport => mediaReport.RTPdelay ?? 0).MaxOrDefault(0);
		}
		public static double MaxJitter(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.WithAudio().SelectMany(call => call.MediaReports.WithValidjitter()).Select(mediaReport => mediaReport.RTPjitterms ?? 0).MaxOrDefault(0);
		}

		public static double AvgPacketLoss(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.WithAudio().SelectMany(call => call.MediaReports.WithValidPacketLoss()).Select(mediaReport => mediaReport.PacketLossPercent??0).AverageOrDefault(0);
		}
		public static double AvgDelay(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.WithAudio().SelectMany(call => call.MediaReports.WithValidDelay()).Select(mediaReport => mediaReport.RTPdelay ?? 0).AverageOrDefault(0d);

		}
		public static double AvgJitter(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.WithAudio().SelectMany(call => call.MediaReports.WithValidjitter()).Select(mediaReport => mediaReport.RTPjitterms ?? 0d).AverageOrDefault(0d);
		}


		public static IEnumerable<string> SIPInterfaces(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.Select(call => call.SIPInterfaceId).Where(item => item != null).Cast<string>().Distinct();
		}

		public static IEnumerable<CallViewModel> Calls(this IEnumerable<SessionViewModel> Sessions)
		{
			return Sessions.SelectMany(session => session.Calls); 
		}
		public static IEnumerable<CDRMediaReportViewModel> MediaReports(this IEnumerable<CallViewModel> Calls)
		{
			return Calls.SelectMany(call => call.MediaReports);
		}

	}
}
