using System;
using System.Linq;
using System.Text;

namespace Jade.Compiler
{
    public class StringUtils
    {

        private StringUtils() { }

        /**
         * Replaces carriage returns, newlines, tabs, formfeeds and the special chars defined in {@link Characters}
         * with their respective escape sequences.
         *
         * @param string the string
         * @return the escaped string
         */
        public static String escape(String str)
        {
            if (isEmpty(str)) return "";
            StringBuilder sb = new StringBuilder();
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == chars.Length - 1 || chars[i] != '\r' || chars[i + 1] != '\n')
                {
                    sb.Append(escape(chars[i]));
                }
            }
            return sb.ToString();
        }

        /**
         * Replaces carriage returns, newlines, tabs, formfeeds and the special chars defined in {@link Characters}
         * with their respective escape sequences.
         *
         * @param c the character to escape
         * @return the escaped string
         */
        public static String escape(char c)
        {
            if (c == '\r')
            {
                return "\\r";
            }
            else if (c == '\n')
            {
                return "\\n";
            }
            else if (c == '\t')
            {
                return "\\t";
            }
            else if (c == '\f')
            {
                return "\\f";
            }
            else if (c == global::Jade.Compiler.Chars.DEL_ERROR)
            {
                return "DEL_ERROR";
            }
            else if (c == global::Jade.Compiler.Chars.INS_ERROR)
            {
                return "INS_ERROR";
            }
            else if (c == global::Jade.Compiler.Chars.RESYNC)
            {
                return "RESYNC";
            }
            else if (c == global::Jade.Compiler.Chars.RESYNC_START)
            {
                return "RESYNC_START";
            }
            else if (c == global::Jade.Compiler.Chars.RESYNC_END)
            {
                return "RESYNC_END";
            }
            else if (c == global::Jade.Compiler.Chars.RESYNC_EOI)
            {
                return "RESYNC_EOI";
            }
            else if (c == global::Jade.Compiler.Chars.INDENT)
            {
                return "INDENT";
            }
            else if (c == global::Jade.Compiler.Chars.DEDENT)
            {
                return "DEDENT";
            }
            else if (c == global::Jade.Compiler.Chars.EOI)
            {
                return "EOI";
            }
            else
            {
                return "" + c;
            }
        }

        /**
         * Creates a string consisting of n times the given character.
         *
         * @param c the char
         * @param n the number of times to repeat
         * @return the string
         */
        public static String repeat(char c, int n)
        {
            return new string(Enumerable.Repeat(c, n).ToArray());
        }

        //***********************************************************************************************
        //**                 THE FOLLOWING CODE IS A PARTIAL, VERBATIM COPY OF                         **
        //**                       org.apache.commons.lang.StringUtils                                 **
        //**                         which is licensed under ASF 2.0                                   **
        //***********************************************************************************************

        /**
         * <p>Joins the elements of the provided <code>Iterable</code> into
         * a single String containing the provided elements.</p>
         * <p/>
         * <p>No delimiter is added before or after the list.
         * A <code>null</code> separator is the same as an empty String ("").</p>
         *
         * @param iterable the <code>Iterable</code> of values to join together, may be null
         * @param separator  the separator character to use, null treated as ""
         * @return the joined String, <code>null</code> if null iterator input
         */
        //public static String join(Iterable iterable, String separator)
        //{
        //    return iterable == null ? null : join(iterable.iterator(), separator);
        //}

        /**
         * <p>Joins the elements of the provided <code>Iterator</code> into
         * a single String containing the provided elements.</p>
         * <p/>
         * <p>No delimiter is added before or after the list.
         * A <code>null</code> separator is the same as an empty String ("").</p>
         *
         * @param iterator  the <code>Iterator</code> of values to join together, may be null
         * @param separator the separator character to use, null treated as ""
         * @return the joined String, <code>null</code> if null iterator input
         */
        //public static String join(Iterator iterator, String separator)
        //{
        //    // handle null, zero and one elements before building a buffer
        //    if (iterator == null) return null;
        //    if (!iterator.hasNext()) return "";
        //    Object first = iterator.next();
        //    if (!iterator.hasNext()) return Utils.ToString(first);

        //    // two or more elements
        //    StringBuilder buf = new StringBuilder(256); // Java default is 16, probably too small
        //    if (first != null) buf.Append(first);

        //    while (iterator.hasNext())
        //    {
        //        if (separator != null) buf.Append(separator);
        //        Object obj = iterator.next();
        //        if (obj != null) buf.Append(obj);
        //    }
        //    return buf.ToString();
        //}

        /**
         * <p>Joins the elements of the provided array into a single String
         * containing the provided list of elements.</p>
         * <p/>
         * <p>No delimiter is added before or after the list.
         * A <code>null</code> separator is the same as an empty String ("").
         * Null objects or empty strings within the array are represented by
         * empty strings.</p>
         * <p/>
         * <pre>
         * StringUtils.join(null, *)                = null
         * StringUtils.join([], *)                  = ""
         * StringUtils.join([null], *)              = ""
         * StringUtils.join(["a", "b", "c"], "--")  = "a--b--c"
         * StringUtils.join(["a", "b", "c"], null)  = "abc"
         * StringUtils.join(["a", "b", "c"], "")    = "abc"
         * StringUtils.join([null, "", "a"], ',')   = ",,a"
         * </pre>
         *
         * @param array     the array of values to join together, may be null
         * @param separator the separator character to use, null treated as ""
         * @return the joined String, <code>null</code> if null array input
         */
        public static String join(Object[] array, String separator)
        {
            return array == null ? null : join(array, separator, 0, array.Length);
        }

        /**
         * <p>Joins the elements of the provided array into a single String
         * containing the provided list of elements.</p>
         * <p/>
         * <p>No delimiter is added before or after the list.
         * A <code>null</code> separator is the same as an empty String ("").
         * Null objects or empty strings within the array are represented by
         * empty strings.</p>
         * <p/>
         * <pre>
         * StringUtils.join(null, *)                = null
         * StringUtils.join([], *)                  = ""
         * StringUtils.join([null], *)              = ""
         * StringUtils.join(["a", "b", "c"], "--")  = "a--b--c"
         * StringUtils.join(["a", "b", "c"], null)  = "abc"
         * StringUtils.join(["a", "b", "c"], "")    = "abc"
         * StringUtils.join([null, "", "a"], ',')   = ",,a"
         * </pre>
         *
         * @param array      the array of values to join together, may be null
         * @param separator  the separator character to use, null treated as ""
         * @param startIndex the first index to start joining from.  It is
         *                   an error to pass in an end index past the end of the array
         * @param endIndex   the index to stop joining from (exclusive). It is
         *                   an error to pass in an end index past the end of the array
         * @return the joined String, <code>null</code> if null array input
         */
        public static String join(Object[] array, String separator, int startIndex, int endIndex)
        {
            if (array == null) return null;
            if (separator == null) separator = "";

            // lastIndex - firstIndex > 0:   Len = NofStrings *(len(firstString) + len(separator))
            //           (Assuming that all Strings are roughly equally long)
            int bufSize = (endIndex - startIndex);
            if (bufSize <= 0) return "";
            bufSize *= ((array[startIndex] == null ? 16 : array[startIndex].ToString().Length) + separator.Length);
            StringBuilder buf = new StringBuilder(bufSize);

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i > startIndex) buf.Append(separator);
                if (array[i] != null) buf.Append(array[i]);
            }
            return buf.ToString();
        }

        // Empty checks
        //-----------------------------------------------------------------------

        /**
         * <p>Checks if a String is empty ("") or null.</p>
         * <p/>
         * <pre>
         * StringUtils.isEmpty(null)      = true
         * StringUtils.isEmpty("")        = true
         * StringUtils.isEmpty(" ")       = false
         * StringUtils.isEmpty("bob")     = false
         * StringUtils.isEmpty("  bob  ") = false
         * </pre>
         * <p/>
         *
         * @param str the String to check, may be null
         * @return <code>true</code> if the String is empty or null
         */
        public static bool isEmpty(String str)
        {
            return str == null || str.Length == 0;
        }

        /**
         * <p>Checks if a String is not empty ("") and not null.</p>
         * <p/>
         * <pre>
         * StringUtils.isNotEmpty(null)      = false
         * StringUtils.isNotEmpty("")        = false
         * StringUtils.isNotEmpty(" ")       = true
         * StringUtils.isNotEmpty("bob")     = true
         * StringUtils.isNotEmpty("  bob  ") = true
         * </pre>
         *
         * @param str the String to check, may be null
         * @return <code>true</code> if the String is not empty and not null
         */
        public static bool isNotEmpty(String str)
        {
            return !isEmpty(str);
        }

        /**
         * Gets a String's length or <code>0</code> if the String is <code>null</code>.
         *
         * @param str a String or <code>null</code>
         * @return String length or <code>0</code> if the String is <code>null</code>.
         */
        public static int length(String str)
        {
            return str == null ? 0 : str.Length;
        }

        /**
         * <p>Compares two Strings, returning <code>true</code> if they are equal ignoring
         * the case.</p>
         * <p/>
         * <p><code>null</code>s are handled without exceptions. Two <code>null</code>
         * references are considered equal. Comparison is case insensitive.</p>
         * <p/>
         * <pre>
         * StringUtils.equalsIgnoreCase(null, null)   = true
         * StringUtils.equalsIgnoreCase(null, "abc")  = false
         * StringUtils.equalsIgnoreCase("abc", null)  = false
         * StringUtils.equalsIgnoreCase("abc", "abc") = true
         * StringUtils.equalsIgnoreCase("abc", "ABC") = true
         * </pre>
         *
         * @param str1 the first String, may be null
         * @param str2 the second String, may be null
         * @return <code>true</code> if the Strings are equal, case insensitive, or
         *         both <code>null</code>
         */
        public static bool equalsIgnoreCase(String str1, String str2)
        {
            return str1 == null ? str2 == null : str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
        }

        /**
         * Test whether a string starts with a given prefix, handling null values without exceptions.
         * <p/>
         * StringUtils.StartsWith(null, null)   = false
         * StringUtils.StartsWith(null, "abc")  = false
         * StringUtils.StartsWith("abc", null)  = true
         * StringUtils.StartsWith("abc", "ab")  = true
         * StringUtils.StartsWith("abc", "abc") = true
         *
         * @param string the string
         * @param prefix the prefix
         * @return true if string starts with prefix
         */
        public static bool startsWith(String strin, String prefix)
        {
            return strin != null && (prefix == null || strin.StartsWith(prefix));
        }

        /**
         * <p>Gets a substring from the specified String avoiding exceptions.</p>
         * <p/>
         * <p>A negative start position can be used to start <code>n</code>
         * characters from the end of the String.</p>
         * <p/>
         * <p>A <code>null</code> String will return <code>null</code>.
         * An empty ("") String will return "".</p>
         * <p/>
         * <pre>
         * StringUtils.Substring(null, *)   = null
         * StringUtils.Substring("", *)     = ""
         * StringUtils.Substring("abc", 0)  = "abc"
         * StringUtils.Substring("abc", 2)  = "c"
         * StringUtils.Substring("abc", 4)  = ""
         * StringUtils.Substring("abc", -2) = "bc"
         * StringUtils.Substring("abc", -4) = "abc"
         * </pre>
         *
         * @param str   the String to get the substring from, may be null
         * @param start the position to start from, negative means
         *              count back from the end of the String by this many characters
         * @return substring from start position, <code>null</code> if null String input
         */
        public static String substring(String str, int start)
        {
            if (str == null)
            {
                return null;
            }

            // handle negatives, which means last n characters
            if (start < 0)
            {
                start = str.Length + start; // remember start is negative
            }

            if (start < 0)
            {
                start = 0;
            }
            if (start > str.Length)
            {
                return "";
            }

            return str.Substring(start);
        }

        /**
         * <p>Gets a substring from the specified String avoiding exceptions.</p>
         * <p/>
         * <p>A negative start position can be used to start/end <code>n</code>
         * characters from the end of the String.</p>
         * <p/>
         * <p>The returned substring starts with the character in the <code>start</code>
         * position and ends before the <code>end</code> position. All position counting is
         * zero-based -- i.e., to start at the beginning of the string use
         * <code>start = 0</code>. Negative start and end positions can be used to
         * specify offsets relative to the end of the String.</p>
         * <p/>
         * <p>If <code>start</code> is not strictly to the left of <code>end</code>, ""
         * is returned.</p>
         * <p/>
         * <pre>
         * StringUtils.Substring(null, *, *)    = null
         * StringUtils.Substring("", * ,  *)    = "";
         * StringUtils.Substring("abc", 0, 2)   = "ab"
         * StringUtils.Substring("abc", 2, 0)   = ""
         * StringUtils.Substring("abc", 2, 4)   = "c"
         * StringUtils.Substring("abc", 4, 6)   = ""
         * StringUtils.Substring("abc", 2, 2)   = ""
         * StringUtils.Substring("abc", -2, -1) = "b"
         * StringUtils.Substring("abc", -4, 2)  = "ab"
         * </pre>
         *
         * @param str   the String to get the substring from, may be null
         * @param start the position to start from, negative means
         *              count back from the end of the String by this many characters
         * @param end   the position to end at (exclusive), negative means
         *              count back from the end of the String by this many characters
         * @return substring from start position to end positon,
         *         <code>null</code> if null String input
         */
        public static String substring(String str, int start, int end)
        {
            if (str == null)
            {
                return null;
            }

            // handle negatives
            if (end < 0)
            {
                end = str.Length + end; // remember end is negative
            }
            if (start < 0)
            {
                start = str.Length + start; // remember start is negative
            }

            // check length next
            if (end > str.Length)
            {
                end = str.Length;
            }

            // if start is greater than end, return ""
            if (start > end)
            {
                return "";
            }

            if (start < 0)
            {
                start = 0;
            }
            if (end < 0)
            {
                end = 0;
            }

            return str.Substring(start, end);
        }

        // Left/Right/Mid
        //-----------------------------------------------------------------------

        /**
         * <p>Gets the leftmost <code>len</code> characters of a String.</p>
         * <p/>
         * <p>If <code>len</code> characters are not available, or the
         * String is <code>null</code>, the String will be returned without
         * an exception. An exception is thrown if len is negative.</p>
         * <p/>
         * <pre>
         * StringUtils.left(null, *)    = null
         * StringUtils.left(*, -ve)     = ""
         * StringUtils.left("", *)      = ""
         * StringUtils.left("abc", 0)   = ""
         * StringUtils.left("abc", 2)   = "ab"
         * StringUtils.left("abc", 4)   = "abc"
         * </pre>
         *
         * @param str the String to get the leftmost characters from, may be null
         * @param len the length of the required String, must be zero or positive
         * @return the leftmost characters, <code>null</code> if null String input
         */
        public static String left(String str, int len)
        {
            if (str == null)
            {
                return null;
            }
            if (len < 0)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, len);
        }

        /**
         * <p>Gets the rightmost <code>len</code> characters of a String.</p>
         * <p/>
         * <p>If <code>len</code> characters are not available, or the String
         * is <code>null</code>, the String will be returned without an
         * an exception. An exception is thrown if len is negative.</p>
         * <p/>
         * <pre>
         * StringUtils.right(null, *)    = null
         * StringUtils.right(*, -ve)     = ""
         * StringUtils.right("", *)      = ""
         * StringUtils.right("abc", 0)   = ""
         * StringUtils.right("abc", 2)   = "bc"
         * StringUtils.right("abc", 4)   = "abc"
         * </pre>
         *
         * @param str the String to get the rightmost characters from, may be null
         * @param len the length of the required String, must be zero or positive
         * @return the rightmost characters, <code>null</code> if null String input
         */
        public static String right(String str, int len)
        {
            if (str == null)
            {
                return null;
            }
            if (len < 0)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(str.Length - len);
        }

        /**
         * <p>Gets <code>len</code> characters from the middle of a String.</p>
         * <p/>
         * <p>If <code>len</code> characters are not available, the remainder
         * of the String will be returned without an exception. If the
         * String is <code>null</code>, <code>null</code> will be returned.
         * An exception is thrown if len is negative.</p>
         * <p/>
         * <pre>
         * StringUtils.mid(null, *, *)    = null
         * StringUtils.mid(*, *, -ve)     = ""
         * StringUtils.mid("", 0, *)      = ""
         * StringUtils.mid("abc", 0, 2)   = "ab"
         * StringUtils.mid("abc", 0, 4)   = "abc"
         * StringUtils.mid("abc", 2, 4)   = "c"
         * StringUtils.mid("abc", 4, 2)   = ""
         * StringUtils.mid("abc", -2, 2)  = "ab"
         * </pre>
         *
         * @param str the String to get the characters from, may be null
         * @param pos the position to start from, negative treated as zero
         * @param len the length of the required String, must be zero or positive
         * @return the middle characters, <code>null</code> if null String input
         */
        public static String mid(String str, int pos, int len)
        {
            if (str == null)
            {
                return null;
            }
            if (len < 0 || pos > str.Length)
            {
                return "";
            }
            if (pos < 0)
            {
                pos = 0;
            }
            if (str.Length <= (pos + len))
            {
                return str.Substring(pos);
            }
            return str.Substring(pos, len);
        }

        public static string repeat(string pattern, int count)
        {
            return string.Join("", Enumerable.Repeat(pattern, count));
        }

        public static bool isNotBlank(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        public static bool isBlank(string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}
