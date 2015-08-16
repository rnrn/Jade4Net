using System;
using Jade.Lexer.Tokens;
using Template;

namespace Jade.Exceptions
{
    public class JadeParserException : JadeException
    {

        //private static readonly long serialVersionUID = 2022663314591205451L;

        public JadeParserException(String filename, int lineNumber, TemplateLoader templateLoader, Type expected,
            Type got)
            : base("expected " + expected + " but got " + got, filename, lineNumber, templateLoader, null)
        {
            ;
        }

        public JadeParserException(String filename, int lineNumber, TemplateLoader templateLoader, Token token)
            : base("unknown token " + token, filename, lineNumber, templateLoader, null)
        {
            ;
        }

        public JadeParserException(String filename, int lineNumber, TemplateLoader templateLoader, String message)
            : base(message, filename, lineNumber, templateLoader, null)
        {
            ;
        }

    }

}