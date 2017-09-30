/*******************************************************
 * Filename: QueryableExtension.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	07/29/2015 11:40:10 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq //HKH.Common
{
	/// <summary>
	/// QueryableExtension
	/// </summary>
	public static class QueryableExtension
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryable"></param>
		/// <param name="propName"> Property.Property such as User.Name </param>
		/// <param name="desc"></param>
		/// <returns></returns>
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propName, bool desc) where T : class
		{
			Expression<Func<T,object>> keySelector = ObjectExtension.BuildGetter<T>(propName);
			return desc ? Queryable.OrderByDescending<T, object>(queryable, keySelector) : Queryable.OrderBy<T, object>(queryable, keySelector);
		}
	}
}