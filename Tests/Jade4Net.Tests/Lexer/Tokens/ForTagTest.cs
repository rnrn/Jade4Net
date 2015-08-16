using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class ForTagTest : TokenTest
    {

        [TestMethod]
        public void shouldRecognizeForToken() //throws Exception
        {
            lexer = initLexer("for_each.jade");
            assertToken(typeof(ForTag), "for");
        }
    }
}
