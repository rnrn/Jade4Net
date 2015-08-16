using System;
using System.Collections.Generic;
using Jade.Parser.Nodes;

namespace Jade.Lexer.Tokens
{
    public class Attribute : Token
    {
        private Dictionary<String, Object> attributes = new Dictionary<String, Object>();

        public Attribute(String value, int lineNumber): base(value, lineNumber) {
           ;
        }

        public Dictionary<String, Object> getAttributes() {
            return attributes;
        }

        public void addAttribute(String name, String value) {
            attributes.Add(name, value);
        }

        public void addExpressionAttribute(String name, String expression) {
            attributes.Add(name, new ExpressionString(expression));
        }

        public void addBooleanAttribute(String name, Boolean value) {
            attributes.Add(name, value);
        }

    }

}
