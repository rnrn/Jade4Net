using System;

namespace Jade.Lexer.Tokens
{
    public abstract class Token
    {

        private String value;
        private int lineNumber;
        private bool buffer = false;
        private String mode;
        private String name;
        private int indents;
        private bool selfClosing = false;

        public Token(String value, int lineNumber)
        {
            this.value = value;
            this.lineNumber = lineNumber;
        }

        public Token(String value, int lineNumber, bool buffer)
        {
            this.value = value;
            this.lineNumber = lineNumber;
            this.buffer = buffer;
        }

        public String getValue()
        {
            return this.value;
        }

        public int getLineNumber()
        {
            return lineNumber;
        }

        public void setBuffer(bool buffer)
        {
            this.buffer = buffer;
        }

        public bool isBuffer()
        {
            return buffer;
        }

        public void setMode(String mode)
        {
            this.mode = mode;
        }

        public String getMode()
        {
            return mode;
        }

        public override String ToString()
        {
            return value;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public String getName()
        {
            return name;
        }

        public void setIndents(int indents)
        {
            this.indents = indents;
        }

        public int getIndents()
        {
            return indents;
        }

        public void setValue(String value)
        {
            this.value = value;
        }

        public bool isSelfClosing()
        {
            return selfClosing;
        }

        public void setSelfClosing(bool selfClosing)
        {
            this.selfClosing = selfClosing;
        }
    }

}
