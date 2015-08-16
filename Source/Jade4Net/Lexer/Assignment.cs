using System;
using Jade.Lexer.Tokens;

namespace Jade.Lexer
{
    public class Assignment : Token
    {
        public Assignment(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }
    }
}
