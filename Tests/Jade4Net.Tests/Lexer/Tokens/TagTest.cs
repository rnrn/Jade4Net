using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class TagTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnIndentTag() //throws Exception
        {
            lexer = initLexer("tag.jade");
            assertToken("div");
            assertToken(":");
            assertToken("p");
            assertToken(":");
            assertToken("strong");
            assertToken("indent");
            assertToken("div");
            assertToken(":");
            assertToken("strong");
            assertToken("indent");
            assertToken("p");
            assertToken("outdent");
            assertToken("eos");
        }
    }

}