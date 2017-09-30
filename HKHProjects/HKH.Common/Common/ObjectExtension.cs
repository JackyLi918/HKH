using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System //HKH.Common
{
	public static class ObjectExtension
	{
		private static ConcurrentDictionary<string, LambdaExpression> cache_getter = new ConcurrentDictionary<string, LambdaExpression>();
		private static ConcurrentDictionary<string, LambdaExpression> cache_setter = new ConcurrentDictionary<string, LambdaExpression>();

		public static object GetValue<T>(this T t, string propName) where T : class
		{
			return BuildGetter<T>(propName).Compile()(t);
		}

		public static void SetValue<T>(this T t, string propName, object value, bool defaultForInvalidValue = false) where T : class
		{
			Expression<Action<T, object>> setter = BuildSetter<T>(propName);
			if (defaultForInvalidValue)
				setter.Compile()(t, ChangeType(value, (setter.Body as BinaryExpression).Right.Type));
			else
				setter.Compile()(t, value);
		}

		internal static Expression<Func<T, object>> BuildGetter<T>(string propName) where T : class
		{
			if (string.IsNullOrEmpty(propName))
				throw new ArgumentException("propName can't be null.");

			string[] propNames = propName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (propNames.Length == 0)
				throw new ArgumentException("propName can't be empty.");

			LambdaExpression func = null;
			Type t = typeof(T);
			string cacheKey = t.FullName + "_" + propName;

			if (!cache_getter.TryGetValue(cacheKey, out func))
			{
				var param = Expression.Parameter(t, "p");
				var body = Expression.Property(param, propNames[0]);

				for (int i = 1; i < propNames.Length; i++)
				{
					body = Expression.Property(body, propNames[i]);
				}

				func = Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), param);
				cache_getter.TryAdd(cacheKey, func);
			}

			return func as Expression<Func<T, object>>;
		}

		internal static Expression<Action<T, object>> BuildSetter<T>(string propName) where T : class
		{
			if (string.IsNullOrEmpty(propName))
				throw new ArgumentException("propName can't be null.");

			string[] propNames = propName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (propNames.Length == 0)
				throw new ArgumentException("propName can't be empty.");

			LambdaExpression action = null;
			Type t = typeof(T);
			string cacheKey = t.FullName + "_" + propName;

			if (!cache_setter.TryGetValue(cacheKey, out action))
			{
				var param1 = Expression.Parameter(t, "p");
				var param2 = Expression.Parameter(typeof(object), "val");
				var body = Expression.Property(param1, propNames[0]);

				for (int i = 1; i < propNames.Length; i++)
				{
					body = Expression.Property(body, propNames[i]);
				}

				var setter = Expression.Assign(body, Expression.Convert(param2, body.Type));
				action = Expression.Lambda<Action<T, object>>(setter, param1, param2);
				cache_setter.TryAdd(cacheKey, action);
			}

			return action as Expression<Action<T, object>>;
		}

		internal static object ChangeType(object value, Type conversionType)
		{
			Type targetType = conversionType;
			if (conversionType.IsValueType && conversionType.IsGenericType)
			{
				//int?/double?/datetime? 
				targetType = conversionType.GetGenericArguments()[0];
			}

			object result = null;
			try
			{
				result = Convert.ChangeType(value, targetType);
			}
			catch//InvalidCastException
			{
				//eat all convert exceptions
			}

			if (result == null && conversionType.IsValueType)
			{
				result = Activator.CreateInstance(conversionType);
			}

			return result;
		}

		internal static List<string> DumpCache(bool getter, bool setter)
		{
			List<string> lst = new List<string>();

			if (getter)
			{
				foreach (var kvp in cache_getter)
				{
					lst.Add(kvp.Key + " : " + kvp.Value.ToString());
				}
			}

			if (setter)
			{
				foreach (var kvp in cache_setter)
				{
					lst.Add(kvp.Key + " : " + kvp.Value.ToString());
				}
			}

			return lst;
		}
	}
}
