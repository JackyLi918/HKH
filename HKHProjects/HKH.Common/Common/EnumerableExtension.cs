using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using HKH.Common;

namespace System.Linq//HKH.Common
{
    public static class EnumerableExtension
    {
        #region Do Action
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="doWork"></param>
        public static void Do<T>(this IEnumerable<T> source, Action<T> doWork)
        {
            Do<T>(source, t => true, doWork);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="doWork"></param>
        public static void Do<T>(this IEnumerable<T> source, Func<T, bool> predicate, Action<T> doWork)
        {
            if (source == null)
            {
                throw Error.ArgumentNull("source");
            }
            if (predicate == null)
            {
                throw Error.ArgumentNull("predicate)");
            }
            if (doWork == null)
            {
                throw Error.ArgumentNull("doWork)");
            }

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    if (predicate(current))
                        doWork(current);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="doWork"></param>
        public static void DoAsync<T>(this IEnumerable<T> source, Action<T> doWork)
        {
            DoAsync<T>(source, t => true, doWork);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="doWork"></param>
        public static void DoAsync<T>(this IEnumerable<T> source, Func<T, bool> predicate, Action<T> doWork)
        {
            if (source == null)
            {
                throw Error.ArgumentNull("source");
            }
            if (predicate == null)
            {
                throw Error.ArgumentNull("predicate)");
            }
            if (doWork == null)
            {
                throw Error.ArgumentNull("doWork)");
            }

            Parallel.ForEach(source, current =>
            {
                if (predicate(current))
                    doWork(current);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="doWork"></param>
        public static void DoFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate, Action<T> doWork)
        {
            if (doWork == null)
            {
                throw Error.ArgumentNull("func)");
            }

            T t = source.FirstOrDefault(predicate);
            if (t != null)
                doWork(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="toConcatEnumerables"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params IEnumerable<T>[] toConcatEnumerables)
        {
            return Concat<T>(source, t => true, toConcatEnumerables);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="toConcatEnumerables"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, Func<T, bool> predicate, params IEnumerable<T>[] toConcatEnumerables)
        {
            if (source == null)
            {
                throw Error.ArgumentNull("source");
            }

            if (predicate == null)
            {
                throw Error.ArgumentNull("predicate)");
            }

            foreach (T tObj in source)
            {
                if (predicate(tObj))
                    yield return tObj;
            }

            if (toConcatEnumerables != null && toConcatEnumerables.Length > 0)
            {
                foreach (IEnumerable<T> enumerable in toConcatEnumerables)
                {
                    foreach (T tObj in enumerable)
                    {
                        if (predicate(tObj))
                            yield return tObj;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSub">must inherit from TBase</typeparam>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TBase> ConvertTo<TSub, TBase>(this IEnumerable<TSub> source)
        {
            return ConvertTo<TSub, TBase>(source, t => true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSub">must inherit from TBase</typeparam>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TBase> ConvertTo<TSub, TBase>(this IEnumerable<TSub> source, Func<TSub, bool> predicate)
        {
            Type baseType = typeof(TBase);
            Type subType = typeof(TSub);

            TypeConverter tc = TypeDescriptor.GetConverter(baseType);

            if (!tc.CanConvertFrom(subType))
            {
                throw new Exception(subType.ToString() + " can't be converted to " + baseType.ToString());
            }

            if (source == null)
            {
                throw Error.ArgumentNull("source");
            }

            if (predicate == null)
            {
                throw Error.ArgumentNull("predicate)");
            }

            using (IEnumerator<TSub> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TSub current = enumerator.Current;
                    if (predicate(current))
                        yield return (TBase)tc.ConvertFrom(current);
                }
            }
        }

        public static IEnumerable<TResult> Cast<T, TResult>(this IEnumerable<T> source, Func<T, TResult> predicate)
        {
            if (source == null)
            {
                throw Error.ArgumentNull("source");
            }

            if (predicate == null)
            {
                throw Error.ArgumentNull("predicate)");
            }

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return predicate(enumerator.Current);
                }
            }
        }
        #endregion

        #region Sort

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, string propName, bool desc) where T : class
        {
            return OrderBy(enumerable, propName, (desc ? SortDirection.Descending : SortDirection.Ascending));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value">field desc/asc</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, string value) where T : class
        {
            string[] arr = value.Split(' ');
            SortDirection direction = SortDirection.Ascending;
            if (arr.Length > 1 && "DESC".EqualsIgnoreCase(arr[1]))
                direction = SortDirection.Descending;
            return OrderBy(enumerable, arr[0], direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, string propName, SortDirection direction) where T : class
        {
            LambdaExpression keySelector = ObjectExtension.BuildGetter<T>(propName);
            string methodName = direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";

            MethodInfo method = typeof(Enumerable).GetMethods().Single(a => a.Name == methodName && a.IsGenericMethodDefinition && a.GetGenericArguments().Length == 2 && a.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), keySelector.ReturnType);

            return (IOrderedEnumerable<T>)method.Invoke(null, new object[] { enumerable, keySelector.Compile() });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> enumerable, string propName, bool desc) where T : class
        {
            return ThenBy(enumerable, propName, (desc ? SortDirection.Descending : SortDirection.Ascending));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value">field desc/asc</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> enumerable, string value) where T : class
        {
            string[] arr = value.Split(' ');
            SortDirection direction = SortDirection.Ascending;
            if (arr.Length > 1 && "DESC".EqualsIgnoreCase(arr[1]))
                direction = SortDirection.Descending;
            return ThenBy(enumerable, arr[0], direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="propName"> Property.Property such as User.Name </param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> enumerable, string propName, SortDirection direction) where T : class
        {
            LambdaExpression keySelector = ObjectExtension.BuildGetter<T>(propName);
            string methodName = direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";

            MethodInfo method = typeof(Enumerable).GetMethods().Single(a => a.Name == methodName && a.IsGenericMethodDefinition && a.GetGenericArguments().Length == 2 && a.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), keySelector.ReturnType);

            return (IOrderedEnumerable<T>)method.Invoke(null, new object[] { enumerable, keySelector.Compile() });
        }

        #endregion
    }
}
