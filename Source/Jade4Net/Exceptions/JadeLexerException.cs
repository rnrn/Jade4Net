using System;
using Template;

namespace Jade.Exceptions
{
    public class JadeLexerException : JadeException
    {

        //private static readonly long serialVersionUID = -4390591022593362563L;

        public JadeLexerException(String message, String filename, int lineNumber, TemplateLoader templateLoader)
            :base(message, filename, lineNumber, templateLoader, null) {
            ;
        }
    }

}
