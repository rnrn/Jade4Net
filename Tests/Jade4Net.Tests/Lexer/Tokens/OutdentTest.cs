using System;
using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class OutdentTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnCssClassAndCssIdTags() //throws Exception 
        {
            lexer = initLexer("outdent1.jade");
            assertToken(1, typeof (Tag), "div");
            assertToken(1, typeof (CssClass), "footer");
            assertToken(2, typeof (Indent), "indent");
            assertToken(2, typeof (Tag), "div");
            assertToken(2, typeof (CssId), "leftdiv");
            assertToken(2, typeof (CssClass), "left");
            assertToken(2, typeof (CssClass), "happy");
            assertToken(3, typeof (Indent), "indent");
            assertToken(3, typeof (Tag), "p");
            assertToken(3, typeof (CssClass), "red");
            assertToken(4, typeof (Indent), "indent");
            assertToken(4, typeof (Text), "Hello World");
            assertToken(5, typeof (Outdent), "outdent");
            assertToken(5, typeof (Tag), "p");
            assertToken(5, typeof (CssClass), "green");
            assertToken(6, typeof (Indent), "indent");
            assertToken(6, typeof (Text), "Hello Berlin");
            assertToken(7, typeof (Newline), "newline");
            assertToken(7, typeof (Text), "Hello Tokyo");
            assertToken(8, typeof (Newline), "newline");
            assertToken(9, typeof (Outdent), "outdent");
            assertToken(9, typeof (Eos), "eos");
        }

        protected override void assertToken(int expectedLineNumber, Type expectedClazz, String expectedValue)
        {
            Token token = lexer.next();
            Assert.AreEqual(token.getValue(), (expectedValue));
            Assert.AreEqual(token.getLineNumber(), (expectedLineNumber));
            Assert.AreEqual(token.GetType(), expectedClazz);
        }
    }

}