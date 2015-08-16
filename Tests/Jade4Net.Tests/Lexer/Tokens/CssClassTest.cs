using System;
using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class CssClassTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnCssClassAndCssIdTags() //throws Exception
        {
            lexer = initLexer("tag_with_css_class.jade");
            assertToken(1, typeof (Tag), "p");
            assertToken(1, typeof (CssId), "firstdiv");
            assertToken(1, typeof (CssClass), "red");
            assertToken(1, typeof (CssClass), "bold");
            assertToken(2, typeof (Indent), "indent");
            assertToken(2, typeof (Text), "Hello World");
            assertToken(3, typeof (Newline), "newline");
            assertToken(3, typeof (Text), "Hello Berlin");
            assertToken(4, typeof (Newline), "newline");
            assertToken(4, typeof (Text), "Hello Tokyo");
            assertToken(5, typeof (Outdent), "outdent");
            assertToken(5, typeof (Tag), "div");
            assertToken(5, typeof (CssClass), "content");
            assertToken(6, typeof (Indent), "indent");
            assertToken(6, typeof (Tag), "p");
            assertToken(7, typeof (Indent), "indent");
            assertToken(7, typeof (Text), "Hello World");
            assertToken(8, typeof (Indent), "indent");
            assertToken(8, typeof (Text), "Hello Berlin");
            assertToken(9, typeof (Indent), "indent");
            assertToken(9, typeof (Text), "Hello Tokyo");
            assertToken(10, typeof (Outdent), "outdent");
            assertToken(10, typeof (Tag), "div");
            assertToken(10, typeof (CssClass), "footer");
            assertToken(11, typeof (Indent), "indent");
            assertToken(11, typeof (Tag), "div");
            assertToken(11, typeof (CssId), "leftdiv");
            assertToken(11, typeof (CssClass), "left");
            assertToken(11, typeof (CssClass), "happy");
            assertToken(12, typeof (Indent), "indent");
            assertToken(12, typeof (Tag), "p");
            assertToken(12, typeof (CssClass), "red");
            assertToken(13, typeof (Indent), "indent");
            assertToken(13, typeof (Text), "Hello World");
            assertToken(14, typeof(Indent), "indent");
            assertToken(14, typeof (Tag), "p");
            assertToken(14, typeof (CssClass), "green");
            assertToken(15, typeof (Indent), "indent");
            assertToken(15, typeof (Text), "Hello Berlin");
            assertToken(16, typeof (Indent), "indent");
            assertToken(16, typeof (Text), "Hello Tokyo");
            assertToken(17, typeof (Newline), "newline");
            assertToken(18, typeof (Outdent), "outdent");
            assertToken(18, typeof (Eos), "eos");
        }

        protected new void assertToken(int expectedLineNumber, Type expectedClazz,
            String expectedValue)
        {
            Token token = lexer.advance();
            Assert.AreEqual(token.getValue(), (expectedValue));
            Assert.AreEqual(token.getLineNumber(), (expectedLineNumber));
            // assertTrue(token.getClass() == expectedClazz);
        }
    }
}