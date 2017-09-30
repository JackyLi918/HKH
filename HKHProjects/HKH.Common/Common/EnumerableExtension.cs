using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using HKH.Common;
using System.Threading.Tasks;

namespace System.Linq//HKH.Common
{
    public static class EnumerableExtension
    {
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
    }
}
