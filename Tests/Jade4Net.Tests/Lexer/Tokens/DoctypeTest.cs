using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class DoctypeTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnIndentTag() //throws Exception
        {
            lexer = initLexer("doctype.jade");
            Token nextToken = lexer.next();
            Assert.AreEqual(nextToken.getValue(),  "strict");
            Assert.AreEqual(nextToken.GetType(), typeof (Doctype));

            nextToken = lexer.next();
            Assert.AreEqual(nextToken.getValue(), "newline");
            Assert.AreEqual(nextToken.GetType(), typeof (Newline));

            nextToken = lexer.next();
            Assert.AreEqual(nextToken.getValue(),  "eos");
            Assert.AreEqual(nextToken.GetType(), typeof (Eos));
        }
    }
}
