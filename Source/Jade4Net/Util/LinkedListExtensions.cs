using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jade.Util
{
    public static class LinkedListExtensions
    {
        public static string replaceAll(this string input, string pattern, string sample)
        {
            var regex = new Regex(pattern);
            input = regex.Replace(input, sample);
            return input;
        }

        public static string replace(this string input, string pattern, string sample)
        {
            var regex = new Regex(pattern);
            input = regex.Replace(input, sample);
            return input;
        }

        public static void AppendRange<T>(this LinkedList<T> source,
            IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                source.AddLast(item);
            }
        }

        public static void PrependRange<T>(this LinkedList<T> source,
            IEnumerable<T> items)
        {
            LinkedListNode<T> first = source.First;
            foreach (T item in items)
            {
                source.AddBefore(first, item);
            }
        }

        public static IEnumerable<T> Reverse<T>(this LinkedList<T> list)
        {
            var el = list.Last;
            while (el != null)
            {
                yield return el.Value;
                el = el.Previous;
            }
        }
    }
}