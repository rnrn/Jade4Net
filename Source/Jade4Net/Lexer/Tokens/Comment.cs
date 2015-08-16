using System;

namespace Jade.Lexer.Tokens
{
    public class Comment : Token
    {

        public Comment(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }

        public Comment(String value, int lineNumber, bool buffer)
            : base(value, lineNumber, buffer)
        {
        }
    }

}
