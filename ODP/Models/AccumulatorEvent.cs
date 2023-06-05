using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP
{
	public struct AccumulatorEvent
	{
		public long Ticks
		{
			get;
		}
		public int Delta
		{
			get;
		}

		public AccumulatorEvent(long Ticks, int Delta)
		{
			this.Ticks = Ticks;	this.Delta = Delta;	
		}

	}
}
