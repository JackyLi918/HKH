using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace System //HKH.Common
{
    public static class ObjectExtension
    {
        private static ConcurrentDictionary<string, LambdaExpression> cache_getter = new ConcurrentDictionary<string, LambdaExpression>();
        private static ConcurrentDictionary<string, LambdaExpression> cache_setter = new ConcurrentDictionary<string, LambdaExpression>();

        public static dynamic GetValue<T>(this T t, string propName) where T : class
        {
            LambdaExpression getter = BuildGetter<T>(propName);
            return getter.Compile().DynamicInvoke(t); ;
        }

        public static void SetValue<T>(this T t, string propName, dynamic value, bool defaultForInvalidValue = false) where T : class
        {
            LambdaExpression setter = BuildSetter<T>(propName);
            dynamic val = defaultForInvalidValue ? ChangeType(value, (setter.Body as BinaryExpression).Right.Type) : value;
            setter.Compile().DynamicInvoke(t, val);
        }

        internal static dynamic BuildGetter<T>(string propName) where T : class
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

                Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), body.Type);

                func = Expression.Lambda(delegateType, body, param);
                cache_getter.TryAdd(cacheKey, func);
            }

            return func;
        }

        internal static dynamic BuildSetter<T>(string propName) where T : class
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
                var body = Expression.Property(param1, propNames[0]);

                for (int i = 1; i < propNames.Length; i++)
                {
                    body = Expression.Property(body, propNames[i]);
                }

                var param2 = Expression.Parameter(body.Type, "val");
                var setter = Expression.Assign(body, param2);

                Type delegateType = typeof(Action<,>).MakeGenericType(typeof(T), body.Type);
                action = Expression.Lambda(delegateType, setter, param1, param2);
                cache_setter.TryAdd(cacheKey, action);
            }

            return action;
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

        public static T DeepClone<T>(this T obj)
        {
            if (obj is string || obj.GetType().IsValueType) return obj;

            object newObj;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                newObj = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)newObj;
        }
    }
}
