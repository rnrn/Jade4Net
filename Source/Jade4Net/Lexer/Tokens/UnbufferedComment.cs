using System;

namespace Jade.Lexer.Tokens
{
    public class UnbufferedComment : Token
    {

        public UnbufferedComment(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }

         
    }

}
