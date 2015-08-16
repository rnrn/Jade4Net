using System;

namespace Jade.Lexer.Tokens
{
    public class Expression : Token
    {

        public Expression(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }

        private bool escape;

        public bool isEscape()
        {
            return escape;
        }

        public void setEscape(bool escape)
        {
            this.escape = escape;
        }
    }

}
