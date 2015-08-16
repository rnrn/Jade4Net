using System;
using Jade.Lexer.Tokens;

namespace Jade.Lexer
{
    public class Each : Token
    {

        private String code;
        private String key;

        public Each(String value, int lineNumber)
            :base(value, lineNumber) {
           ;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getCode() {
            return code;
        }

        public String getKey() {
            return key;
        }

        public void setKey(String key) {
            this.key = key;
        }
    }

}
