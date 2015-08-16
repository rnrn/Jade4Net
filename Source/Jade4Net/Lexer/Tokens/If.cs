using System;

namespace Jade.Lexer.Tokens
{
    public class If : Token
    {

        public If(String value, int lineNumber)
            : base(value, lineNumber)
        {
        }

        private bool inverseCondition = false;
        private bool alternativeCondition = false;

        public bool isInverseCondition()
        {
            return inverseCondition;
        }

        public void setInverseCondition(bool inverseCondition)
        {
            this.inverseCondition = inverseCondition;
        }

        public bool isAlternativeCondition()
        {
            return alternativeCondition;
        }

        public void setAlternativeCondition(bool alternativeCondition)
        {
            this.alternativeCondition = alternativeCondition;
        }

    }

}
