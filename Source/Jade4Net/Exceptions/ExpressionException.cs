using System;

namespace Jade.Exceptions
{
    public class ExpressionException : Exception
    {
        //private static readonly long serialVersionUID = 1201110801125266239L;

        public ExpressionException(String expression, Exception e) 
            :base("unable to evaluate [" + expression.Trim() + "]", e)
        {
            ;
        }

        public ExpressionException(String message):base(message) {
            ;
        }
    }
}