/*******************************************************
 * Filename: QueryableExtension.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	07/29/2015 11:40:10 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Linq.Expressions;
using HKH.Common;

namespace System.Linq //HKH.Common
{
    #region Sort
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
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propName, bool desc) where T : class
        {
            return OrderBy(queryable, propName, (desc ? SortDirection.Descending : SortDirection.Ascending));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="queryable"></param>
        /// <param name="value">field desc/asc</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string value) where T : class
        {
            string[] arr = value.Split(' ');
            SortDirection direction = SortDirection.Ascending;
            if (arr.Length > 1 && "DESC".EqualsIgnoreCase(arr[1]))
                direction = SortDirection.Descending;
            return OrderBy(queryable, arr[0], direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propName, SortDirection direction) where T : class
        {
            var keySelector = ObjectExtension.BuildGetter<T>(propName);
            string methodName = direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";

            MethodCallExpression methodCall = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), (keySelector as LambdaExpression).ReturnType }, queryable.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)queryable.Provider.CreateQuery<T>(methodCall);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string propName, bool desc) where T : class
        {
            return ThenBy(queryable, propName, (desc ? SortDirection.Descending : SortDirection.Ascending));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="queryable"></param>
        /// <param name="value">field desc/asc</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string value) where T : class
        {
            string[] arr = value.Split(' ');
            SortDirection direction = SortDirection.Ascending;
            if (arr.Length > 1 && "DESC".EqualsIgnoreCase(arr[1]))
                direction = SortDirection.Descending;
            return ThenBy(queryable, arr[0], direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string propName, SortDirection direction) where T : class
        {
            var keySelector = ObjectExtension.BuildGetter<T>(propName);
            string methodName = direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";
            
            MethodCallExpression methodCall = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), (keySelector as LambdaExpression).ReturnType }, queryable.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)queryable.Provider.CreateQuery<T>(methodCall);
        }

        #endregion
    }
}