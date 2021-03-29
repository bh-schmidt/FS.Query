using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FS.Query.Helpers.Extensions
{
    public static class IDictionaryExtensions
    {
        [return: NotNullIfNotNull("getValue")]
        public static TValue? Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue>? getValue = null)
        {
            if (!dictionary.TryGetValue(key, out TValue? value) && getValue is not null)
            {
                value = getValue();
                dictionary.Add(key, value);
            }

            return value;
        }
    }
}
