using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Investments.Utils.Linq.Extensions
{
	public static class EnumerableExtenstions
	{
		public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
			where T : class
			=> source ?? Enumerable.Empty<T>();

		public static T[] OrEmpty<T>(this T[]? source)
			where T : class
			=> source ?? Array.Empty<T>();
	}
}
