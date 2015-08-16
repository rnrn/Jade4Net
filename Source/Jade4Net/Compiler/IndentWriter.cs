using System;
using System.Diagnostics;
using System.IO;

namespace Jade.Compiler
{
    public class IndentWriter
    {
        private int indent = 0;
        private bool useIndent = false;
        private bool empty = true;
        private TextWriter writer;

        public IndentWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public IndentWriter add(String str)
        {
            return append(str);
        }

        public IndentWriter append(String str)
        {
            write(str);
            return this;
        }

        public void increment()
        {
            indent++;
        }

        public void decrement()
        {
            indent--;
        }

        private void write(String str)
        {
            try
            {
                writer.Write(str);
                empty = false;
            }
            catch (IOException e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public override String ToString()
        {
            return writer.ToString();
        }

        public void newline()
        {
            if (useIndent && !empty)
            {
                write("\n" + StringUtils.repeat("  ", indent));
            }
        }

        public void setUseIndent(bool useIndent)
        {
            this.useIndent = useIndent;
        }
    }
}
