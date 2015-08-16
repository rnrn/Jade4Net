using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Jade.Expression;
using Jade.Model;
using Jade.Parser.Nodes;

namespace Jade.Compiler
{
    public class Utils
    {
        public static Regex interpolationPattern = new Regex("(\\\\)?([#!])\\{(.*?)\\}");

        public static List<Object> prepareInterpolate(string str, bool xmlEscape)
        {
            int start = 0;
            var result = new List<Object>();

            foreach (Match matcher in interpolationPattern.Matches(str))
            {
                string before = str.Substring(start, matcher.Index - start);
                if (xmlEscape)
                {
                    before = escapeHTML(before);
                }
                result.Add(before);

                bool escape = matcher.Groups[1].Length < 1;
                string flag = matcher.Groups[2].Value;
                string code = matcher.Groups[3].Value;

                if (escape)
                {
                    string escapedExpression = matcher.Groups[0].Value.Substring(1);
                    if (xmlEscape)
                    {
                        escapedExpression = escapeHTML(escapedExpression);
                    }
                    result.Add(escapedExpression);
                }
                else
                {
                    ExpressionString expression = new ExpressionString(code);
                    if (flag.Equals("#"))
                    {
                        expression.setEscape(true);
                    }
                    result.Add(expression);
                }
                start = matcher.Index + matcher.Value.Length;
                var match = matcher.NextMatch();
            }
            string last = str.Substring(start);
            if (xmlEscape)
            {
                last = escapeHTML(last);
            }
            result.Add(last);

            return result;
        }

        public static string interpolate(List<Object> prepared, JadeModel model)
        {
            StringBuilder result = new StringBuilder();

            foreach (Object entry in prepared)
            {
                if (entry is String)
                {
                    result.Append(entry);
                }
                else if (entry is ExpressionString)
                {
                    ExpressionString expression = (ExpressionString) entry;
                    string stringValue = "";
                    string value = ExpressionHandler.evaluateStringExpression(expression.getValue(), model);
                    if (value != null)
                    {
                        stringValue = value;
                    }
                    if (expression.isEscape())
                    {
                        stringValue = escapeHTML(stringValue);
                    }
                    result.Append(stringValue);
                }
            }

            return result.ToString();
        }

        public static string escapeHTML(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string interpolate(string str, JadeModel model, bool escape)
        {
            return interpolate(prepareInterpolate(str, escape), model);
        }
    }
}
