using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	internal static class AsyncEnumerableExtensions
	{
		public struct AsyncEnumerable<T> : IAsyncEnumerable<T>
		{
			private readonly IEnumerable<T> enumerable;

			public AsyncEnumerable(IEnumerable<T> enumerable)
			{
				this.enumerable = enumerable;
			}

			public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
			{
				return new AsyncEnumerator<T>(enumerable.GetEnumerator());
			}
		}

		public struct AsyncEnumerator<T> : IAsyncEnumerator<T>
		{
			private readonly IEnumerator<T> enumerator;

			public AsyncEnumerator(IEnumerator<T> enumerator)
			{
				this.enumerator = enumerator;
			}

			public ValueTask DisposeAsync()
			{
				enumerator?.Dispose();
				return default;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				return new ValueTask<bool>(enumerator == null ? false : enumerator.MoveNext());
			}

			public T Current => enumerator.Current;
		}

		public static AsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> that)
		{
			return new AsyncEnumerable<T>(that);
		}

		public static AsyncEnumerator<T> AsAsyncEnumerator<T>(this IEnumerator<T> that)
		{
			return new AsyncEnumerator<T>(that);
		}
	}

}
