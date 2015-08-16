using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class IndentTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnIndentTag() //throws Exception
        {
            lexer = initLexer("indent_1.jade");
            assertToken("head");
            assertToken("newline");
            assertToken("body");
            assertToken("newline");
            assertToken("div");
            assertToken("indent");
            assertToken("div");
            assertToken("newline");
            assertToken("table");
            assertToken("outdent");
            assertToken("tbody");
            assertToken("newline");
            assertToken("tr");
            assertToken("newline");
            assertToken("td");
            assertToken("newline");
            assertToken("eos");
        }

        [TestMethod]
        public void shouldCorrectlyIndent() //throws Exception 
        {
            lexer = initLexer("indent_2.jade");
            assertToken("head");
            assertToken("newline");
            assertToken("body");
            assertToken("indent");
            assertToken("div");
            assertToken("newline");
            assertToken("div");
            assertToken(":");
            assertToken("p");
            assertToken(":");
            assertToken("span");
            assertToken("indent");
            assertToken("ul");
            assertToken(":");
            assertToken("li");
            assertToken("indent");
            assertToken("span");
            assertToken("indent");
            assertToken("div");
            assertToken("indent");
            assertToken("table");
            assertToken(":");
            assertToken("tbody");
            assertToken("indent");
            assertToken("tr");
            assertToken("indent");
            assertToken("td");
            assertToken("indent");
            assertToken("div");
            assertToken("indent");
            assertToken("span");
            assertToken("indent");
            assertToken("tr");
            assertToken("indent");
            assertToken("td");
            assertToken("indent");
            assertToken("div");
            assertToken("newline");
            assertToken("div");
            assertToken("outdent");
            assertToken("eos");
        }

        [TestMethod]
        public void shouldReturnAnIndentTokenIfTooManyIndentationCharacters() //throws Exception
        {
            lexer = initLexer("indent_error_1.jade");
            assertToken("head");
            assertToken("newline");
            assertToken("body");
            assertToken("newline");
            assertToken("div");
            assertToken("indent");
            assertToken("div");
        }

        [TestMethod]
        public void shouldReturnAnIndentTokenIfNotEnoughIndentationCharacters() //throws Exception
        {
            lexer = initLexer("indent_error_2.jade");
            assertToken("head");
            assertToken("newline");
            assertToken("body");
            assertToken("newline");
            assertToken("div");
            assertToken("indent");
            assertToken("div");
            assertToken("newline");
            assertToken("table");
            assertToken("outdent");
            assertToken("tbody");
        }
    }
}