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

		public static long Sample(this DateTime Date, long SampleSize)
		{
			return Date.Ticks / SampleSize * SampleSize;
		}
		public static IEnumerable<Sample<T>> Sample<TIn, T>(this IEnumerable<TIn> Items, long SampleSize, Func<TIn, DateTime?> DateSelector, Func<TIn, T> ValueGenerator)
		{
			DateTime? dateTime;
			T value;
			long ticks;

			foreach (TIn item in Items)
			{
				dateTime = DateSelector(item);
				if (!dateTime.HasValue) 
					continue;
				ticks = dateTime.Value.Sample(SampleSize);
				value = ValueGenerator(item);
				yield return new Sample<T>(ticks, value);
			}
		}

		public static IEnumerable<Sample<TOut>> AggregateAndOrder<T, TOut>(this IEnumerable<Sample<T>> Samples, Func<IEnumerable<Sample<T>>, TOut> AggregateFunction)
		{
			foreach (IGrouping<long, Sample<T>> sampleGroup in Samples.GroupBy(item => item.Ticks).OrderBy(item => item.Key))
			{
				yield return new Sample<TOut>(sampleGroup.Key, AggregateFunction(sampleGroup));
			}
		}

		public static long[] GenerateSampledPosition<T>(this IEnumerable<Sample<T>> Samples,long SampleSize)
		{
			long count;
			long min,max;

			min = Samples.Min(item => item.Ticks);
			max = Samples.Max(item => item.Ticks);

			count = (max - min) / SampleSize;
			if (count == 0) count = 1;

			long[] values= Enumerable.Range(0, (int)count).Select((index) => (min + index*SampleSize)).ToArray();
			return values;
		}

		public static IEnumerable<int> Accumulate(this IEnumerable<int> Items, int Seed)
		{
			int total;

			total = 0;
			foreach (int item in Items)
			{
				total += item;
				yield return total;
			}

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
