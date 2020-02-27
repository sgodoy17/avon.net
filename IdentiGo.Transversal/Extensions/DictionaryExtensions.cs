using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Transversal.Extensions
{
    public static class DictionaryEstensions
    {
        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary.Any())
                return "{" + string.Join(",", dictionary.Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + "}";
            return "{NO-DATA}";
        }

        public static string ToDebugString(this IDictionary dictionary)
        {
            var typedDictionary = dictionary.Cast<DictionaryEntry>();
            if (typedDictionary.Any())
                return "{" + string.Join(",", dictionary.Cast<DictionaryEntry>().Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + "}";
            return "{NO-DATA}";
        }
    }
}
