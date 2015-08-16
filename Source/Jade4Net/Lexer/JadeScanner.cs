using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Jade.Compiler;

namespace Jade.Lexer
{
    public class JadeScanner
    {

        private String input;

        public JadeScanner(TextReader reader)
        {
            initFromReader(reader);
        }

        public void consume(int length)
        {
            input = input.Substring(length);
        }

        public String findInLine(String re)
        {
            Regex pattern = new Regex(re);
            var matcher = pattern.Matches(input);
            if (matcher.Count>0)
            {
                return matcher[0].Value;
            }
            return null;
        }

        private void initFromReader(TextReader reader)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                
                String s = reader.ReadLine();
                while (s != null)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        sb.Append(s);
                    }
                    sb.Append("\n");
                    s = reader.ReadLine();
                }
                input = sb.ToString();
                if (StringUtils.isNotBlank(input))
                {
                    input = sb.ToString().Replace("\\r\\n", "\\n").Replace("\\r", "\\n");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }


        public char charAt(int i)
        {
            return input[i];
        }

        public bool beginnsWithWhitespace()
        {
            return (input[0] == ' ' || input[0] == '\t');
        }

        public bool isNotEmpty()
        {
            return StringUtils.isNotEmpty(input);
        }

        private bool isEmpty()
        {
            return !isNotEmpty();
        }

        public bool isNotLineBreak()
        {
            return isEmpty() || input[0] != '\n';
        }

        public String getPipelessText()
        {
            int i = input.IndexOf('\n');
            if (-1 == i)
                i = input.Length;
            String str = input.Substring(0, i);
            consume(str.Length);
            return str.Trim();
        }

        public String getInput()
        {
            return input;
        }

        public MatchCollection getMatcherForPattern(String regexp)
        {
            Regex pattern = new Regex(regexp);
            return pattern.Matches(input);
        }
        
        public bool isIntendantionViolated()
        {
            return input != null && input.Length > 0
                    && (' ' == input[0] || '\t' == input[0]);
        }

        public bool isBlankLine()
        {
            return input != null && input.Length > 0 && '\n' == input[0];
        }

        public bool isAdditionalBlankline()
        {
            return input.Length > 2 && input[0] == '\n' && input[1] == '\n';
        }

    }

}
