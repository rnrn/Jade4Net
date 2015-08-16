using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Jade.Filters
{
    public abstract class CachingFilter : Filter
    {
        public static readonly int MAX_ENTRIES = 1000;

        private static OrderedDictionary cache = new OrderedDictionary(MAX_ENTRIES + 1);

        public string convert(string source, Dictionary<string, object> attributes, Dictionary<string, object> model)
        {
            String key = source.GetHashCode() + "-" + attributes.GetHashCode();
            if (!cache.Contains(key))
            {
                cache.Add(key, convert(source, attributes));
            }
            return (string) cache[key];
        }

        public abstract string convert(string source, Dictionary<string, object> attributes);

    }

    public static partial class Extension
    {
        public static bool removeEldestEntry(OrderedDictionary eldest)
        {
            return eldest.Count > CachingFilter.MAX_ENTRIES;
        }
    } 
}

