using System;
using System.Collections.Generic;
using System.Text;

namespace Jade.Util
{
    public class ArgumentSplitter
    {

        private static readonly char argumentDelimiter = ',';
        private readonly String arguments;
        private List<String> argList = new List<String>();

        /**
         * Split arguments passed as single String into list
         * @param arguments
         * @return  Parsed arguments
         */

        public static List<String> split(String arguments)
        {
            return new ArgumentSplitter(arguments).splitArguments();
        }

        private ArgumentSplitter(String arguments)
        {
            this.arguments = arguments;
        }

        private List<String> splitArguments()
        {

            int argLength = arguments.Length;
            StringBuilder sb = new StringBuilder(argLength);
            bool insideQuotas = false;
            int bracesBlock = 0;

            for (int i = 0; i < argLength; i++)
            {
                char ch = arguments[i];

                // detect when pointer is inside quoted text
                if (ch == '"' || ch == '\'')
                {
                    insideQuotas = !insideQuotas;
                }

                else if (ch == '(')
                {
                    bracesBlock++;
                }

                else if (ch == ')')
                {
                    bracesBlock--;
                }

                // detect argument delimiter, then push argument
                else if (ch == argumentDelimiter && !insideQuotas && bracesBlock == 0)
                {
                    pushArg(sb);
                    sb = new StringBuilder(argLength);
                }
                sb.Append(ch);
            }
            pushArg(sb);
            return argList;
        }

        private void pushArg(StringBuilder sb)
        {
            argList.Add(sb.ToString().Trim().replace("^,", "").Trim());
        }
    }

}