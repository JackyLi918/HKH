namespace System.Collections.Generic//HKH.Common
{
    public static class DictionaryExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                return false;
            else
            {
                dictionary.Add(key, value);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="src"></param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool TryGetKey<TKey, TValue>(this Dictionary<TKey, TValue> src, TValue value, out TKey key)
        {
            key = default(TKey);
            bool result = false;

            foreach (KeyValuePair<TKey, TValue> kvp in src)
                if (kvp.Value.Equals(value))
                {
                    key = kvp.Key;
                    result = true;
                    break;
                }

            return result;
        }
    }
}
