using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class IdTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnIndentTag() //throws Exception 
        {
            lexer = initLexer("tag_with_id.jade");
            assertToken(1, typeof (Tag), "p");
            assertToken(1, typeof (CssId), "red");
            assertToken(3, typeof (Indent), "indent");
            assertToken(3, typeof (Text), "Hello World");
            assertToken(4, typeof(Newline), "newline");
            assertToken(5, typeof(Newline), "newline");
            assertToken(5, typeof(Text), "Hello Berlin");
            assertToken(7, typeof (Newline), "newline");
            assertToken(7, typeof (Text), "Hello Tokyo");
            assertToken(8, typeof (Outdent), "outdent");

            assertToken(8, typeof (Tag), "div");
            assertToken(8, typeof (CssId), "content");
            assertToken(11, typeof (Indent), "indent");
            assertToken("p");
            assertToken("indent");
            assertToken("Hello World");
            assertToken("indent");
            assertToken("Hello Berlin");
            assertToken("indent");
            assertToken("Hello Tokyo");
            assertToken("outdent");


            assertToken("div");
            assertToken("footer");
            assertToken("indent");
            assertToken("div");
            assertToken("left");
            assertToken("indent");
            assertToken("p");
            assertToken("red");
            assertToken("indent");
            assertToken("Hello World");
            assertToken("indent");
            assertToken("p");
            assertToken("green");
            assertToken("indent");
            assertToken("Hello Berlin");
            assertToken("indent");
            assertToken("Hello Tokyo");
            assertToken("newline");
            assertToken("outdent");
            assertToken("eos");
        }
    }

}