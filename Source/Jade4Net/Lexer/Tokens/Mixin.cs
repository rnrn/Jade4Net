using System;

namespace Jade.Lexer.Tokens
{
    public class Mixin : Token
    {

        public Mixin(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }

        private String arguments;

        public void setArguments(String args)
        {
            this.arguments = args;
        }

        public String getArguments()
        {
            return arguments;
        }
    }

}
