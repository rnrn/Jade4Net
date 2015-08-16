using System;

namespace Jade.Parser.Nodes
{
    public class ExpressionString
    {
        private String value = null;
        private bool escape = false;

        public bool isEscape()
        {
            return escape;
        }

        public ExpressionString(String value)
        {
            this.value = value;
        }

        public String getValue()
        {
            return value;
        }

        public void setValue(String value)
        {
            this.value = value;
        }

        public void setEscape(bool escape)
        {
            this.escape = escape;
        }
    }
}
