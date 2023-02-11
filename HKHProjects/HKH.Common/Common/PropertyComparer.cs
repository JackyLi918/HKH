using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace HKH.Common
{
	public class PropertyComparer<T> : IComparer<T>, IEqualityComparer<T>
	{
		private SortDirection direction = SortDirection.Ascending;
		private Type tType = null;

		/// <summary>
		/// property to compare
		/// </summary>
		public PropertyInfo Property { get; set; }

		public PropertyComparer()
		{
			tType = typeof(T);
		}

		public PropertyComparer(string pName, SortDirection direction = SortDirection.Ascending)
			: this()
		{
			Property = tType.GetProperty(pName);
			this.direction = direction;
		}

		public PropertyComparer(PropertyInfo pi, SortDirection direction = SortDirection.Ascending)
			: this()
		{
			Property = pi;
			this.direction = direction;
		}

		public PropertyComparer(Expression<Func<T, object>> exp, SortDirection direction = SortDirection.Ascending)
			: this()
		{
			if (exp.Body is MemberExpression)
			{
				Property = (exp.Body as MemberExpression).Member as PropertyInfo;
			}

			if (Property == null)
				throw new ArgumentException("PropertyExpression is required.");

			this.direction = direction;
		}

		#region IComparer<T> Members

		public int Compare(T x, T y)
		{
			if (Property == null)
				throw new ArgumentNullException("CompareProperty can't be null.");

			object xValue = Property.GetValue(x, null);
			object yValue = Property.GetValue(y, null);

			int result;

			if (xValue == null)
			{
				if (yValue == null)
					result = 0;
				else
					result = -1;

			}
			else
			{
				if (yValue == null)
					result = 1;
				else
				{
					if (xValue.Equals(yValue))
					{
						result = 0;
					}
					else if (xValue is IComparable)
					{
						result = ((IComparable)xValue).CompareTo(yValue);
					}
					else
					{
						//generic type reflection
						Type comparableType = Type.GetType("System.IComparable`1");
						Type comparableOfTType = comparableType.MakeGenericType(Property.PropertyType);

						if (comparableOfTType.IsAssignableFrom(Property.PropertyType))
						{
							result = (int)comparableOfTType.GetMethod("CompareTo").Invoke(xValue, new object[] { yValue });
						}
						else
							throw new NotImplementedException("Type of special property does't implement ICompare/ICompare<T>.");
					}
				}
			}

			return result * ((int)direction);
		}

		#endregion

		#region IEqualityComparer<T> Members

		public bool Equals(T x, T y)
		{
			return Compare(x, y) == 0;
		}

		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}

	public enum SortDirection
	{
		Ascending = 1,
		Descending = -1
	}

	public class PropertyComparerList<T> : IComparer<T>, IEqualityComparer<T>
	{
		private List<PropertyComparer<T>> pcList = new List<PropertyComparer<T>>();

		public PropertyComparerList(params PropertyComparer<T>[] pcs)
		{
			if (pcs != null && pcs.Length > 0)
			{
				foreach (var pc in pcs)
					pcList.Add(pc);
			}
		}

		public void Add(string pName, SortDirection direction = SortDirection.Ascending)
		{
			PropertyComparer<T> pc = new PropertyComparer<T>(pName, direction);
			Add(pc);
		}

		public void Add(PropertyInfo pi, SortDirection direction = SortDirection.Ascending)
		{
			PropertyComparer<T> pc = new PropertyComparer<T>(pi, direction);
			Add(pc);
		}

		public void Add(Expression<Func<T, object>> exp, SortDirection direction = SortDirection.Ascending)
		{
			PropertyComparer<T> pc = new PropertyComparer<T>(exp, direction);
			Add(pc);
		}

		public void Add(PropertyComparer<T> pc)
		{
			pcList.Add(pc);
		}

		#region IComparer<T> Members

		public int Compare(T x, T y)
		{
			if (pcList.Count == 0)
				throw new Exception("ComaprePropertyList is empty.");

			int result = 0;

			foreach (PropertyComparer<T> pc in pcList)
			{
				result = pc.Compare(x, y);
				if (result == 0)
					continue;
				else
					break;
			}

			return result;
		}

		#endregion

		#region IEqualityComparer<T> Members

		public bool Equals(T x, T y)
		{
			return Compare(x, y) == 0;
		}

		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}
