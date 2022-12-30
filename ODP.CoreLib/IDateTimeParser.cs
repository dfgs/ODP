using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public interface IDateTimeParser
	{
		DateTime? Parse(string? Input);
	}
}
