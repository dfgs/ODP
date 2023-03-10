using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class DateTimeParser : IDateTimeParser
	{
		public DateTime? Parse(string? Input)
		{
			if (string.IsNullOrEmpty(Input)) return null;
			return DateTime.ParseExact(Input, "HH:mm:ss.fff  'UTC' ddd MMM dd yyyy", CultureInfo.InvariantCulture);
		}
	}
}
