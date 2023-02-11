using System.Linq;
using HKH.Common;

namespace System.Collections.Generic//HKH.Common
{
	public static class IListExtension
	{
		/// <summary>
		/// remove item from ilist
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="predicate"></param>
		public static void Remove<T>(this IList<T> source, Func<T, bool> predicate)
		{
			if (source == null)
			{
				throw Error.ArgumentNull("source");
			}
			if (predicate == null)
			{
				throw Error.ArgumentNull("predicate)");
			}
			for (int i = source.Count - 1; i > -1; i--)
			{
				if (predicate(source[i]))
					source.RemoveAt(i);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static T TryAdd<T>(this IList<T> source, T t)
		{
			return TryAdd<T>(source, t, s => s.Equals(t));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="t"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static T TryAdd<T>(this IList<T> source, T t, Func<T, bool> predicate)
		{
			if (source.Any(predicate))
			{
				return source.FirstOrDefault(predicate);
			}
			else
			{
				source.Add(t);
				return t;
			}
		}
	}
}
