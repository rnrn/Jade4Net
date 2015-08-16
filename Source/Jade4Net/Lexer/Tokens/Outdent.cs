using System;

namespace Jade.Lexer.Tokens
{
    public class Outdent : Token
    {

        public Outdent(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }

         
    }
}