using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP
{
	public struct Sample<T>
	{
		public long Ticks
		{
			get;
		}
		public T Value
		{
			get;
		}

		public Sample(long Ticks, T Value)
		{
			this.Ticks = Ticks;	this.Value = Value;	
		}

		public override string ToString()
		{
			return $"({Ticks},{Value})";
		}

	}
}
