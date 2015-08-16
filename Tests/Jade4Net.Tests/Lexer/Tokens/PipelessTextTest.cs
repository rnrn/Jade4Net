using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class PipelessTextTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnIndentTag()
        {
            lexer = initLexer("pipeless_text.jade");
            Token token = lexer.next();
            Assert.AreEqual(token.getValue(), ("p"));
            token = lexer.next();
            Assert.AreEqual(token.getValue(), ("Hallo Welt"));
            Assert.AreEqual(lexer.next().getValue(), ("newline"));
            token = lexer.next();
            Assert.AreEqual(token.getValue(), ("p"));
            token = lexer.next();
            Assert.AreEqual(token.getValue(), (" Hallo Jade"));
            Assert.AreEqual(lexer.next().getValue(), ("newline"));
            token = lexer.next();
            Assert.AreEqual(token.getValue(), ("p"));
            token = lexer.next();
            Assert.AreEqual(token.getValue(), ("  Hallo Jade"));
            Assert.AreEqual(lexer.next().getValue(), ("newline"));
            Assert.AreEqual(lexer.next().getValue(), ("eos"));
        }
    }

}