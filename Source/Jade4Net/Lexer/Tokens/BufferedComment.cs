using System;

namespace Jade.Lexer.Tokens
{
    public class BufferedComment : Token
    {

        public BufferedComment(String value, int lineNumber):base(value, lineNumber) {
            ;
        }

    }

}
